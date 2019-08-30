import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class LocationInspectEvent extends Equatable {
  LocationInspectEvent([List props = const []]) : super(props);
}

class LocationInspectEventFetch extends LocationInspectEvent {
  @override
  String toString() => 'LocationInspectEventStarted';
}

class LocationInspectEventLoaded extends LocationInspectEvent {
  final Location location;
  LocationInspectEventLoaded({@required this.location}) : super([location]);
  @override
  String toString() => 'LocationInspectEventLoaded';
}

class EnableDisableLocation extends LocationInspectEvent {
  @override
  String toString() => 'EnableDisableLocation';
}
