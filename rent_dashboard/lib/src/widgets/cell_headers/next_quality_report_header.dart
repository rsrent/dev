import 'package:flutter/material.dart';
import '../../models/enums.dart';
import '../../models/workstatus.dart';
import 'package:dart_packages/date_time_operations.dart' as dateTimeOps;

class NextQualityReport extends StatelessWidget {
  final DateTime next;
  final DateTime planned;
  NextQualityReport(this.next, this.planned);

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.only(top: 20.0, left: 20.0, right: 20.0, bottom: 0.0),
      //color: getWorkStatusBackgroundColor(status(next)),
      child: next == null
          ? Row(
              children: <Widget>[
                Expanded(child: Text('Ingen kvalitetsreporter')),
                getStatusIcon(WorkStatus.Unstarted)
              ],
            )
          : Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                Row(
                  children: <Widget>[
                    Expanded(
                      child: Text(
                        'Næste kvalitetsreport: ${next.day}/${next.month} om ${next.difference(DateTime.now()).inDays} dage',
                        style: TextStyle(fontSize: 20.0),
                        maxLines: 2,
                      ),
                    ),
                    getStatusIcon(status(next)),
                  ],
                ),
                Divider(color: Colors.transparent),
                planned != null
                    ? Text(
                        'Aftalt til den: ${dateTimeOps.toDDMM(planned)} kl ${dateTimeOps.toHHmm(planned)} om ${planned.difference(DateTime.now()).inDays} dage',
                        textAlign: TextAlign.start,
                        style: TextStyle(
                          color: Theme.of(context).primaryColor,
                        ),
                      )
                    : Text(''),
                Divider(),
              ],
            ),
    );
  }
}
