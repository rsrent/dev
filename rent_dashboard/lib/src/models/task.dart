import 'package:flutter/material.dart';
import 'enums.dart';
import 'workstatus.dart';

class Task {
  DateTime get nextTime =>
      lastCleaned != null ? lastCleaned.add(Duration(days: (365.0 / timesOfYear).round() )) : null; 
  final PlanType planType;
  final String plan;
  final String area;
  final String floor;
  final String comment;
  final DateTime lastCleaned;
  final int timesOfYear;
  final String customerName;
  final String locationName;
  //final int latestQualityReportRating;

  //WorkStatus status;

  Task.fromJson(json)
      : lastCleaned = json['lastCleaned'] != null
            ? DateTime.parse(json['lastCleaned'])
            : null,
        //nextTime = json['lastCleaned'] != null ? lastCleaned.add(Duration(days: json['timesOfYear'])) : null,
        timesOfYear = json['timesOfYear'],
        planType = PlanType.values[json['planId'] - 1],
        plan = json['plan'],
        area = json['area'],
        floor = json['floor'],
        comment = json['comment'],
        customerName = json['customerName'],
        locationName = json['locationName'];

  /*
  bool included(Filters filter) {
    var _include = true;
    if (!filter.statusSet[status(filter)] ||
        !filter.planTypes.contains(planType) ||
        (nextTime != null && nextTime.compareTo(filter.maxDate) == 1))
      _include = false;
    return _include;
  }
  */

  WorkStatus status() {
    if (nextTime == null) return WorkStatus.Unstarted;
    if (nextTime.compareTo(DateTime.now().add(Duration(days: -1))) == -1)
      return WorkStatus.Overdue;
    if (nextTime.compareTo(DateTime.now().add(Duration(days: 1))) == -1)
      return WorkStatus.Critical;
    return WorkStatus.Okay;
  }
}

class TaskCell extends StatelessWidget {
  final Task task;
  TaskCell(this.task);
  @override
  Widget build(BuildContext context) {
    return Container(
      color: getWorkStatusBackgroundColor(task.status()),
      padding: const EdgeInsets.only(left: 8.0, right: 8.0),
      child: Column(
        children: <Widget>[
          Text('${task.plan} ${task.floor != null ? task.floor + ' ' : ''}'),
          Text('${task.area}'),
          Row(
            children: <Widget>[
              task.nextTime != null
                  ? Text(
                      'Next cleaned: ${task.nextTime.day}/${task.nextTime.month}')
                  : Text('Not cleaned yet')
            ],
          ),
        ],
      ),
    );
  }
}

/*
class TaskOverview extends StatelessWidget {
  final List<Task> tasks;
  final Filters filter;
  TaskOverview(this.tasks, this.filter);

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

  getTasksToCome() =>
      tasks.where((t) => t.status(filter) == WorkStatus.Okay).length;
  getTasksCritical() =>
      tasks.where((t) => t.status(filter) == WorkStatus.Critical).length;
  getTasksDelayed() =>
      tasks.where((t) => t.status(filter) == WorkStatus.Overdue).length;
  getTasksUnstarted() =>
      tasks.where((t) => t.status(filter) == WorkStatus.Unstarted).length; 
}*/
