import 'package:flutter/material.dart';
//import '../filter.dart';
//import '../enums.dart';
import './data.dart';

class Report {
  int id;
  bool completed;
  DateTime time;

  DateTime nextTime;

  bool instanciated = true;

  Report({json}) {
    if (json != null) {
      completed = json['completed'];
      time = DateTime.parse(json['time']);
      nextTime = time.add(Duration(
          days: (365.0 ~/ (json['interval'] != 0 ? json['interval'] : 1))));
    } else {
      instanciated = false;
    }

    //status = qualityReportStatus(this);
  }
/*
  bool included(Filters filter) {
    var _include = true;
    if (!filter.statusSet[status()] ||
        (nextTime != null && nextTime.compareTo(filter.maxDate) == 1))
      _include = false;
    return _include;
  } */
/*
  static compareListTo(ts1, ts2) {
    return ts2
        .where((t) => t.status == WorkStatus.Overdue)
        .length
        .compareTo(ts1.where((t) => t.status == WorkStatus.Overdue).length);
  }
  */

  
}




/*
class QualityReportOverview extends StatelessWidget {
  final List<Report> reports;
  QualityReportOverview(this.reports);

  @override
  Widget build(BuildContext context) {
    return Row(
      children: <Widget>[
        Text(
          '${getTasksDelayed()}',
          style: TextStyle(color: Colors.red),
        ),
        Text(' / '),
        Text(
          '${getTasksCritical()}',
          style: TextStyle(color: Colors.yellow[800]),
        ),
        Text(' / '),
        Text(
          '${getTasksToCome()}',
          style: TextStyle(color: Colors.green),
        ),
        Text(' / '),
        Text(
          '${getTasksUnstarted()}',
          style: TextStyle(color: Colors.grey),
        ),
      ],
    );
  }

  getTasksToCome() => reports.where((t) => t.status == WorkStatus.Okay).length;
  getTasksCritical() =>
      reports.where((t) => t.status == WorkStatus.Critical).length;
  getTasksDelayed() =>
      reports.where((t) => t.status == WorkStatus.Overdue).length;
  getTasksUnstarted() =>
      reports.where((t) => t.status == WorkStatus.Unstarted).length;
}
*/


