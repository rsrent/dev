import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import '../searchable.dart';
import '../selectable.dart';
import './bloc.dart';
import '../../models/project.dart';
import 'package:bms_dart/repositories.dart';

class ProjectListBloc extends Bloc<ProjectListEvent, ListState<Project>>
    with Refreshable, Searchable, Selectable<Project> {
  final ProjectRepository _projectRepository =
      repositoryProvider.projectRepository();

  final ProjectListEvent Function() _refreshEven;

  ProjectListBloc(this._refreshEven) {
    refresh();
  }

  @override
  ListState<Project> get initialState => Loading<Project>();

  @override
  Stream<ListState<Project>> mapEventToState(
    ProjectListEvent event,
  ) async* {
    if (event is ProjectListFetchOfProject) {
      _projectRepository.fetchOfProject(event.projectId).then((result) {
        dispatch(ProjectListFetched(projects: result.value));
      });
    }
    if (event is ProjectListFetchOfUser) {
      _projectRepository.fetchOfUser(event.userId).then((result) {
        dispatch(ProjectListFetched(projects: result.value));
      });
    }
    if (event is ProjectListFetchNotOfUser) {
      _projectRepository.fetchOfNotUser(event.userId).then((result) {
        dispatch(ProjectListFetched(projects: result.value));
      });
    }

    if (event is ProjectListFetched) {
      loaded = event.projects;
      if (event.projects != null) {
        var items = filtered(searchText, filters);
        var oldState = currentState;
        if (oldState is Loaded<Project>) {
          yield Loaded<Project>(
            items: items,
            refreshTime: DateTime.now(),
            selectable: oldState.selectable,
            selectedItems: oldState.selectedItems,
          );
        } else {
          yield Loaded(items: items, refreshTime: DateTime.now());
        }
      } else
        yield Failure();
    }

    if (event is ProjectListAddNew) {
      yield Loading();
      await _projectRepository.create(event.name, event.projectId).then(
          (projectId) =>
              dispatch(ProjectListFetchOfProject(projectId: event.projectId)));
    }

    if (event is ToggleSelectable) {
      var oldState = currentState;
      if (oldState is Loaded<Project>) {
        yield Loaded<Project>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: !oldState.selectable,
          selectedItems: [],
        );
      } else {
        yield Loaded<Project>(
          items: [],
          refreshTime: DateTime.now(),
          selectable: true,
          selectedItems: [],
        );
      }
    }

    if (event is ProjectLongPressed) {
      var oldState = currentState;
      if (oldState is Loaded<Project>) {
        var selectedItems = ((oldState.selectedItems ?? List<Project>()));

        if (selectedItems.any((l) => equal(l, event.project))) {
          selectedItems.remove(event.project);
        } else {
          selectedItems.add(event.project);
        }

        yield Loaded<Project>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: selectedItems.length > 0,
          selectedItems: selectedItems,
        );
      }
    }

    if (event is ClearSelected) {
      var oldState = currentState;
      if (oldState is Loaded<Project>) {
        yield Loaded<Project>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: false,
          selectedItems: null,
        );
      }
    }

    if (event is SelectAll) {
      var oldState = currentState;
      if (oldState is Loaded<Project>) {
        yield Loaded<Project>(
          items: oldState.items,
          refreshTime: DateTime.now(),
          selectable: true,
          selectedItems: oldState.items.map((i) => i).toList(),
        );
      }
    }

    if (event is SearchTextUpdated) {
      searchText = event.searchText;
      dispatch(ProjectListFetched(projects: loaded));
    }

    if (event is RemoveUserFromSelected) {
      var oldState = currentState;
      if (oldState is Loaded<Project>) {
        yield Loading();
        await _projectRepository.removeProjectsFromUser(
            event.userId, oldState.selectedItems.map((l) => l.id).toList());
        refresh();
      }
    }

    if (event is AddUserToSelected) {
      var oldState = currentState;
      if (oldState is Loaded<Project>) {
        yield Loading();
        await _projectRepository.addProjectsToUser(
            event.userId, oldState.selectedItems.map((l) => l.id).toList());
        refresh();
      }
    }
  }

  @override
  void refresh() {
    dispatch(_refreshEven());
  }

  @override
  List<Project> filtered(String text, List<String> filters) {
    var _filter = text.toLowerCase();
    return loaded.where((u) => u.name.toLowerCase().contains(_filter)).toList();
  }

  @override
  List<String> allFilters() => [];

  @override
  List<String> initialFilters() => [];

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
  bool isSelected(Project m) {
    var state = currentState;
    if (state is Loaded<Project>) {
      return state.selectable && state.selectedItems.any((si) => equal(si, m));
    }
    return false;
  }
}
