import 'package:http/http.dart' show Client;
import 'dart:convert';
import '../models/task.dart';
import 'dart:async';
import 'task_repository.dart';
import '../network.dart';

class TaskApi extends TaskSource {
  Client client = Client();

  Future<List<Task>> fetchTasks(int locationId, int customerId, int userId) async {
    final response = await client.get(
      '${Network.root}/Dashboard/Tasks/$locationId/$customerId/$userId',
      headers: Network.getHeaders(),
    );
    final tasks = json.decode(response.body);
    return List.from(tasks.map((j) {
      return Task.fromJson(j);
    }));
  }
}
