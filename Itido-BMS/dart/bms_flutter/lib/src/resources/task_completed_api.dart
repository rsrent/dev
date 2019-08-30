import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class TaskCompletedApi extends TaskCompletedSource {
  ClientController<TaskCompleted> _client;
  String path = '${api.path}/api/CompletedTask';

  TaskCompletedApi({
    http.Client client,
  }) {
    _client = ClientController<TaskCompleted>(
        converter: (json) => TaskCompleted.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<int> createFromComment(int taskId, String comment) {
    return _client.postId(
      '$path/Create/$taskId/$comment',
    );
  }

  @override
  Future<int> create(int taskId, TaskCompleted task) {
    return _client.postId(
      '$path/Create/$taskId',
      body: task.toMap(),
    );
  }

  @override
  Future<bool> update(int taskCompletedId, TaskCompleted taskCompleted) {
    return _client.put(
      '$path/Update/$taskCompletedId',
      body: taskCompleted.toMap(),
    );
  }

  @override
  Future<List<TaskCompleted>> fetchOfTask(int taskId) {
    return _client.getMany('$path/GetOfTask/$taskId');
  }

  @override
  Future<TaskCompleted> fetch(int taskCompletedId) {
    return _client.get('$path/Get/$taskCompletedId');
  }
}
