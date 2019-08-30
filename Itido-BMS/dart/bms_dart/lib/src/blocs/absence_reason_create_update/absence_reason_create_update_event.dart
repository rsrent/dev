import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AbsenceReasonCreateUpdateEvent extends Equatable {
  AbsenceReasonCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends AbsenceReasonCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends AbsenceReasonCreateUpdateEvent {
  final AbsenceReason absenceReason;
  PrepareUpdate({@required this.absenceReason}) : super([absenceReason]);
  @override
  String toString() => 'PrepareUpdate { absenceReason: $absenceReason }';
}

class CreateRequested extends AbsenceReasonCreateUpdateEvent {
  @override
  String toString() => 'CreateRequested';
}

class UpdateRequested extends AbsenceReasonCreateUpdateEvent {
  @override
  String toString() => 'UpdateRequested';
}
