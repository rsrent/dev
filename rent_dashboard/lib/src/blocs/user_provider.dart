import 'package:flutter/material.dart';
import 'user_bloc.dart';
export 'user_bloc.dart';

class UserProvider extends InheritedWidget {
  final UserBloc bloc = UserBloc();

  UserProvider({Key key, Widget child}) : super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static UserBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(UserProvider)
              as UserProvider)
          ?.bloc;
}