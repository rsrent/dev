import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/task_completed_create_update_bloc.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/date_time_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class TaskCompletedCreateUpdateForm extends StatefulWidget {
  @override
  _TaskCompletedCreateUpdateFormState createState() =>
      _TaskCompletedCreateUpdateFormState();
}

class _TaskCompletedCreateUpdateFormState
    extends State<TaskCompletedCreateUpdateForm> {
  TextEditingController _commentController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<TaskCompletedCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, TaskCompletedCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _commentController = (_commentController ?? TextEditingController())
            ..text = state.taskCompleted.comment;
        }
      },
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: BlocBuilder(
            bloc: bloc,
            builder: (context, TaskCompletedCreateUpdateState state) {
              return Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Comment', filled: true),
                    controller: _commentController,
                    onChanged: (text) =>
                        bloc.dispatch(CommentChanged(text: text)),
                  ),
                  Space(),
                  DateTimePicker(
                    labelText: 'Date',
                    selectedDate: state.taskCompleted.completedDate,
                    selectDate: (date) =>
                        bloc.dispatch(CompletedDateChanged(dateTime: date)),
                  ),
                  Space(height: 40),
                  Center(
                    child: RaisedButton(
                      child: Text('SUBMIT'),
                      onPressed: state.isValid
                          ? () {
                              bloc.dispatch(Commit());
                            }
                          : null,
                    ),
                  ),
                  Space(height: 40),
                ],
              );
            },
          ),
        ),
      ),
    );
  }
}
