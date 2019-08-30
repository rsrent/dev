import 'package:bms_dart/src/models/date_time_converter.dart';
import 'package:bms_dart/src/models/request.dart';

import 'user.dart';

enum AccidentReportType {
  Accident,
  AlmostAccident,
}

class AccidentReport {
  int id;
  AccidentReportType accidentReportType;
  DateTime dateTime;
  String place;
  String description;
  String actionTaken;
  int absenceDurationDays;
  User sender;
  Request request;

  AccidentReport({
    this.dateTime,
    this.place,
    this.description,
    this.actionTaken,
    this.absenceDurationDays,
    this.accidentReportType,
  });

  factory AccidentReport.fromJson(json) {
    if (json == null) return null;
    return AccidentReport._fromJson(json);
  }

  AccidentReport._fromJson(json)
      : this.id = json['id'],
        this.accidentReportType = json['accidentReportType'] != null
            ? AccidentReportType.values[json['accidentReportType']]
            : null,
        this.dateTime = toDateTime(json['dateTime']),
        this.place = json['place'],
        this.description = json['description'],
        this.actionTaken = json['actionTaken'],
        this.absenceDurationDays = json['absenceDurationDays'],
        this.sender = User.fromJson(json['sender']),
        this.request = Request.fromJson(json['request']);

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'accidentReportType': this.accidentReportType?.index,
        'dateTime': this.dateTime?.toString(),
        'place': this.place,
        'description': this.description,
        'actionTaken': this.actionTaken,
        'absenceDurationDays': this.absenceDurationDays,
      };
}
