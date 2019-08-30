import 'package:flutter/material.dart';
import 'work_history_bloc.dart';
export 'work_history_bloc.dart';

class WorkHistoryProvider extends InheritedWidget {
  final WorkHistoryBloc bloc;

  WorkHistoryProvider({
    Key key,
    Widget child,
    int customerId: 0,
    int userId: 0,
    int locationId: 0,
    DateTime from,
    int daysBack: 5,
  })  : bloc = WorkHistoryBloc(
          customerId: customerId,
          userId: userId,
          locationId: locationId,
          from: from,
          daysBack: daysBack,
        ),
        super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static WorkHistoryBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(WorkHistoryProvider)
              as WorkHistoryProvider)
          ?.bloc;
}
