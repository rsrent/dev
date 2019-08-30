import 'dart:async';
import '../models/task.dart';
import 'source.dart';

abstract class TaskSource extends Source {
  Future<Task> fetch(int taskId);
  Future<List<Task>> fetchOfProjectItem(int projectItemId);
  Future<int> create(int projectId, Task task);
  Future<bool> update(int taskId, Task task);
}

class TaskRepository extends TaskSource {
  final List<TaskSource> sources;
  TaskRepository(this.sources);

  Future<Task> fetch(int taskId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(taskId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<Task>> fetchOfProjectItem(int projectItemId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfProjectItem(projectItemId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<int> create(int projectId, Task task) =>
      sources[0].create(projectId, task);
  Future<bool> update(int taskId, Task task) => sources[0].update(taskId, task);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
