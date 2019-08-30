import 'package:bms_dart/src/models/log.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

import '../../models/accident_report.dart';

@immutable
abstract class LogCreateUpdateEvent extends Equatable {
  LogCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareUpdate extends LogCreateUpdateEvent {
  final Log log;
  PrepareUpdate({@required this.log}) : super([log]);
  @override
  String toString() => 'PrepareUpdate { log: $log }';
}

class TitleChanged extends LogCreateUpdateEvent {
  final String text;
  TitleChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'TitleChanged { text: $text }';
}

class BodyChanged extends LogCreateUpdateEvent {
  final String text;
  BodyChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'BodyChanged { text: $text }';
}

class Commit extends LogCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
