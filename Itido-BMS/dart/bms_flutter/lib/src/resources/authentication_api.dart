import 'package:bms_dart/models.dart';
import 'dart:io' show Platform;
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'dart:async';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'client_controller.dart';
import 'local_storage.dart';
import 'dart:io';
import 'package:firebase_messaging/firebase_messaging.dart';

import 'package:package_info/package_info.dart';

class AuthenticationApi extends AuthenticationSource {
  User _user;
  String _token;
  http.Client _client;
  Timer _refreshFirebaseTokenTimer;

  ClientController _clientController;

  final FirebaseAuth _firebaseAuth = FirebaseAuth.instance;

  String path = '${api.path}/api/Logins';

  AuthenticationApi({
    http.Client client,
  }) {
    _client = client ?? http.Client();
    _clientController = ClientController(
      client: _client,
    );
  }

  @override
  void dispose() {
    _client?.close();
    _refreshFirebaseTokenTimer?.cancel();
  }

  @override
  bool isAdmin() {
    print(_user);
    print(_user?.projectRole);
    print(_user?.projectRole?.hasAllPermissions);
    return _user?.projectRole?.hasAllPermissions ?? false;
  }

  @override
  String getToken() => _token;

  @override
  String getUserDisplayname() => _user?.displayName;

  @override
  String getUserRole() => _user?.userRole;

  @override
  int getUserId() => _user?.id;

  @override
  User getUser() => User(
        id: _user.id,
        firstName: _user.firstName,
        lastName: _user.lastName,
        userRole: _user.userRole,
      );

  @override
  int getOrganizationId() => 0;

  @override
  bool isLoggedIn() => _token != null;

  @override
  Future<bool> tryLoginWithToken() async {
    var result = await Storage.read('token');

    print('Stored token: $result');

    if (result == null || result.length == 0) {
      return false;
    }
    _token = result;

    return _loginWithToken();
  }

  @override
  Future<bool> login(
      {String username, String password, String organization}) async {
    try {
      print('login');

      final response =
          await _client.post('$path/Login', headers: api.headers(), body: '''
        {
          "userName":"$username",
          "password":"$password"
        }
        ''');

      if (response.statusCode == 200) {
        await updateUser(response.body);
        await listenToNotifications();
        return true;
      }
    } catch (e) {
      print(e);
    }
    return false;
  }

  Future<bool> _loginWithToken() async {
    try {
      final response =
          await _client.post('$path/LoginWithToken', headers: api.headers());

      print('_loginWithToken: ${response.statusCode}');

      if (response.statusCode == 200) {
        await updateUser(response.body);
        await listenToNotifications();
      } else {
        logout();
      }
    } catch (e) {
      print('Error updating fbToken');
      print(e);
      logout();
      return false;
    }
    return true;
  }

  Future updateUser(encodedJson) async {
    var body = json.decode(encodedJson);
    _token = body['token'];
    Storage.write('token', _token);
    _user = User.fromJson(body['user']);
    var _fbToken = body['fbToken'];

    print('_fbToken: $_fbToken');
    print('_firebaseAuth: $_firebaseAuth');
    print('_firebaseAuth.app: ${_firebaseAuth.app}');
    print('_firebaseAuth.toString(): ${_firebaseAuth.toString()}');

    await _firebaseAuth.signInWithCustomToken(token: _fbToken);

    print('SUCCESS ????');

    _refreshFirebaseTokenTimer?.cancel();
    _refreshFirebaseTokenTimer = Timer(Duration(minutes: 55), () async {
      _loginWithToken();
    });
  }

  @override
  Future<void> logout() async {
    await Storage.write('token', '');
    _user = null;
    _token = null;
    _refreshFirebaseTokenTimer?.cancel();
  }

  final FirebaseMessaging _firebaseMessaging = FirebaseMessaging();

  listenToNotifications() {
    firebaseCloudMessaging_Listeners();
    // REQUEST NOTIFICATIONS
    _firebaseMessaging.requestNotificationPermissions();
    _firebaseMessaging.subscribeToTopic('user_${177}');
  }

  void firebaseCloudMessaging_Listeners() {
    if (Platform.isIOS) iOS_Permission();

    _firebaseMessaging.getToken().then((token) {
      print(token);
    });

    _firebaseMessaging.configure(
      onMessage: (Map<String, dynamic> message) async {
        print('on message $message');
      },
      onResume: (Map<String, dynamic> message) async {
        print('on resume $message');
      },
      onLaunch: (Map<String, dynamic> message) async {
        print('on launch $message');
      },
    );

    //_firebaseMessaging.
  }

  void iOS_Permission() {
    _firebaseMessaging.requestNotificationPermissions(
        IosNotificationSettings(sound: true, badge: true, alert: true));
    _firebaseMessaging.onIosSettingsRegistered
        .listen((IosNotificationSettings settings) {
      print("Settings registered: $settings");
    });
  }

  @override
  Future<int> verifyAppVersion() async {
    PackageInfo packageInfo = await PackageInfo.fromPlatform();
    var platform = Platform.operatingSystem;
    var version = packageInfo.version;

    print('platform: $platform');
    print('version: $version');

    int result = await _clientController.get(
        '$path/ValidateVersion/$platform/$version',
        headers: api.headers());

    print('result: $result');

    return result;
  }
}
