import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_flutter/translations.dart';
// import 'package:bms_flutter/src/screens/accident_report_create_screen.dart';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class HomeMenu extends StatelessWidget {
  var authenticationRepository = repositoryProvider.authenticationRepository();

  @override
  Widget build(BuildContext context) {
    var _authenticationBloc = BlocProvider.of<AuthenticationBloc>(context);

    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: <Widget>[
        Container(
          color: Colors.grey[200],
          child: SafeArea(
            left: false,
            right: false,
            bottom: false,
            child: Container(
              height: 160,
              child: Center(
                child: Text(authenticationRepository.getUserDisplayname()),
              ),
            ),
          ),
        ),
        Expanded(
          child: ListView(
            children: <Widget>[
              ListTile(
                title: Text('English'),
                onTap: () {
                  application.onLocaleChanged(Locale('en'));
                },
              ),
              ListTile(
                title: Text('Dansk'),
                onTap: () {
                  application.onLocaleChanged(Locale('da'));
                },
              ),
            ],
          ),
        ),
        Container(
          child: SafeArea(
            left: false,
            right: false,
            top: false,
            child: Container(
              height: 60,
              child: RaisedButton(
                child: Text(Translations.of(context).buttonLogout),
                onPressed: () {
                  _authenticationBloc.dispatch(LoggedOut());
                },
              ),
            ),
          ),
        ),
      ],
    );
  }
}
