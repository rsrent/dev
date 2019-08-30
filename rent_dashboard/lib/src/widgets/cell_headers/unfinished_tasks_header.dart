import 'package:flutter/material.dart';

class UnfinishedTasks extends StatelessWidget {
  final int qualityreports;
  final int morework;

  final TextStyle numberStyle = TextStyle(fontSize: 24.0, color: Colors.white);
  final TextStyle textStyle = TextStyle(fontSize: 18.0, color: Colors.white);

  UnfinishedTasks(
      {this.qualityreports,
      this.morework});

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 100.0,
      color: Colors.grey,
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: [
          Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              Text('$qualityreports', style: numberStyle),
              Text('$morework', style: numberStyle),
            ],
          ),
          Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text('Uafsluttede kvalitetsrapporter', style: textStyle),
              Text('Uafsluttede merarbejde', style: textStyle),
            ],
          ),
        ],
      ),
    );
  }
}
