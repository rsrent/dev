import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/task_create_update_bloc.dart';
import 'package:bms_flutter/components.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class TaskCreateUpdateForm extends StatefulWidget {
  @override
  _TaskCreateUpdateFormState createState() => _TaskCreateUpdateFormState();
}

class _TaskCreateUpdateFormState extends State<TaskCreateUpdateForm> {
  TextEditingController _descriptionController;
  TextEditingController _placeController;
  TextEditingController _squareMetersController;
  TextEditingController _commentController;
  TextEditingController _frequencyController;
  TextEditingController _timesOfYearController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<TaskCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, TaskCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _descriptionController = (_descriptionController ??
              TextEditingController())
            ..text = state.task.description;
          _placeController = (_placeController ?? TextEditingController())
            ..text = state.task.place;
          _squareMetersController = (_squareMetersController ??
              TextEditingController())
            ..text = (state.task.squareMeters ?? '').toString();
          _commentController = (_commentController ?? TextEditingController())
            ..text = state.task.comment;
          _frequencyController = (_frequencyController ??
              TextEditingController())
            ..text = state.task.frequency;
          _timesOfYearController = (_timesOfYearController ??
              TextEditingController())
            ..text = (state.task.timesOfYear ?? '').toString();
        }
      },
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: BlocBuilder(
            bloc: bloc,
            builder: (context, TaskCreateUpdateState state) {
              return Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Description', filled: true),
                    controller: _descriptionController,
                    onChanged: (text) =>
                        bloc.dispatch(DescriptionChanged(text: text)),
                  ),
                  Space(),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Place', filled: true),
                    controller: _placeController,
                    onChanged: (text) =>
                        bloc.dispatch(PlaceChanged(text: text)),
                  ),
                  Space(),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Comment', filled: true),
                    controller: _commentController,
                    onChanged: (text) =>
                        bloc.dispatch(CommentChanged(text: text)),
                  ),
                  Space(),
                  TextField(
                    decoration: InputDecoration(
                        labelText: 'SquareMeters', filled: true),
                    controller: _squareMetersController,
                    onChanged: (text) =>
                        bloc.dispatch(SquareMetersChanged(text: text)),
                    keyboardType: TextInputType.number,
                  ),
                  Space(),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Frequency', filled: true),
                    controller: _frequencyController,
                    onChanged: (text) =>
                        bloc.dispatch(FrequencyChanged(text: text)),
                    keyboardType: TextInputType.number,
                  ),
                  Space(),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'TimesOfYear', filled: true),
                    controller: _timesOfYearController,
                    onChanged: (text) =>
                        bloc.dispatch(TimesOfYearChanged(text: text)),
                    keyboardType: TextInputType.number,
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
