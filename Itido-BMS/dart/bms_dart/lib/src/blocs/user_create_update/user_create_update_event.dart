import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/models/user.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

import '../../models/accident_report.dart';

@immutable
abstract class UserCreateUpdateEvent extends Equatable {
  UserCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends UserCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends UserCreateUpdateEvent {
  final User user;
  PrepareUpdate({@required this.user}) : super([user]);
  @override
  String toString() => 'PrepareUpdate { user: $user }';
}

class UserNameChanged extends UserCreateUpdateEvent {
  final String text;
  UserNameChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'UserNameChanged { text: $text }';
}

class PasswordChanged extends UserCreateUpdateEvent {
  final String text;
  PasswordChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'PasswordChanged { text: $text }';
}

class FirstNameChanged extends UserCreateUpdateEvent {
  final String text;
  FirstNameChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'FirstNameChanged { text: $text }';
}

class LastNameChanged extends UserCreateUpdateEvent {
  final String text;
  LastNameChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'LastNameChanged { text: $text }';
}

class EmailChanged extends UserCreateUpdateEvent {
  final String text;
  EmailChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'EmailChanged { text: $text }';
}

class PhoneChanged extends UserCreateUpdateEvent {
  final String text;
  PhoneChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'PhoneChanged { text: $text }';
}

class CommentChanged extends UserCreateUpdateEvent {
  final String text;
  CommentChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'CommentChanged { text: $text }';
}

class LanguageCodeChanged extends UserCreateUpdateEvent {
  final String text;
  LanguageCodeChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'LanguageCodeChanged { text: $text }';
}

class UserRoleChanged extends UserCreateUpdateEvent {
  final String text;
  UserRoleChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'UserRoleChanged { text: $text }';
}

class EmployeeNumberChanged extends UserCreateUpdateEvent {
  final String text;
  EmployeeNumberChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'EmployeeNumberChanged { text: $text }';
}

class ProjectRoleChanged extends UserCreateUpdateEvent {
  final ProjectRole projectRole;
  ProjectRoleChanged({@required this.projectRole}) : super([projectRole]);
  @override
  String toString() => 'ProjectRoleChanged { projectRole: $projectRole }';
}

class ClientChanged extends UserCreateUpdateEvent {
  final Client client;
  ClientChanged({@required this.client}) : super([client]);
  @override
  String toString() => 'ClientChanged { client: $client }';
}

class IsClientChanged extends UserCreateUpdateEvent {
  final bool isOn;
  IsClientChanged({@required this.isOn}) : super([isOn]);
  @override
  String toString() => 'IsClientChanged { isOn: $isOn }';
}

class Commit extends UserCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
