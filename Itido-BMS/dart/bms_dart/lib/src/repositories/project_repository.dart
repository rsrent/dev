import 'dart:async';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';

import '../models/project.dart';
import '../models/project_item.dart';
import 'source.dart';

abstract class ProjectSource extends Source {
  Future<QueryResult<int>> create(String name, int parentId);
  Future<QueryResult<bool>> update(int projectId, Project project);
  Future<QueryResult<bool>> addProjectsToUser(int userId, List<int> projectIds);
  Future<QueryResult<bool>> removeProjectsFromUser(
      int userId, List<int> projectIds);
  Future<QueryResult<bool>> addAddress(int projectId);
  Future<QueryResult<bool>> addComment(int projectId);
  Future<QueryResult<bool>> addProfileImage(int projectId);
  Future<QueryResult<bool>> addLogs(int projectId);
  Future<QueryResult<bool>> addWork(int projectId);
  Future<QueryResult<bool>> addTasks(int projectId);
  Future<QueryResult<bool>> addQualityReports(int projectId);
  Future<QueryResult<bool>> addFolder(int projectId, String title);
  Future<QueryResult<bool>> addPost(int projectId, String title, String body);
  Future<QueryResult<Project>> fetch(int projectId);
  Future<QueryResult<List<Project>>> fetchOfProject(int projectId);
  Future<QueryResult<List<ProjectItem>>> fetchProjectItemsOfProject(
      int projectId);
  Future<QueryResult<List<ProjectItem>>> fetchDetailedProjectItemsOfProject(
      int projectId);
  Future<QueryResult<List<Project>>> fetchOfUser(int userId);
  Future<QueryResult<List<Project>>> fetchOfNotUser(int userId);
  Future<QueryResult<bool>> updateProjectItemAccess(
      int projectItemId, List<ProjectItemAccess> access);
}

class ProjectRepository extends ProjectSource {
  final List<ProjectSource> sources;

  ProjectRepository(this.sources);

  Future<QueryResult<int>> create(String name, int parentId) =>
      sources[0].create(name, parentId);
  Future<QueryResult<bool>> update(int projectId, Project project) =>
      sources[0].update(projectId, project);

  Future<QueryResult<bool>> addProjectsToUser(
          int userId, List<int> projectIds) =>
      sources[0].addProjectsToUser(userId, projectIds);
  Future<QueryResult<bool>> removeProjectsFromUser(
          int userId, List<int> projectIds) =>
      sources[0].removeProjectsFromUser(userId, projectIds);

  Future<QueryResult<bool>> addComment(int projectId) =>
      sources[0].addComment(projectId);
  Future<QueryResult<bool>> addProfileImage(int projectId) =>
      sources[0].addProfileImage(projectId);
  Future<QueryResult<bool>> addAddress(int projectId) =>
      sources[0].addAddress(projectId);
  Future<QueryResult<bool>> addLogs(int projectId) =>
      sources[0].addLogs(projectId);
  Future<QueryResult<bool>> addWork(int projectId) =>
      sources[0].addWork(projectId);
  Future<QueryResult<bool>> addTasks(int projectId) =>
      sources[0].addTasks(projectId);
  Future<QueryResult<bool>> addFolder(int projectId, String title) =>
      sources[0].addFolder(projectId, title);
  Future<QueryResult<bool>> addPost(int projectId, String title, String body) =>
      sources[0].addPost(projectId, title, body);
  Future<QueryResult<bool>> addQualityReports(int projectId) =>
      sources[0].addQualityReports(projectId);

  Future<QueryResult<Project>> fetch(int projectId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(projectId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<QueryResult<List<Project>>> fetchOfProject(int projectId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfProject(projectId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<QueryResult<List<Project>>> fetchOfUser(int userId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfUser(userId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<QueryResult<List<Project>>> fetchOfNotUser(int userId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfNotUser(userId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<QueryResult<List<ProjectItem>>> fetchProjectItemsOfProject(
      int projectId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchProjectItemsOfProject(projectId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<QueryResult<List<ProjectItem>>> fetchDetailedProjectItemsOfProject(
      int projectId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchDetailedProjectItemsOfProject(projectId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<QueryResult<bool>> updateProjectItemAccess(
          int projectItemId, List<ProjectItemAccess> access) =>
      sources[0].updateProjectItemAccess(projectItemId, access);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
