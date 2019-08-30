import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/components/check_box_row.dart';
import 'package:bms_flutter/translations.dart';

import '../../components/date_time_picker.dart';
import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/work_contract_create_update_bloc.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;
import 'package:flutter_packages/date_timer_picker.dart'
    as dateTimeDurationPicker;

class WorkContractCreateUpdateForm extends StatefulWidget {
  @override
  _WorkContractCreateUpdateFormState createState() =>
      _WorkContractCreateUpdateFormState();
}

class _WorkContractCreateUpdateFormState
    extends State<WorkContractCreateUpdateForm> {
  TextEditingController _noteController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<WorkContractCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _noteController = _noteController ?? TextEditingController();
          _noteController.text = state.workContract.note;
        }
      },
      child: AnimatedBlocBuilder(
        bloc: bloc,
        builder: (context, WorkContractCreateUpdateState state) {
          if (state.createUpdateStatePhase ==
                  CreateUpdateStatePhase.InProgress ||
              state.createUpdateStatePhase == CreateUpdateStatePhase.Failed) {
            var workContract = state.workContract;
            return SingleChildScrollView(
              child: Padding(
                padding: const EdgeInsets.all(24.0),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    DateTimePicker(
                      labelText: Translations.of(context).labelFrom,
                      selectedDate: state.workContract.fromDate,
                      selectDate: (date) =>
                          bloc.dispatch(FromDateChanged(date: date)),
                      firstDate: bloc.firstDate,
                    ),
                    DateTimePicker(
                      labelText: Translations.of(context).labelTo,
                      selectedDate: state.workContract.toDate,
                      selectDate: (date) =>
                          bloc.dispatch(ToDateChanged(date: date)),
                      firstDate: state.workContract.fromDate,
                    ),
                    TextField(
                      decoration: InputDecoration(
                          labelText: Translations.of(context).labelComment),
                      controller: _noteController,
                      onChanged: (text) =>
                          bloc.dispatch(NoteChanged(note: text)),
                      maxLines: 4,
                    ),
                    Divider(
                      color: Colors.transparent,
                    ),
                    CheckBoxRow(
                      title: Translations.of(context).labelIsVisible,
                      value: workContract.isVisible,
                      onChanged: (isVisible) =>
                          bloc.dispatch(IsVisibleChanged(isVisible: isVisible)),
                    ),
                    CheckBoxRow(
                      title: Translations.of(context).labelEvenUnevenWeeks,
                      value: state.evenUnevenWeeks,
                      onChanged: (evenUnevenWeeks) => bloc.dispatch(
                          EvenUnevenWeeksChanged(
                              evenUnevenWeeks: evenUnevenWeeks)),
                    ),
                    Divider(
                      color: Colors.transparent,
                    ),
                    if (state.evenUnevenWeeks)
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: [
                          Text(
                            Translations.of(context).labelEvenWeeks,
                          ),
                          _buildWeeklyTime(
                            bloc,
                            workContract.workDays.sublist(0, 7),
                          ),
                          Divider(color: Colors.transparent),
                          Text(
                            Translations.of(context).labelUnevenWeeks,
                            //style: style.primaryLabelStyle,
                          ),
                          _buildWeeklyTime(
                            bloc,
                            workContract.workDays.sublist(7),
                          ),
                        ],
                      ),
                    if (!state.evenUnevenWeeks)
                      _buildWeeklyTime(
                        bloc,
                        workContract.workDays,
                      ),
                    Divider(
                      color: Colors.transparent,
                    ),
                    for (var holiday in state.holidays)
                      CheckBoxRow(
                        title: holiday.first.name,
                        value: holiday.second,
                        onChanged: (includeHoliday) => bloc.dispatch(
                          HolidayChanged(
                            holiday: holiday.first,
                            include: includeHoliday,
                          ),
                        ),
                      ),
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
                  ],
                ),
              ),
            );
          }
        },
      ),
    );
  }

  Widget _buildWeeklyTime(
    WorkContractCreateUpdateBloc bloc,
    List<WorkDay> workdays,
  ) {
    const List<String> weekDays = [
      'Man',
      'Tir',
      'Ons',
      'Tor',
      'Fre',
      'Lør',
      'Søn'
    ];

    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: <Widget>[
        Divider(
          height: 8.0,
          color: Colors.transparent,
        ),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: List<Widget>.generate(workdays.length, (i) {
            var workday = workdays[i];
            var startMins = workday.startTimeMins;
            var endMins = workday.endTimeMins;
            return StartTimeDurationButton(
              setStartTime: true,
              duration: endMins - startMins,
              startTime: startMins,
              text: weekDays[i],
              update: (d, s) {
                bloc.dispatch(
                  WorkDayStartTimeChanged(
                    index: workday.index,
                    mins: s,
                  ),
                );
                bloc.dispatch(
                  WorkDayEndTimeChanged(
                    index: workday.index,
                    mins: s + d,
                  ),
                );
              },
              width: 36,
            );
          }),
        ),
      ],
    );
  }
}

class StartTimeDurationButton extends StatelessWidget {
  final bool setStartTime;
  final int duration;
  final int startTime;
  final String text;
  final Function(int, int) update;
  final double width;

  StartTimeDurationButton({
    this.setStartTime,
    this.duration,
    this.startTime,
    this.text,
    this.update,
    this.width,
  });

  @override
  Widget build(BuildContext context) {
    String topTime;
    String middleTime;
    String bottomTime;

    if (startTime + duration == 0) {
      topTime = '';
      middleTime = '-';
      bottomTime = '';
    } else if (setStartTime) {
      topTime = dtOps.minsToHHmm(startTime);
      middleTime = '-';
      bottomTime = dtOps.minsToHHmm(startTime + duration);
    } else {
      topTime = '';
      middleTime = dtOps.minsToHHmm(duration);
      bottomTime = '';
    }

    return Builder(builder: (BuildContext context) {
      return Column(
        children: <Widget>[
          text.length > 0 ? Text(text.substring(0, 1)) : null,
          GestureDetector(
            child: Container(
              decoration: BoxDecoration(
                  boxShadow: null,
                  color: Colors.grey[200],
                  borderRadius: BorderRadius.all(const Radius.circular(8.0))),
              width: width,
              height: 42.0,
              child: Stack(
                alignment: Alignment.center,
                children: <Widget>[
                  Positioned(
                    top: 2,
                    child: Text(
                      topTime,
                      textAlign: TextAlign.center,
                      style: TextStyle(
                        fontSize: 12,
                      ),
                    ),
                    width: width,
                  ),
                  Positioned(
                    top: 12,
                    child: Text(
                      middleTime,
                      textAlign: TextAlign.center,
                    ),
                    width: width,
                  ),
                  Positioned(
                    bottom: 2,
                    child: Text(
                      bottomTime,
                      textAlign: TextAlign.center,
                      style: TextStyle(
                        fontSize: 12,
                      ),
                    ),
                    width: width,
                  ),
                  // RaisedButton(
                  //   elevation: 0,
                  //   color: Colors.transparent,
                  //   onPressed: () ,
                  // ),
                ],
              ),
            ),
            onTap: () {
              var dummyDuration = duration;
              var dummyStartTime = startTime;

              if (dummyStartTime == 0) dummyStartTime = 60 * 6;
              if (dummyDuration == 0) dummyDuration = 60 * 6;

              dateTimeDurationPicker
                  .displayDateTimeDurationPicker(
                context: context,
                startTime: DateTime(2018, 1, 1)
                    .add(Duration(minutes: setStartTime ? dummyStartTime : 0)),
                endTime: DateTime(2018, 1, 1).add(Duration(
                    minutes: setStartTime
                        ? dummyStartTime + dummyDuration
                        : dummyDuration)),
                asDurationPicker: !setStartTime,
                startMinBoundaryTime: DateTime(2018, 1, 1, 0),
                startMaxBoundaryTime:
                    setStartTime ? null : DateTime(2018, 1, 1, 0),
              )
                  .then(
                (times) {
                  var startDateTime = times[0];
                  var endDateTime = times[1];

                  update(endDateTime.difference(startDateTime).inMinutes,
                      startDateTime.hour * 60 + startDateTime.minute);
                },
              );
            },
            onLongPress: () async {
              update(0, 0);
            },
          ),
        ].where((w) => w != null).toList(),
      );
    });
  }
}
