import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class UserInspectEvent extends Equatable {
  UserInspectEvent([List props = const []]) : super(props);
}

class UserInspectEventFetch extends UserInspectEvent {
  @override
  String toString() => 'UserInspectEventStarted';
}

class UserInspectEventLoaded extends UserInspectEvent {
  final User user;
  UserInspectEventLoaded({@required this.user}) : super([user]);
  @override
  String toString() => 'UserInspectEventLoaded';
}

class EnableDisableUser extends UserInspectEvent {
  @override
  String toString() => 'EnableDisableUser';
}
