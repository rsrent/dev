import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/blocs.dart';

class LoginForm extends StatefulWidget {
  @override
  _LoginFormState createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<LoginBloc>(context);

    return AnimatedBlocBuilder(
      bloc: bloc,
      builder: (context, LoginState state) {
        if (state is LoginInitial ||
            state is LoginFailure ||
            state is LoginIncorrect) {
          return _buildBody(context, bloc);
        }
        // else {
        //   return Container(
        //     color: Colors.lightBlue,
        //     child: Center(
        //       child: CircularProgressIndicator(),
        //     ),
        //   );
        // }

        // print('wasPreviousFirst: $wasPreviousFirst');
        // if (wasPreviousFirst) {
        //   secondWidget = newChild;
        //   wasPreviousFirst = false;
        // } else {
        //   firstWidget = newChild;
        //   wasPreviousFirst = true;
        // }

        // return AnimatedCrossFade(
        //   firstChild: firstWidget,
        //   secondChild: secondWidget,
        //   crossFadeState: wasPreviousFirst
        //       ? CrossFadeState.showFirst
        //       : CrossFadeState.showSecond,
        //   duration: Duration(milliseconds: 1000),
        // );
      },
    );
  }

  Widget _buildBody(BuildContext context, LoginBloc bloc) {
    return Container(
      color: Theme.of(context).scaffoldBackgroundColor,
      child: Center(
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.all(24.0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                StreamBuilder(
                  stream: bloc.username.stream,
                  builder: (BuildContext context, AsyncSnapshot snapshot) {
                    return Padding(
                      padding: const EdgeInsets.all(8),
                      child: TextField(
                        onChanged: bloc.username.update,
                        decoration: InputDecoration(
                          labelText: Translations.of(context).labelEmail,
                          errorText: snapshot.error,
                          filled: true,
                        ),
                      ),
                    );
                  },
                ),
                StreamBuilder(
                  stream: bloc.password.stream,
                  builder: (BuildContext context, AsyncSnapshot snapshot) {
                    return Padding(
                      padding: const EdgeInsets.all(8),
                      child: TextField(
                        onChanged: bloc.password.update,
                        decoration: InputDecoration(
                          labelText: Translations.of(context).labelPassword,
                          errorText: snapshot.error,
                          filled: true,
                        ),
                        obscureText: true,
                      ),
                    );
                  },
                ),
                StreamBuilder(
                  stream: bloc.organization.stream,
                  builder: (BuildContext context, AsyncSnapshot snapshot) {
                    return Padding(
                      padding: const EdgeInsets.all(8),
                      child: TextField(
                        onChanged: bloc.organization.update,
                        decoration: InputDecoration(
                          labelText: Translations.of(context).labelOrganization,
                          errorText: snapshot.error,
                          filled: true,
                        ),
                      ),
                    );
                  },
                ),
                StreamBuilder(
                  stream: bloc.formValid.stream,
                  builder:
                      (BuildContext context, AsyncSnapshot<bool> snapshot) {
                    return Padding(
                      padding: const EdgeInsets.all(8),
                      child: Center(
                        child: RaisedButton(
                          child: Text(Translations.of(context).buttonLogin),
                          onPressed: (snapshot.data ?? false)
                              ? () {
                                  bloc.dispatch(LoginButtonPressed());
                                }
                              : null,
                        ),
                      ),
                    );
                  },
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
