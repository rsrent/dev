import 'package:dart_packages/tuple.dart';
import 'package:flutter/material.dart';
import 'dart:async';
import 'package:rxdart/rxdart.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;
import 'calendar_widget.dart';

pushCalendarScreen(BuildContext context,
    {Function(DateTime) dateSelected,
    //Future<Iterable<Color>> Function(DateTime) dayColors,
    DateTime startDate}) {
  Navigator.push(
    context,
    MaterialPageRoute(
        builder: (context) => CalendarScreen(
              dateSelected: dateSelected,
              //dayColors: dayColors,
              startDate: startDate,
            )),
  );
}

Future<Tuple2<DateTime, DateTime>> pushCalendarScreenAsRangePicker(
    BuildContext context,
    {DateTime startDate,
    DateTime firstSelected,
    DateTime lastSelected,
    Stream<Map<int, Iterable<Color>>> Function(int, int) monthColors,
    bool Function(DateTime) dateActive}) async {
  var dates = await Navigator.push<Tuple2<DateTime, DateTime>>(
    context,
    MaterialPageRoute(
        builder: (context) => CalendarScreen(
              dateSelected: null,
              rangeSelected: (times) {
                Navigator.of(context)
                    .pop(Tuple2<DateTime, DateTime>(times[0], times[1]));
              },
              //dayColors: dayColors,
              startDate: startDate,
              monthColors: monthColors,
              dateActive: dateActive,
              firstSelected: firstSelected,
              lastSelected: lastSelected,
            )),
  );

  return dates;
}

class CalendarScreen extends StatelessWidget {
  final Function(DateTime) dateSelected;
  final bool Function(DateTime) dateActive;
  //final Future<Iterable<Color>> Function(DateTime) dayColors;
  final DateTime startDate;
  final Function(List<DateTime>) rangeSelected;
  final Stream<Map<int, Iterable<Color>>> Function(int, int) monthColors;

  final BehaviorSubject<List<DateTime>> timeSpanStream =
      BehaviorSubject.seeded([]);

  CalendarScreen({
    Key key,
    this.dateSelected,
    this.rangeSelected,
    //this.dayColors,
    this.startDate,
    this.monthColors,
    this.dateActive,
    DateTime firstSelected,
    DateTime lastSelected,
  }) : super(key: key) {
    if (firstSelected != null && lastSelected != null) {
      timeSpanStream
          .add([dtOps.toDate(firstSelected), dtOps.toDate(lastSelected)]);
    } else if (firstSelected != null) {
      timeSpanStream.add([dtOps.toDate(firstSelected)]);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: StreamBuilder(
            stream: timeSpanStream.stream,
            builder: (BuildContext context, AsyncSnapshot snapshot) {
              List<DateTime> times = snapshot.data ?? [];

              var text = 'Vælg en dato';

              if (times.length == 1) {
                text = dtOps.toDDMMyy(times[0]);
              } else if (times.length == 2) {
                text =
                    '${dtOps.toDDMMyy(times[0])} - ${dtOps.toDDMMyy(times[1])}';
              }

              return Text(text);
            },
          ),
          actions: [
            rangeSelected != null
                ? StreamBuilder(
                    stream: timeSpanStream.stream,
                    builder: (BuildContext context, AsyncSnapshot snapshot) {
                      List<DateTime> times = snapshot.data ?? [];
                      return IconButton(
                        icon: Icon(Icons.check),
                        onPressed: () {
                          rangeSelected(times);
                        },
                      );
                    },
                  )
                : null,
          ].where((w) => w != null).toList(),
        ),
        body: CalendarWidget(
          selectTimeSpan: rangeSelected != null,
          timeSpanStream: timeSpanStream,
          dateSelected: dateSelected,
          startDate: startDate,
          monthColors: monthColors,
          dateActive: dateActive,
        ));
  }
}
