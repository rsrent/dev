import 'package:flutter/material.dart';
import 'dg_bloc.dart';
export 'dg_bloc.dart';

class DgProvider extends InheritedWidget {
  final DgBloc bloc;

  DgProvider({
    Key key,
    Widget child,
    int locationId: 0,
    int customerId: 0,
    int userId: 0,
  })  : bloc = DgBloc(
          locationId: locationId,
          customerId: customerId,
          userId: userId,
        ),
        super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static DgBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(DgProvider)
              as DgProvider)
          ?.bloc;
}
