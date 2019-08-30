import 'package:flutter/material.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;
import 'dart:async';
//import '../../style.dart' as style;
import 'package:rxdart/rxdart.dart';

class CalendarWidget extends StatelessWidget {
  final bool selectTimeSpan;
  bool updatedFirstTime = false;
  final Function(DateTime) dateSelected;
  final Stream<Map<int, Iterable<Color>>> Function(int, int) monthColors;
  final Future<Map<int, Iterable<Color>>> Function(int, int) monthColorsFuture;
  static double monthHeight = 320.0;
  static const int baseIndex = 10000;

  final bool Function(DateTime) dateActive;

  static Container genericContainer;
  static Container genericInActiveContainer;

  final BehaviorSubject<List<DateTime>> timeSpanStream;

  static const weekdays = ['M', 'T', 'O', 'T', 'F', 'L', 'S'];
  DateTime today;

  int startYear;
  int startMonth;

  Color primaryColor;
  Color primaryColorAlpha50;
  Color primaryColorAlpha100;
  Border borderSelected;
  BoxDecoration plainBox;
  BoxDecoration todayBox;
  BoxDecoration selectedBox;
  BoxDecoration selectedTodayBox;

  CalendarWidget(
      {Key key,
      this.timeSpanStream,
      this.selectTimeSpan = false,
      this.dateSelected,
      this.monthColors,
      this.monthColorsFuture,
      this.dateActive,
      //Future<Iterable<Color>> Function(DateTime) dayColors,
      DateTime startDate})
      : super(key: key) {
    startDate = startDate ?? DateTime.now();
    startYear = startDate.year;
    startMonth = startDate.month;
    today = DateTime.now();
  }

  static double _getScrollOffsetForIndex(int index) => index * monthHeight;
  final ScrollController scrollController = ScrollController(
      initialScrollOffset: _getScrollOffsetForIndex(baseIndex));

  @override
  Widget build(BuildContext context) {
    primaryColor = Theme.of(context).primaryColor;
    primaryColorAlpha50 = primaryColor.withAlpha(50);
    primaryColorAlpha100 = primaryColor.withAlpha(100);
    borderSelected =
        Border.all(color: primaryColor, width: 2.0, style: BorderStyle.solid);

    plainBox = BoxDecoration(
      color: Colors.white,
      shape: BoxShape.circle,
    );

    todayBox = BoxDecoration(
      color: primaryColorAlpha50,
      shape: BoxShape.circle,
    );

    selectedBox = BoxDecoration(
      color: Colors.white,
      shape: BoxShape.circle,
      border: borderSelected,
    );

    selectedTodayBox = BoxDecoration(
      color: primaryColorAlpha50,
      shape: BoxShape.circle,
      border: borderSelected,
    );

    // BoxDecoration(
    //       color: sameDay ? primaryColorAlpha50 : Colors.white,
    //       shape: BoxShape.circle,
    //       border: isInSelectedRange ? borderSelected : null,
    //     )

    genericContainer = Container(
      margin: EdgeInsets.fromLTRB(2, 2, 2, 2),
      width: 32.0,
      height: 32.0,
      decoration: BoxDecoration(
        boxShadow: null,
        color: Colors.white,
        borderRadius: BorderRadius.all(const Radius.circular(16.0)),
      ),
    );
    genericInActiveContainer = Container(
      margin: EdgeInsets.fromLTRB(2, 2, 2, 2),
      width: 32.0,
      height: 32.0,
    );

    return ListView.builder(
      itemBuilder: (context, index) {
        var diffIndex = index - baseIndex;
        var monthsOffset = diffIndex + startMonth;
        var yearsOffset = startYear + (((monthsOffset - 1) / 12).floor());

        if (monthColorsFuture != null) {
          return _buildMonthFromFuture(
              context, yearsOffset, ((monthsOffset - 1) % 12) + 1);
        } else if (monthColors != null) {
          return _buildMonthFromStream(
              context, yearsOffset, ((monthsOffset - 1) % 12) + 1);
        } else {
          return _buildMonthTimeSpan(
              context, yearsOffset, ((monthsOffset - 1) % 12) + 1, {});
          // return _buildMonth(
          //     context, yearsOffset, ((monthsOffset - 1) % 12) + 1, dayColor);
        }
      },
      itemExtent: monthHeight,
      controller: scrollController,
    );
  }

  _buildMonthFromFuture(BuildContext context, int year, int month) {
    Future<Map<int, Iterable<Color>>> stream = monthColorsFuture(year, month);

    return FutureBuilder(
      future: stream,
      builder: (context, AsyncSnapshot<Map<int, Iterable<Color>>> snapshot) {
        if (snapshot.hasData) {
          return _buildMonthTimeSpan(context, year, month, snapshot.data);
        } else {
          return Center(child: CircularProgressIndicator());
        }
      },
    );
  }

  _buildMonthFromStream(BuildContext context, int year, int month) {
    Stream<Map<int, Iterable<Color>>> stream = monthColors(year, month);

    return StreamBuilder(
      stream: stream,
      builder: (context, AsyncSnapshot<Map<int, Iterable<Color>>> snapshot) {
        if (snapshot.hasData) {
          return _buildMonthTimeSpan(context, year, month, snapshot.data);
        } else {
          return Center(child: CircularProgressIndicator());
        }
      },
    );
  }

  _buildMonthTimeSpan(BuildContext context, int year, int month,
      Map<int, Iterable<Color>> colorMap) {
    if (timeSpanStream != null) {
      return StreamBuilder(
        stream: timeSpanStream.stream,
        builder: (BuildContext context,
            AsyncSnapshot<List<DateTime>> timesSnapshot) {
          var times = timesSnapshot.data ?? [];

          return _buildMonthAsyncV2(context, year, month, colorMap, times);
        },
      );
    } else {
      return _buildMonthAsyncV2(context, year, month, colorMap, []);
    }
  }

  Widget _buildMonthAsyncV2(BuildContext context, int year, int month,
      Map<int, Iterable<Color>> colorMap, List<DateTime> times) {
    final firstDate = DateTime(year, month);
    final daysInMonth = dtOps.daysInMonth(firstDate);
    final lastDate = DateTime(year, month, daysInMonth);

    var startDate = firstDate.add(Duration(days: -(firstDate.weekday - 1)));
    final endDate = lastDate.add(Duration(days: 7 - lastDate.weekday));

    final firstWeekNr = dtOps.weekOfYear(firstDate);

    var rowNum = 0;
    List<Widget> rows = List();
    List<TableRow> tableRows = List();
    while (startDate.compareTo(endDate) != 1) {
      // if (rowNum >= rows.length) {
      //   rows.add(List());
      // }

      rows.add(startDate.month == month
          ? buildDayV3(startDate, context, colorMap[startDate.day] ?? [], times)
          : Container());

      startDate = startDate.add(Duration(days: 1));
      if (startDate.hour == 23) startDate = startDate.add(Duration(hours: 1));
      if (startDate.hour == 1) startDate = startDate.add(Duration(hours: -1));
      if (startDate.weekday == 1) {
        //print(rows[rowNum].length);
        tableRows.add(TableRow(children: rows));
        rows = List();
        rowNum++;
      }
    }

    const fixedWidth = const FixedColumnWidth(30.0);

    return Container(
      height: monthHeight,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: <Widget>[
          Text(
            '${dtOps.dateTimeShortMonthName(firstDate)} $year',
            textAlign: TextAlign.start,
            style: TextStyle(color: primaryColorAlpha100, fontSize: 60.0),
          ),
          Container(
            margin: const EdgeInsets.only(left: 40, right: 8),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: weekdays
                  .map<Widget>((d) => SizedBox(
                      width: 20.0,
                      height: 20.0,
                      child: Text(
                        d,
                        style: TextStyle(color: primaryColorAlpha100),
                      )))
                  .toList(),
            ),
          ),
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              Container(
                width: 24,
                child: Column(
                  children: List.generate(
                    rowNum,
                    (i) => SizedBox(
                      height: 38,
                      child: Center(
                        child: Text(
                          '${firstWeekNr + i}',
                          style: TextStyle(color: primaryColorAlpha100),
                        ),
                      ),
                    ),
                  ),
                ),
              ),
              Expanded(
                child: Container(
                  height: 200,
                  child: Table(
                    columnWidths: const {
                      0: fixedWidth,
                      1: fixedWidth,
                      2: fixedWidth,
                      3: fixedWidth,
                      4: fixedWidth,
                      5: fixedWidth,
                      6: fixedWidth,
                    },
                    children: tableRows,
                  ),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget buildDayV3(DateTime date, BuildContext context, Iterable<Color> colors,
      List<DateTime> times) {
    bool sameDay = dtOps.isSameDate(date, today);
    bool active = dateActive != null ? dateActive(date) : true;

    var isInSelectedRange = times.length > 0 &&
            (times.length == 1 && dtOps.isSameDate(times[0], date)) ||
        times.length == 2 &&
            times[0].compareTo(date) < 1 &&
            date.compareTo(times[1]) < 1;

    return GestureDetector(
      child: Container(
        margin: const EdgeInsets.fromLTRB(2, 2, 2, 2),
        width: 34.0,
        height: 34.0,
        decoration: sameDay
            ? isInSelectedRange ? selectedTodayBox : todayBox
            : isInSelectedRange ? selectedBox : plainBox,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            const SizedBox(height: 9),
            Text(
              '${date.day}',
              style: const TextStyle(fontSize: 10),
              textAlign: TextAlign.center,
            ),
            colors.length > 0
                ? SizedBox(
                    height: 9,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: []..addAll(colors.map((c) {
                          return Container(
                              padding:
                                  const EdgeInsets.only(left: 2.0, bottom: 3),
                              width: 5.0,
                              height: 5.0,
                              decoration: BoxDecoration(
                                  shape: BoxShape.circle, color: c));
                        })),
                    ),
                  )
                : const SizedBox(height: 9),
          ],
        ),
      ),
      onTap: active
          ? () {
              if (selectTimeSpan) {
                if (times.length == 0) {
                  times.add(date);
                  //times.add(date);
                } else if (times.length == 1) {
                  if (dtOps.isSameDate(date, times[0])) {
                    print('DO NOTHING');
                  } else if (date.compareTo(times[0]) < 0) {
                    times.insert(0, date);
                    updatedFirstTime = true;
                  } else {
                    times.add(date);
                    updatedFirstTime = false;
                  }
                } else if (times.length == 2) {
                  if (dtOps.isSameDate(date, times[0])) {
                    times[1] = date;
                  } else if (dtOps.isSameDate(date, times[1])) {
                    times[0] = date;
                  } else if (date.compareTo(times[0]) < 0) {
                    times[0] = date;
                    updatedFirstTime = true;
                  } else if (times[1].compareTo(date) < 0) {
                    times[1] = date;
                    updatedFirstTime = false;
                  } else {
                    if (updatedFirstTime) {
                      times[1] = date;
                      updatedFirstTime = false;
                    } else {
                      times[0] = date;
                      updatedFirstTime = true;
                    }
                  }
                }
                timeSpanStream.add(times);

                print(times);
              } else {
                dateSelected(date);
              }
            }
          : null,
    );
  }
}
