import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

import '../../models/absence.dart';

@immutable
abstract class AbsenceCreateUpdateEvent extends Equatable {
  AbsenceCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends AbsenceCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends AbsenceCreateUpdateEvent {
  final Absence absence;
  PrepareUpdate({@required this.absence}) : super([absence]);
  @override
  String toString() => 'PrepareUpdate { absence: $absence }';
}

class CreateRequested extends AbsenceCreateUpdateEvent {
  final bool asRequest;
  CreateRequested({@required this.asRequest}) : super([asRequest]);
  @override
  String toString() => 'CreateRequested { asRequest: $asRequest }';
}

class UpdateRequested extends AbsenceCreateUpdateEvent {
  @override
  String toString() => 'UpdateRequested';
}
