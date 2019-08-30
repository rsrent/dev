import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/blocs.dart';

class AuthenticationDecisionScreen extends StatefulWidget {
  final WidgetBuilder splashScreen;
  final WidgetBuilder homeScreen;
  final WidgetBuilder loginScreen;

  const AuthenticationDecisionScreen({
    Key key,
    @required this.splashScreen,
    @required this.homeScreen,
    @required this.loginScreen,
  }) : super(key: key);

  @override
  _AuthenticationDecisionScreenState createState() =>
      _AuthenticationDecisionScreenState();
}

class _AuthenticationDecisionScreenState
    extends State<AuthenticationDecisionScreen> with WidgetsBindingObserver {
  @override
  void initState() {
    WidgetsBinding.instance.addObserver(this);
    super.initState();
  }

  @override
  void dispose() {
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    print('state = $state');
    if (state == AppLifecycleState.resumed)
      BlocProvider.of<AuthenticationBloc>(context).dispatch(AppStarted());
  }

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<AuthenticationBloc>(context);
    return BlocListener(
      bloc: bloc,
      listener: (BuildContext context, AuthenticationState state) {
        if (state is AuthenticationStateUninitialized) {
          //_redirectToPage(context, widget.splashScreen(context));
        } else if (state is AuthenticationStateAppIsOld) {
          showDialog(
            context: context,
            builder: (context) {
              return AlertDialog(
                title: Text('App is old'),
                content: Text(
                    'The apps version is not the newest. To ensure the best experience, update the app'),
                actions: <Widget>[
                  FlatButton(
                    child: Text('Get new version'),
                    onPressed: () {
                      Navigator.of(context).pop();
                    },
                  ),
                  FlatButton(
                    child: Text('Continue'),
                    onPressed: () {
                      Navigator.of(context).pop();
                      bloc.dispatch(AppIsOldAccepted());
                    },
                  ),
                ],
              );
            },
          );
        } else if (state is AuthenticationStateAppTooOld) {
          showDialog(
            context: context,
            builder: (context) {
              return AlertDialog(
                title: Text('App is too old'),
                content:
                    Text('The apps version is too old. Please update the app'),
                actions: <Widget>[
                  FlatButton(
                    child: Text('Ok'),
                    onPressed: () {
                      Navigator.of(context).pop();
                    },
                  ),
                ],
              );
            },
          );
        } else if (state is AuthenticationStateAuthenticated) {
          print('Redirect');
          _redirectToPage(context, widget.homeScreen(context));
        } else if (state is AuthenticationStateUnauthenticated) {
          print('state is AuthenticationStateUnauthenticated');
          _redirectToPage(context, widget.loginScreen(context));
        }
      },
      child: widget.splashScreen(context),
    );
  }

  void _redirectToPage(BuildContext context, Widget page) {
    MaterialPageRoute newRoute =
        MaterialPageRoute(builder: (BuildContext context) => page);

    Navigator.of(context)
        .pushAndRemoveUntil(newRoute, ModalRoute.withName('/'));
    // WidgetsBinding.instance.addPostFrameCallback((_) {
    // });
  }
}
