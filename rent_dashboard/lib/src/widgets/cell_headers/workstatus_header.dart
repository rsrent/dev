import 'package:flutter/material.dart';
import '../../models/count_with_workstatus.dart';

class WorkStatusHeader extends StatelessWidget {
  final CountWithWorkStatusSet workStatusSet;
  final int total;
  WorkStatusHeader(this.workStatusSet)
      : total = (workStatusSet.delayed.occurences) +
            (workStatusSet.critical.occurences) +
            (workStatusSet.okay.occurences) +
            (workStatusSet.unstarted.occurences);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 50.0,
      child: total > 0
          ? Row(
              children: [
                _buildBarSegment(workStatusSet.delayed.occurences, Colors.red),
                _buildBarSegment(
                    workStatusSet.critical.occurences, Colors.yellow),
                _buildBarSegment(workStatusSet.okay.occurences, Colors.green),
                _buildBarSegment(
                    workStatusSet.unstarted.occurences, Colors.grey),
              ],
            )
          : Center(child: Text('Ingen data', style: TextStyle(fontSize: 20.0))),
    );
  }

  Widget _buildBarSegment(int number, Color c) {
    return Flexible(
      child: Container(
        color: c,
        child: Center(
          child: number > 0
              ? Text(
                  '$number',
                  style: TextStyle(
                    color: Colors.white,
                  ),
                )
              : null,
        ),
      ),
      flex: (number / total * 1000).round(),
    );
  }
}
