import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AccidentReportListEvent extends Equatable {
  AccidentReportListEvent([List props = const []]) : super(props);
}

class AccidentReportListFetchOfUser extends AccidentReportListEvent {
  final int userId;

  AccidentReportListFetchOfUser({@required this.userId}) : super([userId]);
  @override
  String toString() => 'AccidentReportListFetchOfUser';
}

class AccidentReportListFetchOfSignedInUser extends AccidentReportListEvent {
  @override
  String toString() => 'AccidentReportListFetchOfSignedInUser';
}

class AccidentReportListRespond extends AccidentReportListEvent {
  final int id;
  final bool isApproved;

  AccidentReportListRespond({@required this.id, @required this.isApproved})
      : super([id, isApproved]);

  @override
  String toString() =>
      'AccidentReportListRespond { id: $id, isApproved: $isApproved }';
}

class AccidentReportListResponded extends AccidentReportListEvent {
  final int id;
  final bool isApproved;
  final bool success;

  AccidentReportListResponded(
      {@required this.id, @required this.isApproved, @required this.success})
      : super([id, isApproved, success]);

  @override
  String toString() =>
      'AccidentReportListResponded { id: $id, isApproved: $isApproved, success: $success }';
}
