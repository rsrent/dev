import 'package:flutter/material.dart';
import 'data_bloc.dart';
export 'data_bloc.dart';

class DataProvider extends InheritedWidget {
  final DataBloc bloc;

  DataProvider(
      {Key key,
      Widget child,
      int customerId: 0,
      int userId: 0,
      int locationId: 0})
      : bloc = DataBloc(
          //scope,
          customerId: customerId,
          userId: userId,
          locationId: locationId,
        ),
        super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static DataBloc of(BuildContext context) {
    var bloc =
        (context.inheritFromWidgetOfExactType(DataProvider) as DataProvider)
            ?.bloc;
    return bloc;
    //return bloc == null || bloc.scope == DataBlocScope.Hide ? null : bloc;
  }
}
