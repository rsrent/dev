import 'package:bms_dart/src/models/absence_reason.dart';
import 'package:bms_dart/src/models/date_time_converter.dart';

import 'approval_state.dart';
import 'request.dart';
import 'user.dart';

class Absence {
  int id;
  String comment;
  //String description;
  DateTime from;
  DateTime to;
  bool isRequest;
  //bool canRespondToApprovalState;
  User creator;
  AbsenceReason absenceReason;
  Request request;

  Absence({
    this.id,
    this.comment,
    this.from,
    this.to,
    this.isRequest,
    //this.canRespondToApprovalState,
  });

  factory Absence.fromJson(json, {rootJson, path}) {
    if (json == null) return null;
    return Absence._fromJson(json, rootJson ?? {}, path ?? '');
  }

  Absence._fromJson(json, rootJson, path)
      : this.id = json['id'],
        // this.description = json['description'],
        this.comment = json['comment'],
        this.from = toDateTime(json['from']),
        this.to = toDateTime(json['to']),
        this.isRequest = json['isRequest'],
        //this.canRespondToApprovalState = json['canRespondToApprovalState'],
        this.creator = User.fromJson(json['creator']),
        this.absenceReason = AbsenceReason.fromJson(
            json['absenceReason'] ?? rootJson['${path}_absenceReason']),
        this.request = Request.fromJson(json['request']);

  Map<String, dynamic> toMap() => {
        'id': this.id,
        // 'description': this.description,
        'comment': this.comment,
        'from': this.from?.toString(),
        'to': this.to?.toString(),
        'isRequest': this.isRequest,
        //'canRespondToApprovalState': this.canRespondToApprovalState,
      };
}
