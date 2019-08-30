import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class LocationInspectState extends Equatable {
  LocationInspectState([List props = const []]) : super(props);
}

class InitialLocationInspectState extends LocationInspectState {
  @override
  String toString() => 'InitialLocationInspectState';
}

// class LoadingLocationInspectState extends LocationInspectState {
//   @override
//   String toString() => 'LoadingLocationInspectState';
// }

class LoadedLocationInspectState extends LocationInspectState {
  final Location location;
  final bool loading;
  LoadedLocationInspectState({@required this.location, this.loading = false})
      : super([location, loading]);
  @override
  String toString() =>
      'LoadedLocationInspectState { location: ${location?.displayName} }';
}

class ErrorLocationInspectState extends LocationInspectState {
  @override
  String toString() => 'ErrorLocationInspectState';
}
