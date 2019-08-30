import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

import '../../models/work.dart';

@immutable
abstract class WorkCreateUpdateEvent extends Equatable {
  WorkCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends WorkCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends WorkCreateUpdateEvent {
  final Work work;
  PrepareUpdate({@required this.work}) : super([work]);
  @override
  String toString() => 'PrepareUpdate { work: $work }';
}

class Commit extends WorkCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}

class DateChanged extends WorkCreateUpdateEvent {
  final DateTime date;
  DateChanged({@required this.date}) : super([date]);
  @override
  String toString() => 'DateChanged { date: $date }';
}

class NoteChanged extends WorkCreateUpdateEvent {
  final String note;
  NoteChanged({@required this.note}) : super([note]);
  @override
  String toString() => 'NoteChanged { note: $note }';
}

class StartTimeMinsChanged extends WorkCreateUpdateEvent {
  final int mins;
  StartTimeMinsChanged({@required this.mins}) : super([mins]);
  @override
  String toString() => 'StartTimeMinsChanged { mins: $mins }';
}

class EndTimeMinsChanged extends WorkCreateUpdateEvent {
  final int mins;
  EndTimeMinsChanged({@required this.mins}) : super([mins]);
  @override
  String toString() => 'EndTimeMinsChanged { mins: $mins }';
}

class BreakMinsChanged extends WorkCreateUpdateEvent {
  final int mins;
  BreakMinsChanged({@required this.mins}) : super([mins]);
  @override
  String toString() => 'BreakMinsChanged { mins: $mins }';
}

class IsVisibleChanged extends WorkCreateUpdateEvent {
  final bool isVisible;
  IsVisibleChanged({@required this.isVisible}) : super([isVisible]);
  @override
  String toString() => 'IsVisibleChanged { isVisible: $isVisible }';
}
