import 'dart:async';
import '../models/task_completed.dart';
import 'source.dart';

abstract class TaskCompletedSource extends Source {
  Future<TaskCompleted> fetch(int taskCompletedId);
  Future<List<TaskCompleted>> fetchOfTask(int taskId);
  Future<int> createFromComment(int taskId, String comment);
  Future<int> create(int taskId, TaskCompleted taskCompleted);
  Future<bool> update(int taskCompletedId, TaskCompleted taskCompleted);
}

class TaskCompletedRepository extends TaskCompletedSource {
  final List<TaskCompletedSource> sources;
  TaskCompletedRepository(this.sources);

  Future<TaskCompleted> fetch(int taskCompletedId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(taskCompletedId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<TaskCompleted>> fetchOfTask(int taskId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfTask(taskId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<int> createFromComment(int taskId, String comment) =>
      sources[0].createFromComment(taskId, comment);

  Future<int> create(int taskId, TaskCompleted taskCompleted) =>
      sources[0].create(taskId, taskCompleted);

  Future<bool> update(int taskCompletedId, TaskCompleted taskCompleted) =>
      sources[0].update(taskCompletedId, taskCompleted);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
