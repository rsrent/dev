import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AbsenceReasonListEvent extends Equatable {
  AbsenceReasonListEvent([List props = const []]) : super(props);
}

class AbsenceReasonListFetch extends AbsenceReasonListEvent {
  @override
  String toString() => 'AbsenceReasonListFetch';
}
