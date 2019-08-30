import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/query_result.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/src/blocs/dispatch_query_result.dart';
import '../refreshable.dart';
import '../searchable.dart';
import '../selectable.dart';
import './bloc.dart';
import 'package:bms_dart/models.dart';

class UserListBloc extends Bloc<UserListEvent, ListState<User>>
    with Refreshable, Searchable<User>, Selectable<User>, DispatchQueryResult {
  final UserRepository _userRepository = repositoryProvider.userRepository();
  // final ProjectRepository _projectRepository =
  //     repositoryProvider.projectRepository();

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;

  UserListBloc(this._refreshEvent, {this.queryResultBloc}) {
    refresh();
    filters.addAll(initialFilters());
  }

  @override
  ListState<User> get initialState => Loading();

  @override
  Stream<ListState<User>> mapEventToState(
    UserListEvent event,
  ) async* {
    if (event is FetchAll) {
      await _userRepository.fetchAllUsers().then((result) {
        dispatch(UsersFetched(users: result.value));
      });
    }

    if (event is FetchOfProject) {
      await _userRepository.fetchOfProject(event.projectId).then((result) {
        dispatch(UsersFetched(users: result.value));
      });
    }
    if (event is FetchOfProjectAvailableOnDate) {
      await _userRepository
          .fetchOfProjectAvailableOnDate(event.projectId, event.date)
          .then((result) {
        dispatch(UsersFetched(users: result.value));
      });
    }
    if (event is FetchOfNotProject) {
      await _userRepository.fetchOfNotProject(event.projectId).then((result) {
        dispatch(UsersFetched(users: result.value));
      });
    }

    if (event is SearchTextUpdated) {
      searchText = event.searchText;
      dispatch(UsersFetched(users: loaded));
    }

    if (event is UsersFetched) {
      loaded = event.users;
      if (event.users != null) {
        print('1');
        var items = filtered(searchText, filters);
        print('2');
        var oldState = currentState;
        if (oldState is Loaded<User>) {
          print('3');
          yield Loaded<User>(
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

    if (event is UserLongPressed) {
      var oldState = currentState;
      if (oldState is Loaded<User>) {
        var selectedItems = ((oldState.selectedItems ?? List<User>()));

        if (selectedItems.contains(event.user)) {
          selectedItems.remove(event.user);
        } else {
          selectedItems.add(event.user);
        }

        yield Loaded<User>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: selectedItems.length > 0,
          selectedItems: selectedItems,
        );
      }
    }

    if (event is ToggleSelectable) {
      var oldState = currentState;
      if (oldState is Loaded<User>) {
        yield Loaded<User>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: !oldState.selectable,
          selectedItems: [],
        );
      } else {
        yield Loaded<User>(
          items: [],
          refreshTime: DateTime.now(),
          selectable: true,
          selectedItems: [],
        );
      }
    }

    if (event is ClearSelected) {
      var oldState = currentState;
      if (oldState is Loaded<User>) {
        yield Loaded<User>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: false,
          selectedItems: null,
        );
      }
    }

    if (event is SelectAll) {
      var oldState = currentState;
      if (oldState is Loaded<User>) {
        yield Loaded<User>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: true,
          selectedItems: oldState.items.map((i) => i).toList(),
        );
      }
    }

    if (event is AddSelectedToProject) {
      var oldState = currentState;
      if (oldState is Loaded<User>) {
        yield Loading();
        await _userRepository.addUsersToProject(
          event.projectId,
          oldState.selectedItems.map((l) => l.id).toList(),
        );
        dispatch(_refreshEvent());
      }
    }

    if (event is RemoveSelectedFromProject) {
      var oldState = currentState;
      if (oldState is Loaded<User>) {
        yield Loading();
        await _userRepository.removeUsersFromProject(
          event.projectId,
          oldState.selectedItems.map((l) => l.id).toList(),
        );
        dispatch(_refreshEvent());
      }
    }
  }

  @override
  List<String> allFilters() => const [
        'Active',
        'Disabled',
        'Admin',
        'Manager',
        'User',
        'ClientAdmin',
        'ClientManager',
      ];

  @override
  List<String> initialFilters() => const [
        'Active',
        'Admin',
        'Manager',
        'User',
        'ClientAdmin',
        'ClientManager',
      ];

  @override
  List<User> filtered(String text, List<String> filters) {
    var _filter = text.toLowerCase();
    Iterable<User> result = loaded
        .where((u) => u.displayName.toLowerCase().contains(_filter))
        .toList();

    print(filters);

    result = result.where((u) => filters.any((f) => u.userRole == f));
    if (filters.contains('Active')) result = result.where((u) => !u.disabled);
    if (filters.contains('Disabled')) result = result.where((u) => u.disabled);

    return result.toList();
  }

  final UserListEvent Function() _refreshEvent;
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
  bool isSelected(User m) {
    var state = currentState;
    if (state is Loaded<User>) {
      return state.selectable && state.selectedItems.any((si) => equal(si, m));
    }
    return false;
  }
}
