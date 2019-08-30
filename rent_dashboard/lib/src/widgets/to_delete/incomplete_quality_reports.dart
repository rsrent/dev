import 'package:flutter/material.dart';

class IncompleteQualityReportsWidget extends StatelessWidget {
  final int count;
  IncompleteQualityReportsWidget(this.count);
  @override
  Widget build(BuildContext context) {
    return count > 0
        ? Row(
            children: <Widget>[
              Padding(
                padding: const EdgeInsets.only(right: 8.0),
                child: Icon(
                  Icons.report_problem,
                  color: Color(0x66FFAB00),
                ),
              ),
              Text('$count report mangler at blive afsluttet',
                  style: TextStyle(color: Color(0xFF999999))),
            ],
          )
        : Container();
  }
}
