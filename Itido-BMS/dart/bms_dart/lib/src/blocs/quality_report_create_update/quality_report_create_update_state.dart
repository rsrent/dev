import 'package:bms_dart/src/blocs/quality_report_create_update/quality_report_create_update_errors.dart';
import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/quality_report.dart';
import '../../models/login.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class QualityReportCreateUpdateState extends Equatable {
  final QualityReport qualityReport;
  final QualityReportCreateUpdateErrors errors;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  QualityReportCreateUpdateState({
    @required this.errors,
    @required this.qualityReport,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          qualityReport.toMap(),
          isValid,
          createUpdateStatePhase,
        ]);

  factory QualityReportCreateUpdateState.createOrCopy(
    dynamic old, {
    QualityReport qualityReport,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(QualityReport) qualityReportChanges,
  }) {
    QualityReportCreateUpdateState previous;
    if (old is QualityReportCreateUpdateState) previous = old;

    var _qualityReport =
        qualityReport ?? previous?.qualityReport ?? QualityReport();
    var _isCreate = isCreate ?? previous?.isCreate ?? false;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = (createUpdateStatePhase ??
            (qualityReportChanges != null
                ? CreateUpdateStatePhase.InProgress
                : null)) ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;
    var _errors = previous?.errors ?? QualityReportCreateUpdateErrors();

    if (qualityReportChanges != null) qualityReportChanges(_qualityReport);

    _errors.qualityReportUpdated(_qualityReport);

    _isValid = _errors.isValid(_isCreate);

    return QualityReportCreateUpdateState(
      errors: _errors,
      qualityReport: _qualityReport,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'QualityReportCreateUpdateState { qualityReport: ${qualityReport.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
