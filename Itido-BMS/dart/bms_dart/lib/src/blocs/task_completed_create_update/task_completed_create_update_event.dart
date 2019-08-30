import 'package:bms_dart/src/models/task_completed.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

@immutable
abstract class TaskCompletedCreateUpdateEvent extends Equatable {
  TaskCompletedCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends TaskCompletedCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends TaskCompletedCreateUpdateEvent {
  final TaskCompleted taskCompleted;
  PrepareUpdate({@required this.taskCompleted}) : super([taskCompleted]);
  @override
  String toString() => 'PrepareUpdate { taskCompleted: $taskCompleted }';
}

class CommentChanged extends TaskCompletedCreateUpdateEvent {
  final String text;
  CommentChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'CommentChanged { text: $text }';
}

class CompletedDateChanged extends TaskCompletedCreateUpdateEvent {
  final DateTime dateTime;
  CompletedDateChanged({@required this.dateTime}) : super([dateTime]);
  @override
  String toString() => 'CompletedDateChanged { dateTime: $dateTime }';
}

class Commit extends TaskCompletedCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
