import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/blocs.dart';

import 'login_form.dart';

class LoginScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: Container(),
        title: Text(Translations.of(context).buttonLogin),
      ),
      body: BlocProvider(
        builder: (context) {
          return LoginBloc(
            authenticationBloc: BlocProvider.of<AuthenticationBloc>(context),
          )..dispatch(CheckIfLoggedIn());
        },
        child: Builder(
          builder: (context) {
            return BlocListener(
              bloc: BlocProvider.of<LoginBloc>(context),
              listener: (context, LoginState state) {
                if (state is LoginIncorrect) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content: Text(Translations.of(context).infoLoginFailed),
                    ));
                }
              },
              child: LoginForm(),
            );
          },
        ),
      ),
    );
  }
}
