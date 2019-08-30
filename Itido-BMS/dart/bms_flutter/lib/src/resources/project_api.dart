import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'query_client_controller.dart';

class ProjectApi extends ProjectSource {
  //Client _client;
  QueryClientController<Project> _client;
  //ClientController<Project> _client;
  QueryClientController<ProjectItem> _projectItemClient;
  String path = '${api.path}/api/Project';

  ProjectApi({
    http.Client client,
  }) {
    _client = QueryClientController(
      converter: (json) => Project.fromJson(json),
      client: client,
      getHeaders: () => api.headers(),
    );
    // _client = ClientController<Project>(
    //     converter: (json) => Project.fromJson(json),
    //     client: client,
    //     getHeaders: () => api.headers());
    _projectItemClient = QueryClientController<ProjectItem>(
        converter: (json) => ProjectItem.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<QueryResult<int>> create(String name, int parentId) {
    return _client.postId(
      '$path/Create/$name' + (parentId != null ? '/$parentId' : ''),
    );
  }

  @override
  Future<QueryResult<bool>> update(int projectId, Project project) {
    return _client.put(
      '$path/Update/$projectId',
      body: project.toMap(),
    );
  }

  @override
  Future<QueryResult<bool>> addProjectsToUser(
      int userId, List<int> projectIds) {
    return _client.put(
      '$path/AddProjectsToUsers/$userId',
      body: projectIds,
    );
  }

  @override
  Future<QueryResult<bool>> removeProjectsFromUser(
      int userId, List<int> projectIds) {
    return _client.put(
      '$path/RemoveProjectsFromUser/$userId',
      body: projectIds,
    );
  }

  @override
  Future<QueryResult<bool>> addAddress(int projectId) {
    return _client.put('$path/AddAddress/$projectId');
  }

  @override
  Future<QueryResult<bool>> addComment(int projectId) {
    return _client.put('$path/AddComment/$projectId');
  }

  @override
  Future<QueryResult<bool>> addProfileImage(int projectId) {
    return _client.put('$path/AddProfileImage/$projectId');
  }

  @override
  Future<QueryResult<bool>> addLogs(int projectId) {
    return _client.put('$path/AddLogs/$projectId');
  }

  @override
  Future<QueryResult<bool>> addWork(int projectId) {
    return _client.put('$path/AddWork/$projectId');
  }

  @override
  Future<QueryResult<bool>> addTasks(int projectId) {
    return _client.put('$path/AddTasks/$projectId');
  }

  @override
  Future<QueryResult<bool>> addQualityReports(int projectId) {
    return _client.put('$path/AddQualityReports/$projectId');
  }

  @override
  Future<QueryResult<bool>> addFolder(int projectId, String title) {
    return _client.put('$path/AddFolder/$projectId/$title');
  }

  @override
  Future<QueryResult<bool>> addPost(int projectId, String title, String body) {
    return _client.put('$path/AddPost/$projectId/$title/$body');
  }

  @override
  Future<QueryResult<Project>> fetch(int projectId) {
    return _client.get('$path/Get/$projectId');
  }

  @override
  Future<QueryResult<List<Project>>> fetchOfProject(int projectId) {
    return _client.getMany('$path/GetProjectsOfProject/$projectId');
  }

  @override
  Future<QueryResult<List<ProjectItem>>> fetchProjectItemsOfProject(
      int projectId) {
    return _projectItemClient
        .getMany('$path/GetProjectItemsOfProject/$projectId');
  }

  @override
  Future<QueryResult<List<ProjectItem>>> fetchDetailedProjectItemsOfProject(
      int projectId) {
    return _projectItemClient
        .getMany('$path/GetDetailedProjectItemsOfProject/$projectId');
  }

  @override
  Future<QueryResult<List<Project>>> fetchOfUser(int userId) {
    return _client.getMany('$path/GetOfUser/$userId');
  }

  @override
  Future<QueryResult<List<Project>>> fetchOfNotUser(int userId) {
    return _client.getMany('$path/GetOfNotUser/$userId');
  }

  @override
  Future<QueryResult<bool>> updateProjectItemAccess(
      int projectItemId, List<ProjectItemAccess> access) {
    return _client.put(
      '$path/UpdateProjectItemAccess/$projectItemId',
      body: access.map((a) => a.toMap()).toList(),
    );
  }
}
