import 'package:bms_dart/src/models/quality_report.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

@immutable
abstract class QualityReportCreateUpdateEvent extends Equatable {
  QualityReportCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends QualityReportCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends QualityReportCreateUpdateEvent {
  final QualityReport qualityReport;
  PrepareUpdate({@required this.qualityReport}) : super([qualityReport]);
  @override
  String toString() => 'PrepareUpdate { qualityReport: $qualityReport }';
}

class DateTimeCreatedChanged extends QualityReportCreateUpdateEvent {
  final DateTime dateTime;
  DateTimeCreatedChanged({@required this.dateTime}) : super([dateTime]);
  @override
  String toString() => 'DateTimeCreatedChanged { dateTime: $dateTime }';
}

class DateTimeCompletedChanged extends QualityReportCreateUpdateEvent {
  final DateTime dateTime;
  DateTimeCompletedChanged({@required this.dateTime}) : super([dateTime]);
  @override
  String toString() => 'DateTimeCompletedChanged { dateTime: $dateTime }';
}

class Commit extends QualityReportCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
