import 'dart:async';
import 'package:bloc/bloc.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class TaskCreateUpdateBloc
    extends Bloc<TaskCreateUpdateEvent, TaskCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final TaskRepository _taskRepository = repositoryProvider.taskRepository();

  final int projectId;

  TaskCreateUpdateBloc({this.projectId});

  @override
  TaskCreateUpdateState get initialState =>
      TaskCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<TaskCreateUpdateState> mapEventToState(
    TaskCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      yield TaskCreateUpdateState.createOrCopy(
        null,
        task: Task(active: true),
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: true,
      );
      yield TaskCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }
    if (event is PrepareUpdate) {
      var task = await _taskRepository.fetch(event.task.id);
      yield TaskCreateUpdateState.createOrCopy(
        null,
        task: task,
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: false,
      );
      yield TaskCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is DescriptionChanged)
      yield TaskCreateUpdateState.createOrCopy(currentState,
          taskChanges: (task) => task.description = event.text);
    if (event is PlaceChanged)
      yield TaskCreateUpdateState.createOrCopy(currentState,
          taskChanges: (task) => task.place = event.text);
    if (event is CommentChanged)
      yield TaskCreateUpdateState.createOrCopy(currentState,
          taskChanges: (task) => task.comment = event.text);
    if (event is SquareMetersChanged)
      yield TaskCreateUpdateState.createOrCopy(currentState,
          taskChanges: (task) => task.squareMeters =
              event.text != '' ? int.parse(event.text) : null);
    if (event is FrequencyChanged)
      yield TaskCreateUpdateState.createOrCopy(currentState,
          taskChanges: (task) {
        task.frequency = event.text;
        task.timesOfYear = null;
      });
    if (event is TimesOfYearChanged)
      yield TaskCreateUpdateState.createOrCopy(currentState,
          taskChanges: (task) {
        task.frequency = null;
        task.squareMeters = event.text != '' ? int.parse(event.text) : null;
      });

    if (event is Commit) {
      var newState = TaskCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result;
      if (newState.isCreate) {
        result =
            (await _taskRepository.create(projectId, newState.task)) != null;
      } else {
        result = await _taskRepository.update(newState.task.id, newState.task);
      }
      if (result) {
        yield TaskCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield TaskCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
