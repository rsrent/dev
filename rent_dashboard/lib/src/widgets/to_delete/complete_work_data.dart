import 'package:flutter/material.dart';
import '../../generic/date_time.dart';
import '../models/data.dart';
import '../models/workstatus.dart';
import '../models/count_with_workstatus.dart';

class CompleteWorkView extends StatelessWidget {
  static const double spaceBetween = 3.0;
  final fontSize = 16.0;
  final CompleteData data;
  CompleteWorkView(this.data);
  @override
  Widget build(BuildContext context) {
    return Column(
      children: <Widget>[
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            SizedBox(
              width: 100.0,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: <Widget>[
                  SizedBox(
                    height: 40.0,
                  ),
                  textIcon(Icons.assignment, 'Reporter'),
                  textIcon(Icons.filter_tilt_shift, 'Opgaver'),
                  textIcon(Icons.panorama_wide_angle, 'Vinduer',
                      color: Color(0xFF999999)),
                  textIcon(Icons.cached, 'Fan Coil', color: Color(0xFF999999)),
                  textIcon(Icons.av_timer, 'Periodisk',
                      color: Color(0xFF999999)),
                ],
              ),
            ),
            column(data, WorkStatus.Overdue),
            column(data, WorkStatus.Critical),
            column(data, WorkStatus.Okay),
            column(data, WorkStatus.Unstarted),
          ],
        ),
      ],
    );
  }

  Widget column(CompleteData data, WorkStatus status) {
    var style1 = TextStyle(fontSize: fontSize);
    var style2 = TextStyle(fontSize: fontSize, color: Color(0xFF999999));
    return Column(
      children: <Widget>[
        getStatusIcon(status),
        Text(
          '${data.qualityReportStatus.getAmount(status)}',
          style: style1,
        ),
        horizontalSeperator(),
        Text(
          '${data.allTasks.getAmount(status)}',
          style: style1,
        ),
        horizontalSeperator(),
        Text(
          '${data.windowTasks.getAmount(status)}',
          style: style2,
        ),
        horizontalSeperator(),
        Text(
          '${data.fanCoilTasks.getAmount(status)}',
          style: style2,
        ),
        horizontalSeperator(),
        Text(
          '${data.periodicTasks.getAmount(status)}',
          style: style2,
        ),
      ],
    );
  }

  Widget textIcon(icon, text, {color}) {
    var style = TextStyle(
        fontSize: fontSize, color: color != null ? color : Colors.black);
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: <Widget>[
        Icon(
          icon,
          color: color != null ? color : Colors.blue,
        ),
        Text(
          text,
          style: style,
        ),
      ],
    );
  }

  Widget horizontalSeperator() {
    return Container(
        padding: const EdgeInsets.all(spaceBetween), child: Center());
  }
}