import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

import '../../../models.dart';

@immutable
abstract class AuthenticationState extends Equatable {
  AuthenticationState([List props = const []]) : super(props);
}

class AuthenticationStateUninitialized extends AuthenticationState {
  @override
  String toString() => 'AuthenticationUninitialized';
}

class AuthenticationStateAppTooOld extends AuthenticationState {
  @override
  String toString() => 'AuthenticationStateAppTooOld';
}

class AuthenticationStateAppIsOld extends AuthenticationState {
  @override
  String toString() => 'AuthenticationStateAppIsOld';
}

class AuthenticationStateAuthenticated extends AuthenticationState {
  @override
  String toString() => 'AuthenticationAuthenticated';
}

class AuthenticationStateUnauthenticated extends AuthenticationState {
  @override
  String toString() => 'AuthenticationUnauthenticated';
}
