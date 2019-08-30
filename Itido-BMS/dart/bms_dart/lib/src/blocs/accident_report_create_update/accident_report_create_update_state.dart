import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/accident_report.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class AccidentReportCreateUpdateState extends Equatable {
  final AccidentReport accidentReport;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  AccidentReportCreateUpdateState({
    @required this.accidentReport,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          accidentReport.toMap(),
          isCreate,
          isValid,
          createUpdateStatePhase,
        ]);

  factory AccidentReportCreateUpdateState.createOrCopy(
    dynamic old, {
    AccidentReport accidentReport,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(AccidentReport) changes,
  }) {
    AccidentReportCreateUpdateState previous;
    if (old is AccidentReportCreateUpdateState) previous = old;

    var _accidentReport =
        accidentReport ?? previous?.accidentReport ?? AccidentReport();
    var _isCreate = isCreate ?? previous?.isCreate ?? true;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = createUpdateStatePhase ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;

    if (changes != null) changes(_accidentReport);

    return AccidentReportCreateUpdateState(
      accidentReport: _accidentReport,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'AccidentReportCreateUpdateState { accidentReport: ${accidentReport.toMap()}, isCreate: $isCreate, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
