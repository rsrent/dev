import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_list_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_packages/date_timer_picker.dart'
    as dateTimeDurationPicker;

void registerWork(BuildContext context, Work work, WorkListBloc bloc) {
  dateTimeDurationPicker
      .displayDateTimeDurationPicker(
    context: context,
    startTime: DateTime(2018, 1, 1).add(Duration(minutes: work.startTimeMins)),
    endTime: DateTime(2018, 1, 1).add(Duration(minutes: work.endTimeMins)),
    asDurationPicker: false,
    startMinBoundaryTime: DateTime(2018, 1, 1, 0),
    startMaxBoundaryTime: null,
  )
      .then(
    (times) {
      if (times != null) {
        var startDateTime = times[0];
        var endDateTime = times[1];

        //var bloc = BlocProvider.of<WorkListBloc>(context);
        bloc.dispatch(
          WorkListRegister(
            workId: work.id,
            startTimeMins: startDateTime.minute + (startDateTime.hour * 60),
            endTimeMins: endDateTime.minute + (endDateTime.hour * 60),
          ),
        );
      }
    },
  );
}
