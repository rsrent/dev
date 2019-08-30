import 'package:flutter/material.dart';
import '../models/data.dart';
import '../models/count_with_workstatus.dart';
import '../models/workstatus.dart';

class SimpleWorkView extends StatelessWidget {
  static const double spaceBetween = 3.0;
  SimpleWorkView.fromMap(this.workDoneSets);
  SimpleWorkView.fromTaskSet(CountWithWorkStatusSet taskSet) {
    workDoneSets = {Icons.adjust: taskSet};
  }
  SimpleWorkView.fromReportSet(CountWithWorkStatusSet reportSet) {
    workDoneSets = {Icons.assignment: reportSet};
  }
  SimpleWorkView.fromSmallData(SimpleData smallData) {
    workDoneSets = {
      Icons.assignment: smallData.reports,
      Icons.adjust: smallData.tasks
    };
  }

  Map<IconData, CountWithWorkStatusSet> workDoneSets;

  @override
  Widget build(BuildContext context) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: <Widget>[]..addAll(getRows()),
    );
  }

  List<Widget> getRows() {
    List<Widget> widgets = List();
    workDoneSets.forEach((k, v) {
      widgets.add(buildRow(k, v));
    });
    return widgets;
  }

  Widget buildRow(iconData, workStatusSet) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: <Widget>[
        Icon(
          iconData,
          color: Colors.blue,
          size: 18.0,
        ),
        Text(
          '${workStatusSet.getAmount(WorkStatus.Overdue)}',
          style: TextStyle(color: getWorkStatusTextColor(WorkStatus.Overdue)),
        ),
        Text(
          '${workStatusSet.getAmount(WorkStatus.Critical)}',
          style: TextStyle(color: getWorkStatusTextColor(WorkStatus.Critical)),
        ),
        Text(
          '${workStatusSet.getAmount(WorkStatus.Okay)}',
          style: TextStyle(color: getWorkStatusTextColor(WorkStatus.Okay)),
        ),
        Text(
          '${workStatusSet.getAmount(WorkStatus.Unstarted)}',
          style: TextStyle(color: getWorkStatusTextColor(WorkStatus.Unstarted)),
        ),
      ],
    );
  }
}

Widget verticalSeperator() {
  return Column(
    children: <Widget>[
      Text('/'),
      horizontalSeperator(),
      Text('/'),
    ],
  );
}

Widget horizontalSeperator() {
  return Container(padding: const EdgeInsets.all(3.0), child: Center());
}
