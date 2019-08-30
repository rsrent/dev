import 'package:flutter/material.dart';

class UnfinishedSetup extends StatelessWidget {
  final int serviceLeader;
  final int personale;
  final int plan;

  final TextStyle numberStyle = TextStyle(fontSize: 24.0, color: Colors.white);
  final TextStyle textStyle = TextStyle(fontSize: 18.0, color: Colors.white);

  UnfinishedSetup(
      {this.serviceLeader,
      this.personale,
      this.plan});

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
              Text('$serviceLeader', style: numberStyle),
              Text('$personale', style: numberStyle),
              Text('$plan', style: numberStyle),
            ],
          ),
          Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text('Mangler service leder', style: textStyle),
              Text('Mangler personal', style: textStyle),
              Text('Mangler rengøringsplan', style: textStyle),
            ],
          ),
        ],
      ),
    );
  }
}
