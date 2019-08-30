import 'package:flutter/material.dart';
import '../models/data.dart';
import '../models/workdone.dart';

class RecentData extends StatelessWidget {
  Map<String,WorkDoneSet> workDoneSets;

  RecentData.fromMap(this.workDoneSets);
  RecentData.fromData(CompleteData data) {
    workDoneSets = {"igår":data.yesterday, "idag":data.today};
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: <Widget>[
        Padding(
          padding: const EdgeInsets.only(bottom: 8.0),
          child: Text(
            'Aktivitet',
            style: TextStyle(fontSize: 24.0),
          ),
        ),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                Text(''),
                Text('Kvalitetsrepoter'),
                Text('Logs'),
                Text('Vinder'),
                Text('Fan Coil'),
                Text('Periodisk'),
              ],
            ),
          ]..addAll(sets()),
        ),
      ],
    );
  }

  List<Widget> sets()
  {
    List<Widget> widgets = List();
    workDoneSets.forEach((k,v) {
      widgets.add(workDoneSetColumn(k,v));
    });
    return widgets;
  }

  Widget workDoneSetColumn(String s, WorkDoneSet doneSet) {
    return Column(
      children: <Widget>[
        Text(s),
        Text('${doneSet.reports}'),
        Text('${doneSet.logs}'),
        Text('${doneSet.windows}'),
        Text('${doneSet.fancoils}'),
        Text('${doneSet.periodics}'),
      ],
    );
  }
}