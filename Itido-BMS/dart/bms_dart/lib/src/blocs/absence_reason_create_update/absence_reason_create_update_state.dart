import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AbsenceReasonCreateUpdateState extends Equatable {
  AbsenceReasonCreateUpdateState([List props = const []]) : super(props);
}

class Initial extends AbsenceReasonCreateUpdateState {
  @override
  String toString() => 'Initial';
}

class PreparingCreate extends AbsenceReasonCreateUpdateState {
  @override
  String toString() => 'PreparingCreate';
}

class PreparingUpdate extends AbsenceReasonCreateUpdateState {
  final AbsenceReason absenceReason;
  PreparingUpdate({@required this.absenceReason}) : super([absenceReason]);
  @override
  String toString() => 'PreparingUpdate { absenceReason: $absenceReason }';
}

class Loading extends AbsenceReasonCreateUpdateState {
  @override
  String toString() => 'Loading';
}

class CreateFailure extends AbsenceReasonCreateUpdateState {
  @override
  String toString() => 'CreateFailure';
}

class UpdateFailure extends AbsenceReasonCreateUpdateState {
  @override
  String toString() => 'UpdateFailure';
}

class CreateSuccessful extends AbsenceReasonCreateUpdateState {
  final int id;

  CreateSuccessful({@required this.id}) : super([id]);

  @override
  String toString() => 'CreateSuccessful { id: $id }';
}

class UpdateSuccessful extends AbsenceReasonCreateUpdateState {
  @override
  String toString() => 'UpdateSuccessful';
}
