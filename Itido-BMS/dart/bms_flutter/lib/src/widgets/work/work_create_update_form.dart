import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/animated_transition.dart';
import 'package:bms_flutter/src/components/check_box_row.dart';
import 'package:bms_flutter/src/components/date_time_picker.dart';
import 'package:bms_flutter/translations.dart';

import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/work_create_update_bloc.dart';
import 'package:flutter_packages/time_of_day_operations.dart';

class WorkCreateUpdateForm extends StatefulWidget {
  @override
  _WorkCreateUpdateFormState createState() => _WorkCreateUpdateFormState();
}

class _WorkCreateUpdateFormState extends State<WorkCreateUpdateForm> {
  TextEditingController _breakMinsController;
  TextEditingController _noteController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<WorkCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, WorkCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _breakMinsController =
              _breakMinsController ?? TextEditingController();
          _noteController = _noteController ?? TextEditingController();
          _breakMinsController.text = '${state.work?.breakMins ?? ''}';
          _noteController.text = state.work?.note;
        }
      },
      child: AnimatedBlocBuilder(
        bloc: bloc,
        builder: (context, WorkCreateUpdateState state) {
          if (state.createUpdateStatePhase != CreateUpdateStatePhase.Loading &&
              state.createUpdateStatePhase != CreateUpdateStatePhase.Initial) {
            print(state.isCreate);

            var work = state.work;
            return TransitionWidget(
              name: '${state.createUpdateStatePhase}',
              child: SingleChildScrollView(
                child: Padding(
                  padding: const EdgeInsets.all(24.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: <Widget>[
                      if (state.isCreate)
                        DateTimePicker(
                          labelText: Translations.of(context).labelDate,
                          selectedDate: state.work.date,
                          selectDate: (date) =>
                              bloc.dispatch(DateChanged(date: date)),
                        ),
                      DateTimePicker(
                        labelText: Translations.of(context).labelStartTime,
                        selectedTime: minsToTimeOfDay(work.startTimeMins),
                        selectTime: (time) => bloc.dispatch(
                            StartTimeMinsChanged(mins: timeOfDayToMins(time))),
                      ),
                      DateTimePicker(
                        labelText: Translations.of(context).labelEndTime,
                        selectedTime: minsToTimeOfDay(work.endTimeMins),
                        selectTime: (time) => bloc.dispatch(
                            EndTimeMinsChanged(mins: timeOfDayToMins(time))),
                      ),
                      TextField(
                        decoration: InputDecoration(
                            labelText:
                                Translations.of(context).labelBreakDuration),
                        controller: _breakMinsController,
                        onChanged: (text) => bloc.dispatch(
                            BreakMinsChanged(mins: int.tryParse(text))),
                        keyboardType: TextInputType.number,
                      ),
                      TextField(
                        decoration: InputDecoration(
                            labelText: Translations.of(context).labelComment),
                        controller: _noteController,
                        onChanged: (text) =>
                            bloc.dispatch(NoteChanged(note: text)),
                        maxLines: 4,
                      ),
                      CheckBoxRow(
                        title: Translations.of(context).labelIsVisible,
                        value: work.isVisible ?? false,
                        onChanged: (isVisible) => bloc
                            .dispatch(IsVisibleChanged(isVisible: isVisible)),
                      ),
                      Space(height: 40),
                      Center(
                        child: RaisedButton(
                          child: Text(state.isCreate
                              ? Translations.of(context).buttonCreate
                              : Translations.of(context).buttonUpdate),
                          onPressed: () {
                            bloc.dispatch(Commit());
                          },
                        ),
                      ),
                      Space(height: 40),
                    ],
                  ),
                ),
              ),
            );
          }
        },
      ),
    );
  }
}
