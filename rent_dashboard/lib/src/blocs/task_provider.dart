import 'package:flutter/material.dart';
import 'task_bloc.dart';
export 'task_bloc.dart';

class TaskProvider extends InheritedWidget {
  final TaskBloc taskBloc;

  TaskProvider({
    Key key,
    Widget child,
    int locationId: 0,
    int customerId: 0,
    int userId: 0,
  })  : taskBloc = TaskBloc(
          locationId: locationId,
          customerId: customerId,
          userId: userId,
        ),
        super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static TaskBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(TaskProvider) as TaskProvider)
          ?.taskBloc;
}
