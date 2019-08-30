import 'package:flutter/material.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;

class WeekTile extends StatelessWidget {
  final String title;
  List<int> _startTs = List<int>(7);
  List<int> _endTs = List<int>(7);
  final List<String> days = ['M', 'T', 'O', 'T', 'F', 'L', 'S'];
  WeekTile(this.title, List<int> starttimes, List<int> durations) {
    for (int i = 0; i < 7; i++) {
      if (starttimes[i] + durations[i] == 0) {
        _startTs[i] = null;
        _endTs[i] = null;
      } else {
        _startTs[i] = starttimes[i];
        _endTs[i] = durations[i];
      }
    }
  }
  @override
  Widget build(BuildContext context) {
    return ListTile(
      title: title != null
          ? Padding(
              padding: const EdgeInsets.only(bottom: 8),
              child: Text(title),
            )
          : null,
      subtitle: Table(
        columnWidths: {
          0: FractionColumnWidth(1.0 / 7.0),
          1: FractionColumnWidth(1.0 / 7.0),
          2: FractionColumnWidth(1.0 / 7.0),
          3: FractionColumnWidth(1.0 / 7.0),
          4: FractionColumnWidth(1.0 / 7.0),
          5: FractionColumnWidth(1.0 / 7.0),
          6: FractionColumnWidth(1.0 / 7.0),
        },
        children: [
          TableRow(
            children: List<Widget>.of(days.map((d) => _buildText(d))),
          ),
          TableRow(
            children: List<Widget>.of(_startTs
                .map((h) => _buildText(h != null ? dtOps.minsToHHmm(h) : ''))),
          ),
          TableRow(
            children: List<Widget>.of(days.map((d) => _buildText('-'))),
          ),
          TableRow(
            children: List<Widget>.of(List<int>.generate(
                    7,
                    ((i) =>
                        _startTs[i] != null ? _startTs[i] + _endTs[i] : null))
                .map<Widget>(
                    (h) => _buildText(h != null ? dtOps.minsToHHmm(h) : ''))),
          ),
        ],
      ),
    );
  }

  _buildText(String text) {
    return Text(
      text,
      style: TextStyle(fontSize: 12.0),
    );
  }
}
