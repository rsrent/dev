import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/models/date_time_converter.dart';
import 'package:flutter/material.dart';

class WorkRegistration {
  // int workId;
  DateTime date;
  int startTimeMins;
  int endTimeMins;
  int breakMins;
  Request request;

  WorkRegistration({
    // this.workId,
    this.date,
    this.startTimeMins,
    this.endTimeMins,
    this.breakMins,
  });

  factory WorkRegistration.fromJson(json, {rootJson, path}) {
    if (json == null) return null;
    return WorkRegistration._fromJson(json, rootJson ?? {}, path ?? '');
  }

  WorkRegistration._fromJson(json, rootJson, path)
      : this.date = toDateTime(json['date']),
        this.startTimeMins = json['startTimeMins'],
        this.endTimeMins = json['endTimeMins'],
        this.breakMins = json['breakMins'],
        this.request = Request.fromJson(
          json['request'] ?? rootJson['${path}_request'],
          rootJson: rootJson,
          path: '${path}_request',
        );

  Map<String, dynamic> toMap() => {
        'date': this.date.toString(),
        'startTimeMins': this.startTimeMins,
        'endTimeMins': this.endTimeMins,
        'breakMins': this.breakMins,
      };
}
