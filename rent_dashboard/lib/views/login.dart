import 'package:flutter/material.dart';
import 'dart:async';
import 'dart:io';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:path_provider/path_provider.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import '../network.dart';

class LoginView extends StatefulWidget {
  var loggedIn;
  LoginView(this.loggedIn);
  @override
  _LoginViewState createState() => _LoginViewState(this.loggedIn);
}

class _LoginViewState extends State<LoginView> {
  TextEditingController usernameController;
  TextEditingController passwordController;

  final FocusNode usernameFocusNode = FocusNode();

  var loggedIn;
  _LoginViewState(this.loggedIn) {
    usernameController = TextEditingController();
    passwordController = TextEditingController();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Container(
        padding: const EdgeInsets.only(left: 20.0, right: 20.0, top: 60.0),
        child: Column(
          children: <Widget>[
            TextFormField(
              autofocus: true,
              controller: usernameController,
              decoration: InputDecoration(hintText: 'Brugernavn'),
              onFieldSubmitted: (text) {
                FocusScope.of(context).requestFocus(usernameFocusNode);
              },
            ),
            TextFormField(
              focusNode: usernameFocusNode,
              controller: passwordController,
              decoration: InputDecoration(hintText: 'Kodeord'),
              onFieldSubmitted: attemptLogin,
            ),
          ],
        ),
      ),
    );
  }

//"name":"$name"

  //"Authorization", "Bearer " + token

  void attemptLogin(text) async {
    var baseAPI = 'https://renttesting.azurewebsites.net';
    //var baseAPI = 'https://rentapp.azurewebsites.net';
    var username = usernameController.text;
    var password = passwordController.text;

    http
        .post(baseAPI + '/api/Logins/Login',
            headers: {"content-type": "application/json"},
            body: '{"userName":"$username","password":"$password"}')
        .then((result) {
      var rest = json.decode(result.body);
      if (result.statusCode == 200) {
        loggedIn(rest['token']);
      }
    });
  }
}

class Storage {
  Future<String> get _localPath async {
    final directory = await getApplicationDocumentsDirectory();
    return directory.path;
  }

  Future<File> _localFile(String filename) async {
    final path = await _localPath;
    return File('$path/$filename.txt');
  }

  Future<String> read(String filename) async {
    try {
      final file = await _localFile(filename);

      // Read the file
      String contents = await file.readAsString();

      return contents;
    } catch (e) {
      // If we encounter an error, return 0
      return "";
    }
  }

  Future<File> write(String filename, String object) async {
    final file = await _localFile(filename);

    // Write the file
    return file.writeAsString('$object');
  }
}
