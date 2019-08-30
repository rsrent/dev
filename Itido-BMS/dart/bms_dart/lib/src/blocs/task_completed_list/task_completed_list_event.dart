import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/task_completed.dart';

@immutable
abstract class TaskCompletedListEvent extends Equatable {
  TaskCompletedListEvent([List props = const []]) : super(props);
}

class TaskCompletedListFetchOfTask extends TaskCompletedListEvent {
  final int taskId;
  TaskCompletedListFetchOfTask({@required this.taskId}) : super([taskId]);
  @override
  String toString() => 'TaskCompletedListFetchOfTask';
}

class TaskCompletedListFetched extends TaskCompletedListEvent {
  final List<TaskCompleted> taskCompleteds;
  TaskCompletedListFetched({@required this.taskCompleteds})
      : super([taskCompleteds]);
  @override
  String toString() =>
      'TaskCompletedListFetched { taskCompleteds: ${taskCompleteds.length} }';
}
