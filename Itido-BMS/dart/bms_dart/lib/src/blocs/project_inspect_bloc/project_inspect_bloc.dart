import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/sprog.dart';
import 'package:bms_dart/src/models/query_result.dart';
import 'package:flutter/foundation.dart';
import '../dispatch_query_result.dart';
import './bloc.dart';

class ProjectInspectBloc extends Bloc<ProjectInspectEvent, ProjectInspectState>
    with DispatchQueryResult {
  final ProjectRepository _projectRepository =
      repositoryProvider.projectRepository();

  final projectId;

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  ProjectInspectBloc(
    this.projectId, {
    @required this.queryResultBloc,
    @required this.sprog,
  });

  @override
  ProjectInspectState get initialState => InitialProjectInspectState();

  @override
  Stream<ProjectInspectState> mapEventToState(
    ProjectInspectEvent event,
  ) async* {
    if (event is ProjectInspectEventFetch) {
      yield LoadingProjectInspectState();

      var futureProject = _projectRepository.fetch(projectId);
      var futureProjectItems =
          _projectRepository.fetchProjectItemsOfProject(projectId);

      var result = await Future.wait([futureProject, futureProjectItems]);

      dispatch(ProjectInspectEventLoaded(
          project: result[0].value, projectItems: result[1].value));
    }

    if (event is ProjectInspectEventLoaded) {
      if (event.project != null && event.projectItems != null) {
        yield LoadedProjectInspectState(
          project: event.project,
          projectItems: event.projectItems,
        );
      } else {
        yield ErrorProjectInspectState();
      }
    }

    if (event is ProjectInspectEventAddProject) {
      yield copyOrCreate(loading: true);
      var id = await _projectRepository.create(event.name, event.projectId);
      if (id != null) {
        dispatch(ProjectInspectEventFetch());
      } else {
        yield copyOrCreate(loading: false);
      }
    }
    if (event is ProjectInspectEventAddAddress) {
      yield* tryAdd(_projectRepository.addAddress(event.projectId));
    }
    if (event is ProjectInspectEventAddLogs) {
      yield* tryAdd(_projectRepository.addLogs(event.projectId));
    }
    if (event is ProjectInspectEventAddWork) {
      yield* tryAdd(_projectRepository.addWork(event.projectId));
    }
    if (event is ProjectInspectEventAddTasks) {
      yield* tryAdd(_projectRepository.addTasks(event.projectId));
    }
    if (event is ProjectInspectEventAddQualityReports) {
      yield* tryAdd(_projectRepository.addQualityReports(event.projectId));
    }
    if (event is ProjectInspectEventAddComment) {
      yield* tryAdd(_projectRepository.addComment(event.projectId));
    }
    if (event is ProjectInspectEventAddProfileImage) {
      yield* tryAdd(_projectRepository.addProfileImage(event.projectId));
    }
    if (event is ProjectInspectEventAddFolder) {
      yield* tryAdd(_projectRepository.addFolder(event.projectId, event.title));
    }
    if (event is ProjectInspectEventAddPost) {
      yield* tryAdd(
          _projectRepository.addPost(event.projectId, event.title, event.body));
    }

    /*
    if (event is ProjectInspectEventAddProject) {
      yield* tryAdd(_projectRepository.create(event.name, event.projectId));
      yield copyOrCreate(loading: true);
      var id = await _projectRepository.create(event.name, event.projectId);
      if (id != null) {
        dispatch(ProjectInspectEventFetch());
      } else {
        yield copyOrCreate(loading: false);
      }
    }

    if (event is ProjectInspectEventAddLogs) {
      yield copyOrCreate(loading: true);
      var id = await _projectRepository.addLogs(event.projectId);
      if (id != null) {
        dispatch(ProjectInspectEventFetch());
      } else {
        yield copyOrCreate(loading: false);
      }
    }

    if (event is ProjectInspectEventAddTasks) {
      yield copyOrCreate(loading: true);
      var id = await _projectRepository.addTasks(event.projectId);
      if (id != null) {
        dispatch(ProjectInspectEventFetch());
      } else {
        yield copyOrCreate(loading: false);
      }
    }

    if (event is ProjectInspectEventAddWork) {
      yield copyOrCreate(loading: true);
      var id = await _projectRepository.addWork(event.projectId);
      if (id != null) {
        dispatch(ProjectInspectEventFetch());
      } else {
        yield copyOrCreate(loading: false);
      }
    }

    if (event is ProjectInspectEventAddQualityReports) {
      yield copyOrCreate(loading: true);
      var id = await _projectRepository.addQualityReports(event.projectId);
      if (id != null) {
        dispatch(ProjectInspectEventFetch());
      } else {
        yield copyOrCreate(loading: false);
      }
    }
    */
  }

  Stream<ProjectInspectState> tryAdd(
    Future<QueryResult<bool>> operation,
  ) async* {
    yield copyOrCreate(loading: true);
    var result = await operation;
    dispatchQueryResult(result, sprog().createAttempted);
    if (result.successful) {
      dispatch(ProjectInspectEventFetch());
    } else {
      yield copyOrCreate(loading: false);
    }
  }

  LoadedProjectInspectState copyOrCreate({
    Project project,
    List<ProjectItem> items,
    bool loading,
  }) {
    var old = currentState;
    if (old is LoadedProjectInspectState) {
      return LoadedProjectInspectState(
        project: project ?? old.project,
        projectItems: items ?? old.projectItems,
        loading: loading ?? old.loading,
      );
    } else {
      return LoadedProjectInspectState(
        project: project ?? null,
        projectItems: items ?? [],
        loading: loading ?? false,
      );
    }
  }

  // Stream<ProjectInspectState> getOldOrNull(
  //   Stream<ProjectInspectState> Function(LoadedProjectInspectState) then, [
  //   Stream<ProjectInspectState> Function() orElse,
  // ]) async* {
  //   var oldState = currentState;
  //   if (oldState is LoadedProjectInspectState) {
  //     yield* then(oldState);
  //   } else if (orElse != null) {
  //     yield* orElse();
  //   }
  // }
}
