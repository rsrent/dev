import 'package:bms_dart/src/blocs/task_completed_create_update/task_completed_create_update_errors.dart';
import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/task_completed.dart';
import '../../models/login.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class TaskCompletedCreateUpdateState extends Equatable {
  final TaskCompleted taskCompleted;
  final TaskCompletedCreateUpdateErrors errors;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  TaskCompletedCreateUpdateState({
    @required this.errors,
    @required this.taskCompleted,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          taskCompleted.toMap(),
          isValid,
          createUpdateStatePhase,
        ]);

  factory TaskCompletedCreateUpdateState.createOrCopy(
    dynamic old, {
    TaskCompleted taskCompleted,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(TaskCompleted) taskCompletedChanges,
  }) {
    TaskCompletedCreateUpdateState previous;
    if (old is TaskCompletedCreateUpdateState) previous = old;

    var _taskCompleted =
        taskCompleted ?? previous?.taskCompleted ?? TaskCompleted();
    var _isCreate = isCreate ?? previous?.isCreate ?? false;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = (createUpdateStatePhase ??
            (taskCompletedChanges != null
                ? CreateUpdateStatePhase.InProgress
                : null)) ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;
    var _errors = previous?.errors ?? TaskCompletedCreateUpdateErrors();

    if (taskCompletedChanges != null) taskCompletedChanges(_taskCompleted);

    _errors.taskCompletedUpdated(_taskCompleted);

    _isValid = _errors.isValid(_isCreate);

    return TaskCompletedCreateUpdateState(
      errors: _errors,
      taskCompleted: _taskCompleted,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'TaskCompletedCreateUpdateState { taskCompleted: ${taskCompleted.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
