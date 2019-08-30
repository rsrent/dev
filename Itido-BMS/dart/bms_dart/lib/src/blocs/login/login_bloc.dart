import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/repositories.dart';
import './bloc.dart';
import '../authentication/bloc.dart';
import 'package:meta/meta.dart';
import '../../repositories/authentication_repository.dart';
import 'package:dart_packages/streamer.dart';
import 'package:rxdart/rxdart.dart';

class LoginBloc extends Bloc<LoginEvent, LoginState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final AuthenticationBloc authenticationBloc;

  Streamer<String> _username;
  Streamer<String> get username => _username;
  Streamer<String> _password;
  Streamer<String> get password => _password;
  Streamer<String> _organization;
  Streamer<String> get organization => _organization;
  Streamer<bool> _formValid;
  Streamer<bool> get formValid => _formValid;

  LoginBloc({
    @required this.authenticationBloc,
  }) : assert(authenticationBloc != null) {
    _username =
        Streamer(seedValue: _authenticationRepository.getUserDisplayname());
    _password = Streamer(seedValue: '');
    _organization = Streamer(seedValue: '');
    _formValid = Streamer(
      seedValue: false,
      source: Observable.combineLatest3<String, String, String, bool>(
          username.stream,
          password.stream,
          organization.stream,
          (u, p, o) => true),
    );
  }

  @override
  LoginState get initialState => LoginInitial();

  @override
  Stream<LoginState> mapEventToState(LoginEvent event) async* {
    if (event is CheckIfLoggedIn) {
      yield LoginLoading();
      var success = await _authenticationRepository.tryLoginWithToken();
      if (success) {
        authenticationBloc.dispatch(LoggedIn());
      }
      yield LoginInitial();
    }

    if (event is LoginButtonPressed) {
      yield LoginLoading();

      try {
        final success = await _authenticationRepository.login(
          username: _username.value,
          password: _password.value,
          organization: _organization.value,
        );
        print('Token: ${_authenticationRepository.getToken()}');
        if (success) {
          authenticationBloc.dispatch(LoggedIn());
          yield LoginInitial();
        } else {
          yield LoginIncorrect();
        }
      } catch (error) {
        yield LoginFailure(error: error.toString());
      }
    }
  }
}
