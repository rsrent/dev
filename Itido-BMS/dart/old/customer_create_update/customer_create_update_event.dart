import 'package:bms_dart/src/models/customer.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

@immutable
abstract class CustomerCreateUpdateEvent extends Equatable {
  CustomerCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends CustomerCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends CustomerCreateUpdateEvent {
  final Customer customer;
  PrepareUpdate({@required this.customer}) : super([customer]);
  @override
  String toString() => 'PrepareUpdate { customer: $customer }';
}

class NameChanged extends CustomerCreateUpdateEvent {
  final String text;
  NameChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'NameChanged { text: $text }';
}

class CommentChanged extends CustomerCreateUpdateEvent {
  final String text;
  CommentChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'CommentChanged { text: $text }';
}

class HasStandardFoldersChanged extends CustomerCreateUpdateEvent {
  final bool isTrue;
  HasStandardFoldersChanged({@required this.isTrue}) : super([isTrue]);
  @override
  String toString() => 'HasStandardFoldersChanged { isTrue: $isTrue }';
}

class CanReadLogsChanged extends CustomerCreateUpdateEvent {
  final bool isTrue;
  CanReadLogsChanged({@required this.isTrue}) : super([isTrue]);
  @override
  String toString() => 'CanReadLogsChanged { isTrue: $isTrue }';
}

class Commit extends CustomerCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
