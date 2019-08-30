import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class LoginEvent extends Equatable {
  LoginEvent([List props = const []]) : super(props);
}

class CheckIfLoggedIn extends LoginEvent {
  @override
  String toString() => 'CheckIfLoggedIn';
}

class LoginButtonPressed extends LoginEvent {
  @override
  String toString() => 'LoginButtonPressed';
}
