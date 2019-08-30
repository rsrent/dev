import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/task.dart';
import 'package:bms_dart/repositories.dart';

class TaskListBloc extends Bloc<TaskListEvent, ListState<Task>>
    with Refreshable {
  final TaskRepository _taskRepository = repositoryProvider.taskRepository();

  final TaskListEvent Function() _refreshEven;

  TaskListBloc(this._refreshEven) {
    refresh();
  }

  @override
  ListState<Task> get initialState => Loading<Task>();

  @override
  Stream<ListState<Task>> mapEventToState(
    TaskListEvent event,
  ) async* {
    if (event is TaskListFetchOfProjectItem) {
      _taskRepository
          .fetchOfProjectItem(event.projectItemId)
          .then((tasks) => dispatch(TaskListFetched(tasks: tasks)));
    }

    if (event is TaskListFetched) {
      final items = event.tasks;
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }
  }

  @override
  void refresh() {
    dispatch(_refreshEven());
  }
}
