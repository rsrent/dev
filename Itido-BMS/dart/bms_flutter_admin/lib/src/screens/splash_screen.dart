import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class SplashScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              'SPLASH',
              style: TextStyle(fontSize: 50),
            ),
            CircularProgressIndicator(),
          ],
        ),
      ),
      //backgroundColor: Colors.red,
    );
  }
}
