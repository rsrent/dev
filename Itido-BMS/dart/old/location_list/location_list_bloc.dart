import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import '../refreshable.dart';
import '../searchable.dart';
import '../selectable.dart';
import 'package:bms_dart/src/repositories/location_repository.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class LocationListBloc extends Bloc<LocationListEvent, ListState<Location>>
    with Refreshable, Searchable, Selectable<Location> {
  final LocationRepository _locationRepository =
      repositoryProvider.locationRepository();

  LocationListBloc(this._refreshEvent) {
    refresh();
  }

  @override
  ListState<Location> get initialState => Loading();

  @override
  Stream<ListState<Location>> mapEventToState(
    LocationListEvent event,
  ) async* {
    if (event is LocationListFetchAll) {
      _locationRepository
          .fetchAllLocations()
          .then((locs) => dispatch(LocationsFetched(locations: locs)));
    }
    if (event is LocationListFetchOfCustomer) {
      _locationRepository
          .fetchAllLocationsOfCustomer(event.customerId)
          .then((locs) => dispatch(LocationsFetched(locations: locs)));
    }

    if (event is LocationListFetchOfUser) {
      _locationRepository
          .fetchAllLocationsOfUser(event.userId)
          .then((locs) => dispatch(LocationsFetched(locations: locs)));
    }

    if (event is LocationListFetchNotOfUser) {
      _locationRepository
          .fetchAllLocationsNotOfUser(event.userId)
          .then((locs) => dispatch(LocationsFetched(locations: locs)));
    }

    if (event is SearchTextUpdated) {
      searchText = event.searchText;
      dispatch(LocationsFetched(locations: loaded));
    }

    if (event is LocationsFetched) {
      loaded = event.locations;
      if (event.locations != null) {
        var items = filtered(searchText, filters);

        var oldState = currentState;
        if (oldState is Loaded<Location>) {
          yield Loaded<Location>(
            items: items,
            refreshTime: DateTime.now(),
            selectable: oldState.selectable,
            selectedItems: oldState.selectedItems,
          );
        } else {
          yield Loaded(items: items, refreshTime: DateTime.now());
        }
      } else {
        yield Failure();
      }
    }

    if (event is ToggleSelectable) {
      var oldState = currentState;
      if (oldState is Loaded<Location>) {
        yield Loaded<Location>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: !oldState.selectable,
          selectedItems: [],
        );
      } else {
        yield Loaded<Location>(
          items: [],
          refreshTime: DateTime.now(),
          selectable: true,
          selectedItems: [],
        );
      }
    }

    if (event is LocationLongPressed) {
      var oldState = currentState;
      if (oldState is Loaded<Location>) {
        var selectedItems = ((oldState.selectedItems ?? List<Location>()));

        if (selectedItems.any((l) => equal(l, event.location))) {
          selectedItems.remove(event.location);
        } else {
          selectedItems.add(event.location);
        }

        yield Loaded<Location>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: selectedItems.length > 0,
          selectedItems: selectedItems,
        );
      }
    }

    if (event is ClearSelected) {
      var oldState = currentState;
      if (oldState is Loaded<Location>) {
        yield Loaded<Location>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: false,
          selectedItems: null,
        );
      }
    }

    if (event is SelectAll) {
      var oldState = currentState;
      if (oldState is Loaded<Location>) {
        yield Loaded<Location>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: true,
          selectedItems: oldState.items,
        );
      }
    }

    if (event is RemoveUserFromSelected) {
      var oldState = currentState;
      if (oldState is Loaded<Location>) {
        yield Loading();

        await _locationRepository.removeUserFromLocations(
            event.userId, oldState.selectedItems.map((l) => l.id).toList());

        dispatch(_refreshEvent());
      }
    }

    if (event is AddUserToSelected) {
      var oldState = currentState;
      if (oldState is Loaded<Location>) {
        yield Loading();

        await _locationRepository.addUserToLocations(
            event.userId, oldState.selectedItems.map((l) => l.id).toList());

        dispatch(_refreshEvent());
      }
    }
  }

  @override
  List<Location> filtered(String text, List<String> filters) {
    var _filter = text.toLowerCase();
    return loaded
        .where((u) => u.displayName.toLowerCase().contains(_filter))
        .toList();
  }

  @override
  List<String> allFilters() => [];

  @override
  List<String> initialFilters() => [];

  final LocationListEvent Function() _refreshEvent;

  @override
  void refresh() => dispatch(_refreshEvent());

  @override
  void searchTextUpdated(String text) =>
      dispatch(SearchTextUpdated(searchText: text));

  @override
  void clear() {
    dispatch(ClearSelected());
  }

  @override
  void selectAll() {
    dispatch(SelectAll());
  }

  @override
  void toggleSelectable() {
    dispatch(ToggleSelectable());
  }

  @override
  bool equal(m1, m2) => m1.id == m2.id;

  @override
  bool isSelected(Location m) {
    var state = currentState;
    if (state is Loaded<Location>) {
      return state.selectable && state.selectedItems.any((si) => equal(si, m));
    }
    return false;
  }
}
