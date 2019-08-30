import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

import '../../models/work_contract.dart';

@immutable
abstract class WorkContractCreateUpdateEvent extends Equatable {
  WorkContractCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends WorkContractCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends WorkContractCreateUpdateEvent {
  final WorkContract workContract;
  PrepareUpdate({@required this.workContract}) : super([workContract]);
  @override
  String toString() => 'PrepareUpdate { workContract: $workContract }';
}

class Commit extends WorkContractCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}

class FromDateChanged extends WorkContractCreateUpdateEvent {
  final DateTime date;
  FromDateChanged({@required this.date}) : super([date]);
  @override
  String toString() => 'FromDateChanged { date: $date }';
}

class ToDateChanged extends WorkContractCreateUpdateEvent {
  final DateTime date;
  ToDateChanged({@required this.date}) : super([date]);
  @override
  String toString() => 'ToDateChanged { date: $date }';
}

class NoteChanged extends WorkContractCreateUpdateEvent {
  final String note;
  NoteChanged({@required this.note}) : super([note]);
  @override
  String toString() => 'NoteChanged { note: $note }';
}

class IsVisibleChanged extends WorkContractCreateUpdateEvent {
  final bool isVisible;
  IsVisibleChanged({@required this.isVisible}) : super([isVisible]);
  @override
  String toString() => 'IsVisibleChanged { isVisible: $isVisible }';
}

class HolidayChanged extends WorkContractCreateUpdateEvent {
  final Holiday holiday;
  final bool include;
  HolidayChanged({@required this.holiday, @required this.include})
      : super([holiday, include]);
  @override
  String toString() =>
      'HolidayChanged { include: $include, holiday: $holiday }';
}

class EvenUnevenWeeksChanged extends WorkContractCreateUpdateEvent {
  final bool evenUnevenWeeks;
  EvenUnevenWeeksChanged({@required this.evenUnevenWeeks})
      : super([evenUnevenWeeks]);
  @override
  String toString() =>
      'EvenUnevenWeeksChanged { evenUnevenWeeks: $evenUnevenWeeks }';
}

class WorkDayStartTimeChanged extends WorkContractCreateUpdateEvent {
  final int index;
  final int mins;
  WorkDayStartTimeChanged({@required this.index, @required this.mins})
      : super([index, mins]);
  @override
  String toString() => 'WorkDayStartTimeChanged { index: $index, mins: $mins }';
}

class WorkDayEndTimeChanged extends WorkContractCreateUpdateEvent {
  final int index;
  final int mins;
  WorkDayEndTimeChanged({@required this.index, @required this.mins})
      : super([index, mins]);
  @override
  String toString() => 'WorkDayEndTimeChanged { index: $index, mins: $mins }';
}
