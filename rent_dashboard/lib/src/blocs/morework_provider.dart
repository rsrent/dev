import 'package:flutter/material.dart';
import 'morework_bloc.dart';
export 'morework_bloc.dart';

class MoreWorkProvider extends InheritedWidget {
  final MoreWorkBloc moreworkBloc = MoreWorkBloc();

  MoreWorkProvider({Key key, Widget child}) : super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static MoreWorkBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(MoreWorkProvider)
              as MoreWorkProvider)
          ?.moreworkBloc;
}
