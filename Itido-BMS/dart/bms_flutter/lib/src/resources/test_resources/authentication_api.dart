import 'package:bms_dart/src/models/user.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'dart:async';
import 'package:bms_dart/repositories.dart';
import 'client_faker.dart';

class AuthenticationApi extends AuthenticationSource {
  String _username;
  String _token = 'test token';
  http.Client _client;

  AuthenticationApi({
    http.Client client,
  }) {
    _client = client ?? http.Client();
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  String getToken() => _token;

  @override
  String getUserDisplayname() => _username;

  @override
  bool isLoggedIn() => _token != null;

  @override
  String getUserRole() => 'Admin';

  @override
  int getUserId() => 2;

  @override
  int getOrganizationId() => 0;

  @override
  Future<bool> login(
      {String username, String password, String organization}) async {
    return true;
  }

  @override
  Future<void> logout() async {
    _username = null;
    _token = null;
  }

  @override
  Future<bool> tryLoginWithToken() async {
    // TODO: implement tryLoginWithToken
    return false;
  }

  @override
  Future<int> verifyAppVersion() async {
    return 1;
  }

  @override
  User getUser() => User(
      id: getUserId(),
      firstName: getUserDisplayname(),
      lastName: getUserDisplayname(),
      userRole: getUserRole());

  @override
  bool isAdmin() {
    // TODO: implement isAdmin
    return null;
  }
}
