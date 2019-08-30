import 'package:bms_dart/src/models/absence.dart';
import 'package:bms_dart/src/models/absence_reason.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AbsenceCreateUpdateEvent extends Equatable {
  AbsenceCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends AbsenceCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends AbsenceCreateUpdateEvent {
  final Absence absence;
  PrepareUpdate({@required this.absence}) : super([absence]);
  @override
  String toString() => 'PrepareUpdate { absence: $absence }';
}

class AbsenceReasonChanged extends AbsenceCreateUpdateEvent {
  final AbsenceReason absenceReason;
  AbsenceReasonChanged({@required this.absenceReason}) : super([absenceReason]);
  @override
  String toString() => 'AbsenceReasonChanged { absenceReason: $absenceReason }';
}

class FromChanged extends AbsenceCreateUpdateEvent {
  final DateTime dateTime;
  FromChanged({@required this.dateTime}) : super([dateTime]);
  @override
  String toString() => 'FromChanged { dateTime: $dateTime }';
}

class ToChanged extends AbsenceCreateUpdateEvent {
  final DateTime dateTime;
  ToChanged({@required this.dateTime}) : super([dateTime]);
  @override
  String toString() => 'ToChanged { dateTime: $dateTime }';
}

class CommentChanged extends AbsenceCreateUpdateEvent {
  final String text;
  CommentChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'CommentChanged { text: $text }';
}

class Commit extends AbsenceCreateUpdateEvent {
  final bool asRequest;
  Commit({@required this.asRequest}) : super([asRequest]);
  @override
  String toString() => 'Commit';
}
