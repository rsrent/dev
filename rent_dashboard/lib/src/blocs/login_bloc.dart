import 'package:rxdart/rxdart.dart';
import 'dart:async';
import '../resources/login_repository.dart';
import '../network.dart';

class LoginBloc {
  final _repository = LoginRepository();
  final _emailSubject = BehaviorSubject<String>();
  final _passwordSubject = BehaviorSubject<String>();
  final _loginSubject = BehaviorSubject<bool>();

  //Stream
  Observable<String> get email =>
      _emailSubject.stream.transform(emailValidator);
  Observable<String> get password =>
      _passwordSubject.stream.transform(passwordValidator);
  Observable<bool> get submitValid =>
      Observable.combineLatest2(email, password, (e, p) => true);
  Observable<bool> get loggedIn =>
      _loginSubject.stream.transform(loginValidator);

  //Sinks
  Function(String) get updateEmail => _emailSubject.sink.add;
  Function(String) get updatePassword => _passwordSubject.sink.add;

  final emailValidator = StreamTransformer<String, String>.fromHandlers(
    handleData: (String email, sink) {
      sink.add(email);
      /*
      RegExp exp = RegExp(
          r'^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$');
      if (exp.hasMatch(email)) {
        sink.add(email);
      } else {
        sink.addError('Enter valid email');
      } */
    },
  );

  final passwordValidator = StreamTransformer<String, String>.fromHandlers(
    handleData: (String password, sink) {
      if (password.length > 3) {
        sink.add(password);
      } else {
        sink.addError('Password must be at least 4 characters');
      }
    },
  );

  final loginValidator = StreamTransformer<bool, bool>.fromHandlers(
      handleData: (bool loggedIn, sink) {
    if (loggedIn != null && loggedIn) {
      sink.add(loggedIn);
    } else {
      sink.addError('Ikke gyldigt login, prøv igen');
    }
  });

  tryLogin() async {
    final validEmail = _emailSubject.value;
    final validPassword = _passwordSubject.value;

    var token = await _repository.tryLogin(validEmail, validPassword);
    Network.token = token;
    _loginSubject.sink.add(token != null);
  }

  dispose() {
    _loginSubject.close();
    _emailSubject.close();
    _passwordSubject.close();
  }
}
