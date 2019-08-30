import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/task.dart';

@immutable
abstract class TaskListEvent extends Equatable {
  TaskListEvent([List props = const []]) : super(props);
}

class TaskListFetchOfProjectItem extends TaskListEvent {
  final int projectItemId;
  TaskListFetchOfProjectItem({@required this.projectItemId})
      : super([projectItemId]);
  @override
  String toString() => 'TaskListFetchOfProjectItem';
}

class TaskListFetched extends TaskListEvent {
  final List<Task> tasks;
  TaskListFetched({@required this.tasks}) : super([tasks]);
  @override
  String toString() => 'TaskListFetched { tasks: ${tasks.length} }';
}
