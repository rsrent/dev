import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/project_item.dart';
import 'package:bms_dart/repositories.dart';

class ProjectItemListBloc
    extends Bloc<ProjectItemListEvent, ListState<ProjectItem>>
    with Refreshable {
  final ProjectRepository _projectRepository =
      repositoryProvider.projectRepository();

  final ProjectItemListEvent Function() _refreshEven;

  ProjectItemListBloc(this._refreshEven) {
    refresh();
  }

  @override
  ListState<ProjectItem> get initialState => Loading<ProjectItem>();

  @override
  Stream<ListState<ProjectItem>> mapEventToState(
    ProjectItemListEvent event,
  ) async* {
    if (event is ProjectItemListFetchOfProject) {
      _projectRepository
          .fetchProjectItemsOfProject(event.projectId)
          .then((result) {
        dispatch(ProjectItemListFetched(projectItems: result.value));
      });
    }
    if (event is ProjectItemListFetchDetailedOfProject) {
      _projectRepository
          .fetchDetailedProjectItemsOfProject(event.projectId)
          .then((result) {
        dispatch(ProjectItemListFetched(projectItems: result.value));
      });
    }

    if (event is ProjectItemListFetched) {
      final items = event.projectItems;
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is ProjectItemListUpdateAccessOfItem) {
      await _projectRepository
          .updateProjectItemAccess(event.projectItemId, event.access)
          .then((logId) => refresh());
    }
  }

  @override
  void refresh() {
    dispatch(_refreshEven());
  }
}
