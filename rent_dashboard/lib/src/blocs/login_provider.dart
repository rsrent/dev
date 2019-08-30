import 'package:flutter/material.dart';
import 'login_bloc.dart';
export 'login_bloc.dart';

class LoginProvider extends InheritedWidget {
  final LoginBloc bloc = LoginBloc();

  LoginProvider({Key key, Widget child}) : super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static LoginBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(LoginProvider)
              as LoginProvider)
          ?.bloc;
}
