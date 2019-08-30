import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/task_create_update_bloc.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'task_create_update_form.dart';

class TaskCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Task task,
    int projectId,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => TaskCreateUpdateScreen(
        taskToUpdate: task,
        projectId: projectId,
      ),
    ));
  }

  final Task taskToUpdate;
  final int projectId;
  final bool isCreate;

  const TaskCreateUpdateScreen({Key key, this.taskToUpdate, this.projectId})
      : isCreate = taskToUpdate == null,
        super(key: key);

  @override
  _TaskCreateUpdateScreenState createState() => _TaskCreateUpdateScreenState();
}

class _TaskCreateUpdateScreenState extends State<TaskCreateUpdateScreen> {
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
                ? Translations.of(context).titleCreateTask
                : Translations.of(context).titleUpdateTask,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            if (widget.isCreate)
              return TaskCreateUpdateBloc(projectId: widget.projectId)
                ..dispatch(PrepareCreate());
            else
              return TaskCreateUpdateBloc()
                ..dispatch(PrepareUpdate(task: this.widget.taskToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc = BlocProvider.of<TaskCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, TaskCreateUpdateState state) {
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
                child: TaskCreateUpdateForm(),
              );
            },
          ),
        ),
      ),
    );
  }
}
