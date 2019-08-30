import 'dart:async';
import 'login_api.dart';

class LoginRepository extends LoginSource {
  LoginSource loginApi = LoginApi();

  Future<String> tryLogin(String username, String password) async {
    return await loginApi.tryLogin(username, password);
  }
}

abstract class LoginSource {
  Future<String> tryLogin(String username, String password);
}