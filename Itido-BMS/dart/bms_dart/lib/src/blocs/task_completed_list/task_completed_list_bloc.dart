import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/task_completed.dart';
import 'package:bms_dart/repositories.dart';

class TaskCompletedListBloc
    extends Bloc<TaskCompletedListEvent, ListState<TaskCompleted>>
    with Refreshable {
  final TaskCompletedRepository _taskCompletedRepository =
      repositoryProvider.taskCompletedRepository();

  final TaskCompletedListEvent Function() _refreshEven;

  TaskCompletedListBloc(this._refreshEven) {
    refresh();
  }

  @override
  ListState<TaskCompleted> get initialState => Loading<TaskCompleted>();

  @override
  Stream<ListState<TaskCompleted>> mapEventToState(
    TaskCompletedListEvent event,
  ) async* {
    if (event is TaskCompletedListFetchOfTask) {
      _taskCompletedRepository.fetchOfTask(event.taskId).then(
          (taskCompleteds) => dispatch(
              TaskCompletedListFetched(taskCompleteds: taskCompleteds)));
    }

    if (event is TaskCompletedListFetched) {
      final items = event.taskCompleteds;
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
