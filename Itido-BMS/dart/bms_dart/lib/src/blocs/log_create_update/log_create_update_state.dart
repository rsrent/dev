import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/log.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class LogCreateUpdateState extends Equatable {
  final Log log;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  LogCreateUpdateState({
    @required this.log,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          log.toMap(),
          isValid,
          createUpdateStatePhase,
        ]);

  factory LogCreateUpdateState.createOrCopy(
    dynamic old, {
    Log log,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Log) changes,
  }) {
    LogCreateUpdateState previous;
    if (old is LogCreateUpdateState) previous = old;

    var _log = log ?? previous?.log ?? Log();
    var _isValid = isValid ?? previous?.isValid ?? true;
    var _createUpdateStatePhase = createUpdateStatePhase ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;

    if (changes != null) changes(_log);

    return LogCreateUpdateState(
      log: _log,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'LogCreateUpdateState { log: ${log.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
