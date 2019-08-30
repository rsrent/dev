import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

import '../../models/accident_report.dart';

@immutable
abstract class AccidentReportCreateUpdateEvent extends Equatable {
  AccidentReportCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends AccidentReportCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends AccidentReportCreateUpdateEvent {
  final AccidentReport accidentReport;
  PrepareUpdate({@required this.accidentReport}) : super([accidentReport]);
  @override
  String toString() => 'PrepareUpdate { accidentReport: $accidentReport }';
}

class DateTimeChanged extends AccidentReportCreateUpdateEvent {
  final DateTime dateTime;
  DateTimeChanged({@required this.dateTime}) : super([dateTime]);
  @override
  String toString() => 'DateTimeChanged { dateTime: $dateTime }';
}

class PlaceChanged extends AccidentReportCreateUpdateEvent {
  final String text;
  PlaceChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'PlaceChanged { text: $text }';
}

class DescriptionChanged extends AccidentReportCreateUpdateEvent {
  final String text;
  DescriptionChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'DescriptionChanged { text: $text }';
}

class ActionTakenChanged extends AccidentReportCreateUpdateEvent {
  final String text;
  ActionTakenChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'ActionTakenChanged { text: $text }';
}

class AbsenceDurationChanged extends AccidentReportCreateUpdateEvent {
  final int days;
  AbsenceDurationChanged({@required this.days}) : super([days]);
  @override
  String toString() => 'AbsenceDurationChanged { days: $days }';
}

class Commit extends AccidentReportCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
