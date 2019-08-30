import 'package:bms_dart/src/models/address.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

import '../../models/accident_report.dart';

@immutable
abstract class AddressCreateUpdateEvent extends Equatable {
  AddressCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareUpdate extends AddressCreateUpdateEvent {
  final Address address;
  PrepareUpdate({@required this.address}) : super([address]);
  @override
  String toString() => 'PrepareUpdate { address: $address }';
}

class AddressNameChanged extends AddressCreateUpdateEvent {
  final String text;
  AddressNameChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'AddressNameChanged { text: $text }';
}

class PositionChanged extends AddressCreateUpdateEvent {
  final double lat;
  final double lon;
  PositionChanged({@required this.lat, @required this.lon}) : super([lat, lon]);
  @override
  String toString() => 'PositionChanged { lat: $lat, lon: $lon }';
}

class Commit extends AddressCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
