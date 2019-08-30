import 'package:flutter/material.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;
import 'dart:async';
//import '../../style.dart' as style;
import 'package:rxdart/rxdart.dart';

class CalendarWidget extends StatelessWidget {
  final bool selectTimeSpan;

  bool updatedFirstTime = true;
  final Function(DateTime) dateSelected;
  //final Future<Iterable<Color>> Function(DateTime) dayColor;
  final Stream<Map<int, Iterable<Color>>> Function(int, int) monthColors;
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

  CalendarWidget(
      {Key key,
      this.timeSpanStream,
      this.selectTimeSpan = false,
      this.dateSelected,
      this.monthColors,
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

        if (monthColors != null) {
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

  _buildMonthFromStream(BuildContext context, int year, int month) {
    Stream<Map<int, Iterable<Color>>> stream = monthColors(year, month);

    return StreamBuilder(
      stream: stream,
      builder: (context, AsyncSnapshot<Map<int, Iterable<Color>>> snapshot) {
        if (snapshot.hasData) {
          return _buildMonthTimeSpan(context, year, month, snapshot.data);
          // if (timeSpanStream != null) {
          //   return StreamBuilder(
          //     stream: timeSpanStream.stream,
          //     builder: (BuildContext context,
          //         AsyncSnapshot<List<DateTime>> timesSnapshot) {
          //       var times = timesSnapshot.data ?? [];

          //       return _buildMonthTimeSpan(
          //           context, year, month, snapshot.data);
          //     },
          //   );
          // } else {
          //   return _buildMonthTimeSpan(context, year, month, snapshot.data);
          // }

          // return _buildMonth(context, year, month, (date) async {
          //   if (snapshot.data.containsKey(date.day)) {
          //     return snapshot.data[date.day];
          //   } else {
          //     return [];
          //   }
          // });
        } else {
          return Center(child: CircularProgressIndicator());
        }
      },
    );
  }

  // _buildMonth(BuildContext context, int year, int month,
  //     Future<Iterable<Color>> Function(DateTime) getColors) {
  //   return _buildMonthTimeSpan(context, year, month, {});
  //   // return FutureBuilder(
  //   //   future: _buildMonthAsync(context, year, month, getColors),
  //   //   builder: (context, future) {
  //   //     if (future.hasData) {
  //   //       return future.data;
  //   //     } else {
  //   //       return Container();
  //   //     }
  //   //   },
  //   // );
  // }

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

  // Future<Widget> _buildMonthAsync(BuildContext context, int year, int month,
  //     Future<Iterable<Color>> Function(DateTime) getColors) async {
  //   //return _buildMonthAsyncV2(context, year, month, getColors);

  //   return Future<Widget>.delayed(Duration(milliseconds: 1), () {
  //     var firstDate = DateTime(year, month);
  //     var daysInMonth = dtOps.daysInMonth(firstDate);
  //     var lastDate = DateTime(year, month, daysInMonth);

  //     var startDate = firstDate.add(Duration(days: -(firstDate.weekday - 1)));
  //     var endDate = lastDate.add(Duration(days: 7 - lastDate.weekday));

  //     var counterDate = startDate;
  //     var days = List<Widget>();
  //     var weeks = List<Widget>();

  //     while (counterDate.compareTo(endDate) != 1) {
  //       days.add(_buildDay(counterDate, month, getColors));

  //       counterDate = counterDate.add(Duration(days: 1));
  //       if (counterDate.hour == 23)
  //         counterDate = counterDate.add(Duration(hours: 1));
  //       if (counterDate.hour == 1)
  //         counterDate = counterDate.add(Duration(hours: -1));

  //       if (counterDate.weekday == 1) {
  //         days.insert(
  //             0,
  //             SizedBox(
  //                 width: 20.0,
  //                 child: Text(
  //                   '${dtOps.weekOfYear(counterDate) - 1}',
  //                   style: TextStyle(
  //                       color: Theme.of(context).primaryColor.withAlpha(100)),
  //                 )));

  //         weeks.add(Row(
  //           mainAxisAlignment: MainAxisAlignment.spaceEvenly,
  //           children: days,
  //         ));
  //         days = List<Widget>();
  //       }
  //     }

  //     return Container(
  //       height: monthHeight,
  //       child: Column(
  //         crossAxisAlignment: CrossAxisAlignment.stretch,
  //         children: <Widget>[
  //           Text(
  //             '${dtOps.dateTimeShortMonthName(firstDate)} $year',
  //             textAlign: TextAlign.start,
  //             style: TextStyle(
  //                 color: Theme.of(context).primaryColor.withAlpha(100),
  //                 fontSize: 60.0),
  //           ),
  //           Container(
  //             margin: EdgeInsets.only(left: 40, right: 8),
  //             child: Row(
  //               mainAxisAlignment: MainAxisAlignment.spaceBetween,
  //               children: weekdays
  //                   .map<Widget>((d) => SizedBox(
  //                       width: 20.0,
  //                       height: 20.0,
  //                       child: Text(
  //                         d,
  //                         style: TextStyle(
  //                             color: Theme.of(context)
  //                                 .primaryColor
  //                                 .withAlpha(100)),
  //                       )))
  //                   .toList(),
  //             ),
  //           ),
  //           Column(
  //             children: weeks,
  //           ),
  //         ],
  //       ),
  //     );
  //   });
  // }

  // Widget _buildDay(DateTime date, int month, getColors) {
  //   if (date.month != month) {
  //     return Container(
  //       width: 46.0,
  //       height: 36.0,
  //     );
  //   }

  //   return FutureBuilder(
  //     future: getColors(date),
  //     builder: (context, snapshot) {
  //       Iterable<Color> specialColors = snapshot.data ?? List<Color>();

  //       if (timeSpanStream != null) {
  //         return StreamBuilder(
  //           stream: timeSpanStream.stream,
  //           builder: (BuildContext context,
  //               AsyncSnapshot<List<DateTime>> timesSnapshot) {
  //             var times = timesSnapshot.data ?? [];

  //             return _buildDayV2(context, date, specialColors, times);
  //           },
  //         );
  //       } else {
  //         return _buildDayV2(context, date, specialColors, []);
  //       }
  //     },
  //   );
  // }

  // _buildDayV2(BuildContext context, DateTime date,
  //     Iterable<Color> specialColors, List<DateTime> times) {
  //   bool sameDay = dtOps.isSameDate(date, today);
  //   bool active = dateActive != null ? dateActive(date) : true;
  //   /*
  //       var textColor = specialColors != null
  //           ? specialColors
  //           : sameDay ? themeData.primaryContrastColor : themeData.primaryColor; */

  //   var btn = SizedBox(
  //     width: 46.0,
  //     height: 36.0,
  //     child: FlatButton(
  //       onPressed: active
  //           ? () {
  //               if (selectTimeSpan) {
  //                 if (times.length == 0) {
  //                   times.add(date);
  //                   //times.add(date);
  //                 } else if (times.length == 1) {
  //                   if (dtOps.isSameDate(date, times[0])) {
  //                     print('DO NOTHING');
  //                   } else if (date.compareTo(times[0]) < 0) {
  //                     times.insert(0, date);
  //                     updatedFirstTime = true;
  //                   } else {
  //                     times.add(date);
  //                     updatedFirstTime = false;
  //                   }
  //                 } else if (times.length == 2) {
  //                   if (dtOps.isSameDate(date, times[0])) {
  //                     times[1] = date;
  //                   } else if (dtOps.isSameDate(date, times[1])) {
  //                     times[0] = date;
  //                   } else if (date.compareTo(times[0]) < 0) {
  //                     times[0] = date;
  //                     updatedFirstTime = true;
  //                   } else if (times[1].compareTo(date) < 0) {
  //                     times[1] = date;
  //                     updatedFirstTime = false;
  //                   } else {
  //                     if (updatedFirstTime) {
  //                       times[1] = date;
  //                       updatedFirstTime = false;
  //                     } else {
  //                       times[0] = date;
  //                       updatedFirstTime = true;
  //                     }
  //                   }
  //                 }
  //                 timeSpanStream.add(times);

  //                 print(times);
  //               } else {
  //                 dateSelected(date);
  //               }
  //             }
  //           : null,
  //       child: Container(),
  //       color: Colors.transparent,
  //     ),
  //   );

  //   var isInSelectedRange =
  //       (times.length == 1 && dtOps.isSameDate(times[0], date)) ||
  //           times.length == 2 &&
  //               times[0].compareTo(date) < 1 &&
  //               date.compareTo(times[1]) < 1;

  //   return Stack(
  //     alignment: Alignment.center,
  //     children: [
  //       !active
  //           ? genericInActiveContainer
  //           : !sameDay && !isInSelectedRange
  //               ? genericContainer
  //               : Container(
  //                   margin: EdgeInsets.fromLTRB(2, 2, 2, 2),
  //                   width: 32.0,
  //                   height: 32.0,
  //                   decoration: BoxDecoration(
  //                     boxShadow: null,
  //                     color: sameDay
  //                         ? Theme.of(context).primaryColor.withAlpha(50)
  //                         : Colors.white,
  //                     borderRadius:
  //                         BorderRadius.all(const Radius.circular(16.0)),
  //                     border: isInSelectedRange
  //                         ? Border.all(
  //                             color: Theme.of(context).primaryColor,
  //                             width: 2.0,
  //                             style: BorderStyle.solid)
  //                         : null,
  //                   ),
  //                 ),
  //       Positioned(
  //         bottom: 6.0,
  //         child: Row(
  //           mainAxisAlignment: MainAxisAlignment.center,
  //           children: []..addAll(specialColors.map((c) {
  //               return Container(
  //                   padding: EdgeInsets.only(left: 2.0),
  //                   width: 6.0,
  //                   height: 6.0,
  //                   decoration:
  //                       BoxDecoration(shape: BoxShape.circle, color: c));
  //             })),
  //         ),
  //       ),
  //       Text(
  //         '${date.day}',
  //         style: const TextStyle(fontSize: 10),
  //         textAlign: TextAlign.center,
  //       ),
  //       btn
  //     ],
  //   );
  // }

  Widget _buildMonthAsyncV2(BuildContext context, int year, int month,
      Map<int, Iterable<Color>> colorMap, List<DateTime> times) {
    var firstDate = DateTime(year, month);
    var daysInMonth = dtOps.daysInMonth(firstDate);
    var lastDate = DateTime(year, month, daysInMonth);

    var startDate = firstDate.add(Duration(days: -(firstDate.weekday - 1)));
    var endDate = lastDate.add(Duration(days: 7 - lastDate.weekday));

    var firstWeekNr = dtOps.weekOfYear(firstDate);

    var counterDate = startDate;
    var rowNum = 0;
    List<List<Widget>> rows = List();
    List<TableRow> tableRows = List();
    while (counterDate.compareTo(endDate) != 1) {
      if (rowNum >= rows.length) {
        rows.add(List());
      }

      // rows[rowNum].add(Container(
      //   width: 30,
      //   height: 40,
      //   child: buildDayV3(counterDate, context, colorMap[counterDate] ?? []),
      // ));

      rows[rowNum].add(counterDate.month == month
          ? buildDayV3(
              counterDate, context, colorMap[counterDate.day] ?? [], times)
          : Container());

      counterDate = counterDate.add(Duration(days: 1));
      if (counterDate.hour == 23)
        counterDate = counterDate.add(Duration(hours: 1));
      if (counterDate.hour == 1)
        counterDate = counterDate.add(Duration(hours: -1));
      //counterDate = counterDate.add(Duration(days: 1));
      if (counterDate.weekday == 1) {
        print(rows[rowNum].length);
        tableRows.add(TableRow(children: rows[rowNum]));
        rowNum++;
      }
    }

    // var firstTableDate = firstDate.subtract(Duration(
    //     days: firstDate.weekday != 1 ? (7 - (firstDate.weekday - 1)) : 0));

    // var tableDateOffset = firstDate.weekday != 1 ? daysInFirstWeek : 0;

    return Container(
      height: monthHeight,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: <Widget>[
          Text(
            '${dtOps.dateTimeShortMonthName(firstDate)} $year',
            textAlign: TextAlign.start,
            style: TextStyle(
                color: Theme.of(context).primaryColor.withAlpha(100),
                fontSize: 60.0),
          ),
          Container(
            margin: EdgeInsets.only(left: 40, right: 8),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: weekdays
                  .map<Widget>((d) => SizedBox(
                      width: 20.0,
                      height: 20.0,
                      child: Text(
                        d,
                        style: TextStyle(
                            color:
                                Theme.of(context).primaryColor.withAlpha(100)),
                      )))
                  .toList(),
            ),
          ),
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              Container(
                width: 20,
                child: Column(
                  children: List.generate(
                      rowNum,
                      (i) => SizedBox(
                            height: 38,
                            child: Center(child: Text('${firstWeekNr + i}')),
                          )),
                ),
              ),
              Expanded(
                child: Container(
                  height: 200,
                  child: Table(
                    columnWidths: {
                      0: FixedColumnWidth(30.0),
                      1: FixedColumnWidth(30.0),
                      2: FixedColumnWidth(30.0),
                      3: FixedColumnWidth(30.0),
                      4: FixedColumnWidth(30.0),
                      5: FixedColumnWidth(30.0),
                      6: FixedColumnWidth(30.0),
                    },
                    children: tableRows,
                    // children: List.generate(weeks, (w) {
                    //   if (w == 0) {
                    //     return buildRow(
                    //         daysInFirstWeek < 7 ? daysInFirstWeek : 0,
                    //         7,
                    //         firstTableDate,
                    //         context,
                    //         snapshot.connectionState == ConnectionState.done
                    //             ? ''
                    //             : null);
                    //   } else if (w == weeks - 1) {
                    //     return buildRow(
                    //         0,
                    //         daysInLastWeek,
                    //         firstDate.add(Duration(days: (w) * 7 - tableDateOffset)),
                    //         context,
                    //         snapshot.connectionState == ConnectionState.done
                    //             ? ''
                    //             : null);
                    //   } else {
                    //     return buildRow(
                    //         0,
                    //         7,
                    //         firstDate.add(Duration(days: (w) * 7 - tableDateOffset)),
                    //         context,
                    //         snapshot.connectionState == ConnectionState.done
                    //             ? ''
                    //             : null);
                    //   }
                    // }),
                  ),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  // TableRow buildRow(int firstDayOfWeek, int lastDayOfWeek,
  //     DateTime firstDateInRow, BuildContext context, colorMap) {
  //   print('row');
  //   return TableRow(
  //       children: List.generate(7, (i) {
  //     if (firstDayOfWeek <= i && i <= lastDayOfWeek) {
  //       var date = firstDateInRow.add(Duration(days: i));
  //       return _buildDayFuture(
  //           date, context, colorMap != null ? [Colors.green] : []);
  //       return Container(
  //         width: 30,
  //         height: 40,
  //         child: _buildDayFuture(
  //             date, context, colorMap != null ? [Colors.green] : []),
  //         // child: _buildDay(
  //         //   DateTime.now(),
  //         //   3,
  //         //   Future<Iterable<Color>>.delayed(Duration.zero, () => []),
  //         // ),
  //       );
  //     } else {
  //       return Container();
  //     }
  //   }));
  // }

  // Widget _buildDayFuture(
  //     DateTime date, BuildContext context, Iterable<Color> colors) {
  //   return buildDayV3(date, context, colors, []);
  //   // if (getColors == null) {}
  //   // return FutureBuilder(
  //   //   future: getColors(date),
  //   //   builder: (context, snapshot) {
  //   //     Iterable<Color> specialColors = snapshot.data ?? List<Color>();
  //   //     return buildDayV3(date, context, specialColors);
  //   //   },
  //   // );
  // }

  Widget buildDayV3(DateTime date, BuildContext context, Iterable<Color> colors,
      List<DateTime> times) {
    bool sameDay = dtOps.isSameDate(date, today);
    bool active = dateActive != null ? dateActive(date) : true;

    var isInSelectedRange =
        (times.length == 1 && dtOps.isSameDate(times[0], date)) ||
            times.length == 2 &&
                times[0].compareTo(date) < 1 &&
                date.compareTo(times[1]) < 1;

    return GestureDetector(
      child: Container(
        margin: const EdgeInsets.fromLTRB(2, 2, 2, 2),
        width: 34.0,
        height: 34.0,
        decoration: BoxDecoration(
          boxShadow: null,
          color: sameDay
              ? Theme.of(context).primaryColor.withAlpha(50)
              : Colors.white,
          shape: BoxShape.circle,
          border: isInSelectedRange
              ? Border.all(
                  color: Theme.of(context).primaryColor,
                  width: 2.0,
                  style: BorderStyle.solid)
              : null,
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            SizedBox(
              height: 9,
            ),
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
                : SizedBox(height: 9),
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

    // return GestureDetector(
    //   child: Stack(
    //     alignment: Alignment.center,
    //     children: [
    //       !active
    //           ? genericInActiveContainer
    //           : !sameDay && !isInSelectedRange
    //               ? genericContainer
    //               : Container(
    //                   margin: EdgeInsets.fromLTRB(2, 2, 2, 2),
    //                   width: 32.0,
    //                   height: 32.0,
    //                   decoration: BoxDecoration(
    //                     boxShadow: null,
    //                     color: sameDay
    //                         ? Theme.of(context).primaryColor.withAlpha(50)
    //                         : Colors.white,
    //                     shape: BoxShape.circle,
    //                     border: isInSelectedRange
    //                         ? Border.all(
    //                             color: Theme.of(context).primaryColor,
    //                             width: 2.0,
    //                             style: BorderStyle.solid)
    //                         : null,
    //                   ),
    //                 ),
    //       Positioned(
    //         bottom: 6.0,
    //         child: Row(
    //           mainAxisAlignment: MainAxisAlignment.center,
    //           children: []..addAll(colors.map((c) {
    //               return Container(
    //                   padding: EdgeInsets.only(left: 2.0),
    //                   width: 6.0,
    //                   height: 6.0,
    //                   decoration:
    //                       BoxDecoration(shape: BoxShape.circle, color: c));
    //             })),
    //         ),
    //       ),
    //       Text(
    //         '${date.day}',
    //         style: const TextStyle(fontSize: 10),
    //         textAlign: TextAlign.center,
    //       ),
    //     ],
    //   ),
    //   onTap: active
    //       ? () {
    //           if (selectTimeSpan) {
    //             if (times.length == 0) {
    //               times.add(date);
    //               //times.add(date);
    //             } else if (times.length == 1) {
    //               if (dtOps.isSameDate(date, times[0])) {
    //                 print('DO NOTHING');
    //               } else if (date.compareTo(times[0]) < 0) {
    //                 times.insert(0, date);
    //                 updatedFirstTime = true;
    //               } else {
    //                 times.add(date);
    //                 updatedFirstTime = false;
    //               }
    //             } else if (times.length == 2) {
    //               if (dtOps.isSameDate(date, times[0])) {
    //                 times[1] = date;
    //               } else if (dtOps.isSameDate(date, times[1])) {
    //                 times[0] = date;
    //               } else if (date.compareTo(times[0]) < 0) {
    //                 times[0] = date;
    //                 updatedFirstTime = true;
    //               } else if (times[1].compareTo(date) < 0) {
    //                 times[1] = date;
    //                 updatedFirstTime = false;
    //               } else {
    //                 if (updatedFirstTime) {
    //                   times[1] = date;
    //                   updatedFirstTime = false;
    //                 } else {
    //                   times[0] = date;
    //                   updatedFirstTime = true;
    //                 }
    //               }
    //             }
    //             timeSpanStream.add(times);

    //             print(times);
    //           } else {
    //             dateSelected(date);
    //           }
    //         }
    //       : null,
    // );
  }
}
