import 'package:bms_dart/src/models/location.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

@immutable
abstract class LocationCreateUpdateEvent extends Equatable {
  LocationCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends LocationCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends LocationCreateUpdateEvent {
  final Location location;
  PrepareUpdate({@required this.location}) : super([location]);
  @override
  String toString() => 'PrepareUpdate { location: $location }';
}

class NameChanged extends LocationCreateUpdateEvent {
  final String text;
  NameChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'NameChanged { text: $text }';
}

class AddressChanged extends LocationCreateUpdateEvent {
  final String text;
  AddressChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'AddressChanged { text: $text }';
}

class CommentChanged extends LocationCreateUpdateEvent {
  final String text;
  CommentChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'CommentChanged { text: $text }';
}

class PhoneChanged extends LocationCreateUpdateEvent {
  final String text;
  PhoneChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'PhoneChanged { text: $text }';
}

class ProjectNumberChanged extends LocationCreateUpdateEvent {
  final String text;
  ProjectNumberChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'ProjectNumberChanged { text: $text }';
}

class Commit extends LocationCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
