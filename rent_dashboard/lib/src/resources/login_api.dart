import 'package:http/http.dart' show Client;
import 'dart:convert';
import 'dart:async';
import 'login_repository.dart';

final _root = 'https://rentapp.azurewebsites.net';

class LoginApi extends LoginSource {
  Client client = Client();

  Future<String> tryLogin(String username, String password) async {
    final response = await client.post('$_root/api/Logins/Login',
        headers: {"content-type": "application/json"},
        body: '{"userName":"$username","password":"$password"}');
    if(response.statusCode == 200)
    {
      return json.decode(response.body)['token'];
    }
    return null;
  }
}
