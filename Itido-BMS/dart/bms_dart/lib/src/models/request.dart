import 'package:bms_dart/src/models/approval_state.dart';

import 'date_time_converter.dart';
import 'user.dart';

class Request {
  ApprovalState approvalState;
  bool canRespondToApprovalState;
  DateTime respondDateTime;
  User responder;

  Request({
    this.approvalState,
    this.canRespondToApprovalState,
    this.respondDateTime,
    this.responder,
  });

  factory Request.fromJson(json, {rootJson, path}) {
    if (json == null) return null;
    return Request._fromJson(json, rootJson ?? {}, path ?? '');
  }

  Request._fromJson(json, rootJson, path)
      : this.approvalState = ApprovalState.values[json['approvalState']],
        this.canRespondToApprovalState = json['canRespondToApprovalState'],
        this.respondDateTime = toDateTime(json['respondDateTime']),
        this.responder = User.fromJson(
          json['responder'] ?? rootJson['${path}_responder'],
        );
}
