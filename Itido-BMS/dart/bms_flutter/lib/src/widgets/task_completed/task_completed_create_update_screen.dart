import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/task_completed_create_update_bloc.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'task_completed_create_update_form.dart';

class TaskCompletedCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    TaskCompleted taskCompleted,
    int taskId,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => TaskCompletedCreateUpdateScreen(
        taskCompletedToUpdate: taskCompleted,
        taskId: taskId,
      ),
    ));
  }

  final TaskCompleted taskCompletedToUpdate;
  final int taskId;
  final bool isCreate;

  const TaskCompletedCreateUpdateScreen(
      {Key key, this.taskCompletedToUpdate, this.taskId})
      : isCreate = taskCompletedToUpdate == null,
        super(key: key);

  @override
  _TaskCompletedCreateUpdateScreenState createState() =>
      _TaskCompletedCreateUpdateScreenState();
}

class _TaskCompletedCreateUpdateScreenState
    extends State<TaskCompletedCreateUpdateScreen> {
  bool updated = false;

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async => false,
      child: Scaffold(
        appBar: AppBar(
          leading: IconButton(
            icon: Icon(Icons.arrow_back),
            onPressed: () {
              Navigator.of(context).pop(updated);
            },
          ),
          title: Text(
            widget.isCreate
                ? Translations.of(context).titleCreateTaskCompleted
                : Translations.of(context).titleUpdateTaskCompleted,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            if (widget.isCreate)
              return TaskCompletedCreateUpdateBloc(taskId: widget.taskId)
                ..dispatch(PrepareCreate());
            else
              return TaskCompletedCreateUpdateBloc()
                ..dispatch(PrepareUpdate(
                    taskCompleted: this.widget.taskCompletedToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc =
                  BlocProvider.of<TaskCompletedCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, TaskCompletedCreateUpdateState state) {
                  print('${state.createUpdateStatePhase}');
                  if (state.createUpdateStatePhase ==
                      CreateUpdateStatePhase.Failed) {
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationFailed
                            : Translations.of(context).infoUpdateFailed);
                  }
                  if (state.createUpdateStatePhase ==
                      CreateUpdateStatePhase.Successful) {
                    updated = true;
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationSuccessful
                            : Translations.of(context).infoUpdateSuccessful);
                  }
                },
                child: TaskCompletedCreateUpdateForm(),
              );
            },
          ),
        ),
      ),
    );
  }
}
