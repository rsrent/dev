import 'package:flutter/material.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;
import 'dart:async';
//import '../../style.dart' as style;
import 'package:rxdart/rxdart.dart';

class CalendarWidget extends StatefulWidget {
  final bool selectTimeSpan;

  //final Stream<Map<int, Iterable<Color>>> Function(int, int) monthColors;
  final Future<Map<int, Iterable<Color>>> Function(int, int) monthColorsFuture;
  final Function(DateTime) dateSelected;
  final bool Function(DateTime) dateActive;
  final BehaviorSubject<List<DateTime>> timeSpanStream;

  DateTime today;

  int startYear;
  int startMonth;

  CalendarWidget(
      {Key key,
      this.timeSpanStream,
      this.selectTimeSpan = false,
      this.dateSelected,
      //this.monthColors,
      this.monthColorsFuture,
      this.dateActive,
      DateTime startDate})
      : super(key: key) {
    startDate = startDate ?? DateTime.now();
    startYear = startDate.year;
    startMonth = startDate.month;
    today = DateTime.now();
  }

  @override
  _CalendarWidgetState createState() => _CalendarWidgetState();
}

class _CalendarWidgetState extends State<CalendarWidget> {
  bool prepared = false;
  bool updatedFirstTime = false;

  static const int baseIndex = 10000;
  static double monthHeight = 320.0;
  static const weekdays = ['M', 'T', 'O', 'T', 'F', 'L', 'S'];

  Map<String, GlobalKey> keys = {};
  Map<Color, Widget> cachedColorDots = {};

  Color primaryColor;
  Color primaryColorAlpha50;
  Color primaryColorAlpha100;
  Border borderSelected;
  BoxDecoration plainBox;
  BoxDecoration todayBox;
  BoxDecoration selectedBox;
  BoxDecoration selectedTodayBox;

  static Container genericContainer;
  static Container genericInActiveContainer;

  static double _getScrollOffsetForIndex(int index) => index * monthHeight;
  final ScrollController scrollController = ScrollController(
      initialScrollOffset: _getScrollOffsetForIndex(baseIndex));

  @override
  void initState() {
    Future.delayed(Duration.zero, () {
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
      prepared = true;
      setState(() {});
    });
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    if (!prepared) {
      return Container();
    }

    return ListView.builder(
      itemBuilder: (context, index) {
        var diffIndex = index - baseIndex;
        var monthsOffset = diffIndex + widget.startMonth;
        var yearsOffset =
            widget.startYear + (((monthsOffset - 1) / 12).floor());

        if (widget.monthColorsFuture != null) {
          return _buildMonthFromFuture(
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
    Future<Map<int, Iterable<Color>>> stream =
        widget.monthColorsFuture(year, month);

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

  _buildMonthTimeSpan(BuildContext context, int year, int month,
      Map<int, Iterable<Color>> colorMap) {
    if (widget.timeSpanStream != null) {
      return StreamBuilder(
        stream: widget.timeSpanStream.stream,
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
    var k = '$year-$month';
    keys.putIfAbsent('$year-$month', () => GlobalKey());

    return MonthWidget(
      key: keys[k],
      dateActive: widget.dateActive,
      primaryColorAlpha100: primaryColorAlpha100,
      plainBox: plainBox,
      todayBox: todayBox,
      today: widget.today,
      onSelect: (date) {
        if (widget.selectTimeSpan) {
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
          widget.timeSpanStream.add(times);

          print(times);
        } else {
          widget.dateSelected(date);
        }
      },
      cachedColorDots: cachedColorDots,
      selectedBox: selectedBox,
      selectedTodayBox: selectedTodayBox,
      colorMap: colorMap,
      times: times,
      year: year,
      month: month,
    );
  }
}

class MonthWidget extends StatefulWidget {
  final int year;
  final int month;
  final Map<int, Iterable<Color>> colorMap;
  final List<DateTime> times;
  //final Function(DateTime) dateSelected;
  final bool Function(DateTime) dateActive;
  //final BehaviorSubject<List<DateTime>> timeSpanStream;

  final Map<Color, Widget> cachedColorDots;
  //final Color primaryColor;
  //final Color primaryColorAlpha50;
  final Color primaryColorAlpha100;
  //final Border borderSelected;
  final BoxDecoration plainBox;
  final BoxDecoration todayBox;
  final BoxDecoration selectedBox;
  final BoxDecoration selectedTodayBox;
  //final bool selectTimeSpan;
  final DateTime today;
  //final bool updatedFirstTime;

  final Function(DateTime) onSelect;

  const MonthWidget({
    Key key,
    @required this.dateActive,
    @required this.primaryColorAlpha100,
    @required this.plainBox,
    @required this.todayBox,
    @required this.selectedBox,
    @required this.selectedTodayBox,
    @required this.today,
    @required this.onSelect,
    @required this.cachedColorDots,
    @required this.year,
    @required this.month,
    @required this.colorMap,
    @required this.times,
  }) : super(key: key);

  @override
  _MonthWidgetState createState() => _MonthWidgetState();
}

class _MonthWidgetState extends State<MonthWidget> {
  static double monthHeight = 320.0;
  static const weekdays = ['M', 'T', 'O', 'T', 'F', 'L', 'S'];

  @override
  Widget build(BuildContext context) {
    return _buildMonthAsyncV2(context, widget.year);
  }

  Widget _buildMonthAsyncV2(BuildContext context, int year) {
    final firstDate = DateTime(year, widget.month);
    final daysInMonth = dtOps.daysInMonth(firstDate);
    final lastDate = DateTime(year, widget.month, daysInMonth);

    var startDate = firstDate.add(Duration(days: -(firstDate.weekday - 1)));
    final endDate = lastDate.add(Duration(days: 7 - lastDate.weekday));

    final firstWeekNr = dtOps.weekOfYear(firstDate);

    var rowNum = 0;
    List<Widget> rows = List();
    List<TableRow> tableRows = List();
    while (startDate.compareTo(endDate) != 1) {
      rows.add(startDate.month == widget.month
          ? buildDayV3(startDate, context, widget.colorMap[startDate.day] ?? [],
              widget.times)
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
      // color: Colors.pink,
      height: monthHeight,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: <Widget>[
          Text(
            '${dtOps.dateTimeShortMonthName(firstDate)} $year',
            textAlign: TextAlign.start,
            style:
                TextStyle(color: widget.primaryColorAlpha100, fontSize: 60.0),
          ),
          // weekdayRow,
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
                        style: TextStyle(color: widget.primaryColorAlpha100),
                      )))
                  .toList(),
            ),
          ),
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              // weekNumberColumn,
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
                          style: TextStyle(color: widget.primaryColorAlpha100),
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
    bool sameDay = dtOps.isSameDate(date, widget.today);
    bool active = widget.dateActive != null ? widget.dateActive(date) : true;

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
            ? isInSelectedRange ? widget.selectedTodayBox : widget.todayBox
            : isInSelectedRange ? widget.selectedBox : widget.plainBox,
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
                          return widget.cachedColorDots[c];
                        })),
                    ),
                  )
                : const SizedBox(height: 9),
          ],
        ),
      ),
      onTap: () => active ? widget.onSelect(date) : null,
    );
  }
}

class DayBuilder {
  final DateTime date;
  final Widget Function(DateTime, BuildContext) builderFunction;

  DayBuilder({
    @required this.date,
    @required this.builderFunction,
  });
  Widget builder(BuildContext context) => builderFunction(date, context);
}
