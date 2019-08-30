import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

import '../../models/absence.dart';

@immutable
abstract class AbsenceCreateUpdateState extends Equatable {
  AbsenceCreateUpdateState([List props = const []]) : super(props);
}

class Initial extends AbsenceCreateUpdateState {
  @override
  String toString() => 'Initial';
}

class PreparingUpdate extends AbsenceCreateUpdateState {
  final Absence absence;
  PreparingUpdate({@required this.absence}) : super([absence]);
  @override
  String toString() => 'PreparingUpdate { absence: $absence }';
}

class PreparingCreate extends AbsenceCreateUpdateState {
  @override
  String toString() => 'PreparingCreate';
}

class Loading extends AbsenceCreateUpdateState {
  @override
  String toString() => 'Loading';
}

class CreateFailure extends AbsenceCreateUpdateState {
  @override
  String toString() => 'CreateFailure';
}

class UpdateFailure extends AbsenceCreateUpdateState {
  @override
  String toString() => 'UpdateFailure';
}

class CreateSuccessful extends AbsenceCreateUpdateState {
  @override
  String toString() => 'CreateSuccessful';
}

class UpdateSuccessful extends AbsenceCreateUpdateState {
  @override
  String toString() => 'UpdateSuccessful';
}
