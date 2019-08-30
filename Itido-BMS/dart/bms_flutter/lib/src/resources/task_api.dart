import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class TaskApi extends TaskSource {
  //Client _client;

  ClientController<Task> _client;
  String path = '${api.path}/api/CleaningTask';

  TaskApi({
    http.Client client,
  }) {
    _client = ClientController<Task>(
        converter: (json) => Task.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<Task>> fetchOfProjectItem(int projectItemId) {
    return _client.getMany('$path/GetOfProjectItem/$projectItemId');
  }

  @override
  Future<int> create(int projectItemId, Task task) {
    return _client.postId(
      '$path/CreateForProjectItem/$projectItemId',
      body: task.toMap(),
    );
  }

  @override
  Future<bool> update(int taskId, Task task) {
    return _client.put(
      '$path/Update/$taskId',
      body: task.toMap(),
    );
  }

  @override
  Future<Task> fetch(int taskId) {
    return _client.get('$path/Get/$taskId');
  }
}
