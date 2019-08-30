import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class UserInspectState extends Equatable {
  UserInspectState([List props = const []]) : super(props);
}

class InitialUserInspectState extends UserInspectState {
  @override
  String toString() => 'InitialUserInspectState';
}

class LoadingUserInspectState extends UserInspectState {
  @override
  String toString() => 'LoadingUserInspectState';
}

class LoadedUserInspectState extends UserInspectState {
  final User user;
  final bool loading;
  LoadedUserInspectState({@required this.user, this.loading = false})
      : super([user, loading]);
  @override
  String toString() => 'LoadedUserInspectState { user: ${user?.displayName} }';
}

class ErrorUserInspectState extends UserInspectState {
  @override
  String toString() => 'ErrorUserInspectState';
}
