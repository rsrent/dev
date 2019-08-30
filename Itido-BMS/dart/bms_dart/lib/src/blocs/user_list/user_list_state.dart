/*
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import 'package:bms_dart/models.dart';

@immutable
abstract class UserListState extends Equatable {
  UserListState([List props = const []]) : super(props);
}

class Loading extends UserListState {
  @override
  String toString() => 'Loading';
}

class Loaded extends UserListState {
  final List<User> users;

  Loaded({@required this.users}) : super([users]);

  @override
  String toString() => 'Loaded { users: ${users.length} }';
}

class Refreshing extends UserListState {
  final List<User> users;

  Refreshing({@required this.users}) : super([users]);

  @override
  String toString() => 'Refreshing { users: ${users.length} }';
}

class Failure extends UserListState {
  @override
  String toString() => 'Failure';
}
*/
