import 'package:bms_dart/src/models/agreement.dart';
import 'package:bms_dart/src/models/contract.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

@immutable
abstract class ContractCreateUpdateEvent extends Equatable {
  ContractCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends ContractCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends ContractCreateUpdateEvent {
  final Contract contract;
  PrepareUpdate({@required this.contract}) : super([contract]);
  @override
  String toString() => 'PrepareUpdate { contract: $contract }';
}

class AgreementChanged extends ContractCreateUpdateEvent {
  final Agreement agreement;
  AgreementChanged({@required this.agreement}) : super([agreement]);
  @override
  String toString() => 'AgreementChanged { agreement: $agreement }';
}

class FromChanged extends ContractCreateUpdateEvent {
  final DateTime dateTime;
  FromChanged({@required this.dateTime}) : super([dateTime]);
  @override
  String toString() => 'FromChanged { dateTime: $dateTime }';
}

class ToChanged extends ContractCreateUpdateEvent {
  final DateTime dateTime;
  ToChanged({@required this.dateTime}) : super([dateTime]);
  @override
  String toString() => 'ToChanged { dateTime: $dateTime }';
}

class WeeklyHoursChanged extends ContractCreateUpdateEvent {
  final String text;
  WeeklyHoursChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'WeeklyHoursChanged { text: $text }';
}

class Commit extends ContractCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
