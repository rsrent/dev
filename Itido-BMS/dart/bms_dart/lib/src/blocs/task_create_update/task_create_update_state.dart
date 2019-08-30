import 'package:bms_dart/src/blocs/task_create_update/task_create_update_errors.dart';
import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/task.dart';
import '../../models/login.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class TaskCreateUpdateState extends Equatable {
  final Task task;
  final TaskCreateUpdateErrors errors;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  TaskCreateUpdateState({
    @required this.errors,
    @required this.task,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          task.toMap(),
          isValid,
          createUpdateStatePhase,
        ]);

  factory TaskCreateUpdateState.createOrCopy(
    dynamic old, {
    Task task,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Task) taskChanges,
  }) {
    TaskCreateUpdateState previous;
    if (old is TaskCreateUpdateState) previous = old;

    var _task = task ?? previous?.task ?? Task();
    var _isCreate = isCreate ?? previous?.isCreate ?? false;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = (createUpdateStatePhase ??
            (taskChanges != null ? CreateUpdateStatePhase.InProgress : null)) ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;
    var _errors = previous?.errors ?? TaskCreateUpdateErrors();

    if (taskChanges != null) taskChanges(_task);

    _errors.taskUpdated(_task);

    _isValid = _errors.isValid(_isCreate);

    return TaskCreateUpdateState(
      errors: _errors,
      task: _task,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'TaskCreateUpdateState { task: ${task.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
