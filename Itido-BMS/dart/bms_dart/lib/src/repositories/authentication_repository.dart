import 'source.dart';
import 'package:meta/meta.dart';
import '../models/user.dart';

abstract class AuthenticationSource extends Source {
  bool isLoggedIn();
  Future<bool> tryLoginWithToken();
  Future<bool> login({
    @required String username,
    @required String password,
    @required String organization,
  });
  Future<void> logout();
  User getUser();
  String getUserDisplayname();
  String getUserRole();
  String getToken();
  int getUserId();
  int getOrganizationId();
  bool isAdmin();

  Future<int> verifyAppVersion();
}

class AuthenticationRepository extends AuthenticationSource {
  AuthenticationSource source;

  AuthenticationRepository(this.source);

  bool isLoggedIn() => source.isLoggedIn();

  Future<bool> tryLoginWithToken() => source.tryLoginWithToken();

  Future<bool> login({
    @required String username,
    @required String password,
    @required String organization,
  }) =>
      source.login(
          username: username, password: password, organization: organization);

  Future<void> logout() => source.logout();
  String getUserDisplayname() => source.getUserDisplayname();
  String getToken() => source.getToken();
  User getUser() => source.getUser();
  int getUserId() => source.getUserId();
  int getOrganizationId() => source.getOrganizationId();
  bool isAdmin() => source.isAdmin();

  @override
  String getUserRole() => source.getUserRole();

  void dispose() {
    source.dispose();
  }

  @override
  Future<int> verifyAppVersion() => source.verifyAppVersion();
}
