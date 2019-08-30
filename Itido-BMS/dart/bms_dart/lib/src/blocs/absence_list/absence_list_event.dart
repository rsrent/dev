import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AbsenceListEvent extends Equatable {
  AbsenceListEvent([List props = const []]) : super(props);
}

// class AbsenceListFetchAll extends AbsenceListEvent {
//   @override
//   String toString() => 'AbsenceListFetchAll';
// }

class AbsenceListFetchOfUser extends AbsenceListEvent {
  final int userId;

  AbsenceListFetchOfUser({@required this.userId}) : super([userId]);
  @override
  String toString() => 'AbsenceListFetchOfUser';
}

class AbsenceListFetchOfSignedInUser extends AbsenceListEvent {
  @override
  String toString() => 'AbsenceListFetchOfSignedInUser';
}

class AbsenceListRespond extends AbsenceListEvent {
  final int id;
  final bool isApproved;

  AbsenceListRespond({@required this.id, @required this.isApproved})
      : super([id, isApproved]);

  @override
  String toString() =>
      'AbsenceListRespond { id: $id, isApproved: $isApproved }';
}

class AbsenceListResponded extends AbsenceListEvent {
  final int id;
  final bool isApproved;
  final bool success;

  AbsenceListResponded(
      {@required this.id, @required this.isApproved, @required this.success})
      : super([id, isApproved, success]);

  @override
  String toString() =>
      'AbsenceListResponded { id: $id, isApproved: $isApproved, success: $success }';
}
