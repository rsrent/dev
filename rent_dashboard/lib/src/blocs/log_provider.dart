import 'package:flutter/material.dart';
import 'log_bloc.dart';
export 'log_bloc.dart';

class LogProvider extends InheritedWidget {
  final LogBloc logBloc;

  LogProvider({
    Key key,
    Widget child,
    int locationId: 0,
    int customerId: 0,
    int userId: 0,
  })  : logBloc = LogBloc(
          locationId: locationId,
          customerId: customerId,
          userId: userId,
        ),
        super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static LogBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(LogProvider) as LogProvider)
          ?.logBloc;
}
