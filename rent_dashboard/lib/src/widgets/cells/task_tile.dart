import 'package:flutter/material.dart';
import '../../models/task.dart';
import '../../models/workstatus.dart';
import '../../models/enums.dart';

class TaskTile extends StatelessWidget {
  final Task task;
  TaskTile(this.task);
  @override
  Widget build(BuildContext context) {
    return Card(
      child: Container(
        margin: EdgeInsets.all(8.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            Padding(
              padding: const EdgeInsets.only(top: 8.0, bottom: 8.0),
              child: Row(
                children: <Widget>[
                  Padding(
                    padding: const EdgeInsets.only(right: 8.0),
                    child: Icon(
                      task.planType == PlanType.Vinduer
                          ? Icons.panorama_wide_angle
                          : task.planType == PlanType.Vinduer
                              ? Icons.cached
                              : Icons.av_timer,
                      color: Color(0xFF888888),
                    ),
                  ),
                  Text(
                    '${task.plan} ${task.floor != null ? task.floor + ' ' : ''}',
                    style: TextStyle(fontSize: 20.0, color: Colors.blue[700]),
                  ),
                ],
              ),
            ),
            Text('${task.area}'),
            Row(
              children: [
                Expanded(
                  child: task.nextTime != null
                      ? Text(
                          'Next cleaned: ${task.nextTime.day}/${task.nextTime.month}')
                      : Text('Not cleaned yet'),
                ),
                getStatusIcon(task.status()),
              ],
            ),
            Text('${task.customerName}'),
            Text('${task.locationName}'),
          ],
        ),
      ),
    );
  }
}
