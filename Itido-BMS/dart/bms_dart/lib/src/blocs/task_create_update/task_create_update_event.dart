import 'package:bms_dart/src/models/task.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

@immutable
abstract class TaskCreateUpdateEvent extends Equatable {
  TaskCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends TaskCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends TaskCreateUpdateEvent {
  final Task task;
  PrepareUpdate({@required this.task}) : super([task]);
  @override
  String toString() => 'PrepareUpdate { task: $task }';
}

class DescriptionChanged extends TaskCreateUpdateEvent {
  final String text;
  DescriptionChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'DescriptionChanged { text: $text }';
}

class PlaceChanged extends TaskCreateUpdateEvent {
  final String text;
  PlaceChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'PlaceChanged { text: $text }';
}

class CommentChanged extends TaskCreateUpdateEvent {
  final String text;
  CommentChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'CommentChanged { text: $text }';
}

class SquareMetersChanged extends TaskCreateUpdateEvent {
  final String text;
  SquareMetersChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'SquareMetersChanged { text: $text }';
}

class FrequencyChanged extends TaskCreateUpdateEvent {
  final String text;
  FrequencyChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'FrequencyChanged { text: $text }';
}

class TimesOfYearChanged extends TaskCreateUpdateEvent {
  final String text;
  TimesOfYearChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'TimesOfYearChanged { text: $text }';
}

class Commit extends TaskCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
