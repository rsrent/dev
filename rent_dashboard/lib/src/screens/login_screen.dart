import 'package:flutter/material.dart';
import '../blocs/login_provider.dart';

class LoginScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final bloc = LoginProvider.of(context);

    return Scaffold(
      body: Container(
        padding: EdgeInsets.all(20.0),
        child: Column(
          children: [
            emailField(bloc),
            passwordField(bloc),
            Divider(color: Colors.transparent),
            submitButton(bloc),
          ],
        ),
        decoration: BoxDecoration(
            gradient: LinearGradient(
          colors: [
            Colors.white,
            Colors.teal[100],
          ],
          begin: Alignment.topCenter,
          end: Alignment.bottomCenter,
        )),
      ),
    );
  }

  Widget emailField(LoginBloc bloc) {
    return StreamBuilder(
      stream: bloc.email,
      builder: (context, AsyncSnapshot<String> snapshot) {
        return TextField(
          decoration: InputDecoration(
            labelText: 'Email address',
            hintText: 'you@example.com',
            errorText: snapshot.error,
          ),
          onChanged: bloc.updateEmail,
        );
      },
    );
  }

  Widget passwordField(LoginBloc bloc) {
    return StreamBuilder(
        stream: bloc.password,
        builder: (context, AsyncSnapshot<String> snapshot) {
          return TextField(
            obscureText: true,
            decoration: InputDecoration(
              labelText: 'Password',
              hintText: 'Password',
              errorText: snapshot.error,
            ),
            onChanged: bloc.updatePassword,
          );
        });
  }

  Widget submitButton(LoginBloc bloc) {
    return StreamBuilder(
      stream: bloc.submitValid,
      builder: (context, AsyncSnapshot<bool> snapshot) {
        return RaisedButton(
          child: Text('Submit'),
          color: Colors.blue,
          onPressed: snapshot.hasData ? bloc.tryLogin : null,
        );
      },
    );
  }
}
