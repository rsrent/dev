import 'dart:async';
import 'package:bloc/bloc.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class TaskCompletedCreateUpdateBloc extends Bloc<TaskCompletedCreateUpdateEvent,
    TaskCompletedCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final TaskCompletedRepository _taskCompletedRepository =
      repositoryProvider.taskCompletedRepository();

  final int taskId;

  TaskCompletedCreateUpdateBloc({this.taskId});

  @override
  TaskCompletedCreateUpdateState get initialState =>
      TaskCompletedCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<TaskCompletedCreateUpdateState> mapEventToState(
    TaskCompletedCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      yield TaskCompletedCreateUpdateState.createOrCopy(
        null,
        taskCompleted: TaskCompleted(
          confirmed: true,
          completedDate: DateTime.now(),
        ),
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: true,
      );
      yield TaskCompletedCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }
    if (event is PrepareUpdate) {
      var taskCompleted =
          await _taskCompletedRepository.fetch(event.taskCompleted.id);
      yield TaskCompletedCreateUpdateState.createOrCopy(
        null,
        taskCompleted: taskCompleted,
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: false,
      );
      yield TaskCompletedCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is CommentChanged)
      yield TaskCompletedCreateUpdateState.createOrCopy(currentState,
          taskCompletedChanges: (taskCompleted) =>
              taskCompleted.comment = event.text);
    if (event is CompletedDateChanged)
      yield TaskCompletedCreateUpdateState.createOrCopy(currentState,
          taskCompletedChanges: (taskCompleted) =>
              taskCompleted.completedDate = event.dateTime);

    if (event is Commit) {
      var newState = TaskCompletedCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result;
      if (newState.isCreate) {
        result = (await _taskCompletedRepository.create(
                taskId, newState.taskCompleted)) !=
            null;
      } else {
        result = await _taskCompletedRepository.update(
            newState.taskCompleted.id, newState.taskCompleted);
      }
      if (result) {
        yield TaskCompletedCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield TaskCompletedCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
