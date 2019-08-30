import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/components/animated_transition.dart';
import 'package:bms_flutter/src/components/date_time_picker.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/accident_report_create_update_bloc.dart';

class AccidentReportCreateUpdateForm extends StatefulWidget {
  @override
  _AccidentReportCreateUpdateFormState createState() =>
      _AccidentReportCreateUpdateFormState();
}

class _AccidentReportCreateUpdateFormState
    extends State<AccidentReportCreateUpdateForm> {
  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<AccidentReportCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, AccidentReportCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {}
      },
      child: AnimatedBlocBuilder(
        bloc: bloc,
        builder: (context, AccidentReportCreateUpdateState state) {
          if (state.createUpdateStatePhase != CreateUpdateStatePhase.Loading &&
              state.createUpdateStatePhase != CreateUpdateStatePhase.Initial) {
            var accidentReport = state.accidentReport;
            return TransitionWidget(
              name: '${state.createUpdateStatePhase}',
              child: SingleChildScrollView(
                child: Padding(
                  padding: const EdgeInsets.all(24.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: <Widget>[
                      DateTimePicker(
                        labelText: Translations.of(context).labelAccidentTime,
                        selectedDate: accidentReport.dateTime,
                        selectDate: (date) =>
                            bloc.dispatch(DateTimeChanged(dateTime: date)),
                        selectedTime:
                            TimeOfDay.fromDateTime(accidentReport.dateTime),
                        selectTime: (time) {
                          var date = accidentReport.dateTime;

                          bloc.dispatch(DateTimeChanged(
                              dateTime: DateTime(
                            date.year,
                            date.month,
                            date.day,
                            time.hour,
                            time.minute,
                          )));
                        },
                      ),
                      Divider(color: Colors.transparent),
                      TextField(
                        decoration: InputDecoration(
                            labelText:
                                Translations.of(context).labelAccidentPlace),
                        onChanged: (text) =>
                            bloc.dispatch(PlaceChanged(text: text)),
                        maxLines: 2,
                      ),
                      Divider(color: Colors.transparent),
                      TextField(
                        decoration: InputDecoration(
                            labelText: Translations.of(context)
                                .labelAccidentDescription),
                        onChanged: (text) =>
                            bloc.dispatch(DescriptionChanged(text: text)),
                        maxLines: 4,
                      ),
                      Divider(color: Colors.transparent),
                      TextField(
                        decoration: InputDecoration(
                            labelText: accidentReport.accidentReportType ==
                                    AccidentReportType.Accident
                                ? Translations.of(context)
                                    .labelAccidentActionTaken
                                : Translations.of(context)
                                    .labelAlmostAccidentActionTaken),
                        onChanged: (text) =>
                            bloc.dispatch(ActionTakenChanged(text: text)),
                        maxLines: 4,
                      ),
                      Divider(color: Colors.transparent),
                      if (accidentReport.accidentReportType ==
                          AccidentReportType.Accident)
                        TextField(
                          decoration: InputDecoration(
                              labelText: Translations.of(context)
                                  .labelAccidentAbsenceDuration),
                          onChanged: (text) => bloc.dispatch(
                              AbsenceDurationChanged(days: int.tryParse(text))),
                          maxLines: 1,
                          keyboardType: TextInputType.number,
                        ),
                      Divider(color: Colors.transparent),
                      Center(
                        child: RaisedButton(
                          child: Text(Translations.of(context).buttonRequest),
                          onPressed: () {
                            bloc.dispatch(Commit());
                          },
                        ),
                      ),
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
