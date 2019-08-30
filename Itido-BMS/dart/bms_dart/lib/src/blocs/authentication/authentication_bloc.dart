import 'dart:async';
import 'package:bloc/bloc.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class AuthenticationBloc
    extends Bloc<AuthenticationEvent, AuthenticationState> {
  @override
  AuthenticationState get initialState => AuthenticationStateUninitialized();
  AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();

  @override
  Stream<AuthenticationState> mapEventToState(
    AuthenticationEvent event,
  ) async* {
    if (event is AppStarted || event is AppIsOldAccepted) {
      bool isLoggedIn = _authenticationRepository.isLoggedIn();

      var appVersionStatus = (event is AppIsOldAccepted || isLoggedIn)
          ? 0
          : (await _authenticationRepository.verifyAppVersion());

      if (appVersionStatus == 1) {
        yield AuthenticationStateAppIsOld();
      } else if (appVersionStatus == 2) {
        yield AuthenticationStateAppTooOld();
      } else {
        //await Future.delayed(Duration(seconds: 1));

        if (!isLoggedIn) {
          isLoggedIn = await _authenticationRepository.tryLoginWithToken();
        }

        if (isLoggedIn) {
          yield AuthenticationStateAuthenticated();
        } else {
          yield AuthenticationStateUnauthenticated();
        }
      }
    }

    if (event is LoggedIn) {
      yield AuthenticationStateAuthenticated();
    }

    if (event is LoggedOut) {
      await _authenticationRepository.logout();
      yield AuthenticationStateUnauthenticated();
    }
  }
}
