import 'package:flutter/material.dart';
import 'package:bms_flutter/main.dart' as flutter_app;
import 'package:bms_flutter/src/screens/authentication_decision_screen.dart';

import 'screens/home_screen.dart';
import 'package:bms_flutter/src/widgets/login/widgets.dart';
import 'screens/splash_screen.dart';

void main() {
  flutter_app.main({
    '/': (BuildContext context) => AuthenticationDecisionScreen(
          splashScreen: (context) => SplashScreen(),
          homeScreen: (context) => HomeScreen(),
          loginScreen: (context) => LoginScreen(),
        ),
  });
}
