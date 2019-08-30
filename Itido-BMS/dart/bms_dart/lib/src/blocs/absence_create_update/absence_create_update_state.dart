import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/models/accident_report.dart';
import 'package:bms_dart/src/models/agreement.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/absence.dart';
import '../../models/login.dart';
import '../create_update_state_phase.dart';
import 'absence_create_update_errors.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class AbsenceCreateUpdateState extends Equatable {
  final User requester;
  final int userId;
  final Absence absence;
  final AbsenceReason selectedAbsenceReason;
  final List<AbsenceReason> allAbsenceReasons;
  final AbsenceCreateUpdateErrors errors;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  AbsenceCreateUpdateState({
    @required this.requester,
    @required this.userId,
    @required this.errors,
    @required this.absence,
    @required this.selectedAbsenceReason,
    @required this.allAbsenceReasons,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          absence.toMap(),
          selectedAbsenceReason?.toMap(),
          allAbsenceReasons?.length,
          isValid,
          createUpdateStatePhase,
        ]);

  factory AbsenceCreateUpdateState.createOrCopy(
    dynamic old, {
    User requester,
    int userId,
    Absence absence,
    AbsenceReason selectedAbsenceReason,
    List<AbsenceReason> allAbsenceReasons,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Absence) absenceChanges,
  }) {
    AbsenceCreateUpdateState previous;
    if (old is AbsenceCreateUpdateState) previous = old;

    var _requester = requester ?? previous?.requester ?? User();
    var _userId = userId ?? previous?.userId ?? null;
    var _absence = absence ?? previous?.absence ?? Absence();
    var _selectedAbsenceReason =
        selectedAbsenceReason ?? previous?.selectedAbsenceReason;
    var _allAbsenceReasons =
        allAbsenceReasons ?? previous?.allAbsenceReasons ?? [];
    var _isCreate = isCreate ?? previous?.isCreate ?? false;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = (createUpdateStatePhase ??
            (absenceChanges != null
                ? CreateUpdateStatePhase.InProgress
                : null)) ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;
    var _errors = previous?.errors ?? AbsenceCreateUpdateErrors();

    print('_errors: $_errors');

    if (absenceChanges != null) absenceChanges(_absence);

    _errors.absenceUpdated(
        _requester, _userId, _absence, _selectedAbsenceReason);

    _isValid = _errors.isValid(_isCreate);

    return AbsenceCreateUpdateState(
      userId: _userId,
      requester: _requester,
      errors: _errors,
      absence: _absence,
      selectedAbsenceReason: _selectedAbsenceReason,
      allAbsenceReasons: _allAbsenceReasons,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'AbsenceCreateUpdateState { absence: ${absence.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
