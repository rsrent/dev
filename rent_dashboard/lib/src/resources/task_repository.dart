import 'dart:async';
import 'task_api.dart';
import '../models/task.dart';

class TaskRepository extends TaskSource {
  List<TaskSource> sources = <TaskSource>[
    TaskApi(),
  ];

  Future<List<Task>> fetchTasks(int locationId, int customerId, int userId) async {
    var tasks;
    for (var source in sources) {
      tasks = await source.fetchTasks(locationId, customerId, userId);
      if (tasks != null) {
        break;
      }
    }
    return tasks;
  }
}

abstract class TaskSource {
  Future<List<Task>> fetchTasks(int locationId, int customerId, int userId);
}