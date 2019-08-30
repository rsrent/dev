import 'package:flutter/material.dart';
import 'location_bloc.dart';
export 'location_bloc.dart';

class LocationProvider extends InheritedWidget {
  final LocationBloc bloc;

  LocationProvider(
      {LocationBlocScope scope: LocationBlocScope.Locations, Key key, Widget child, int customerId: 0, int userId: 0})
      : bloc = LocationBloc(
          scope: scope,
          customerId: customerId,
          userId: userId,
        ),
        super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static LocationBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(LocationProvider)
              as LocationProvider)
          ?.bloc;
}
