import 'package:flutter/material.dart';

class ServiceLeaderHeader extends StatelessWidget {
  final String serviceLeader;

  final TextStyle textStyle = TextStyle(fontSize: 24.0, color: Colors.white);

  ServiceLeaderHeader({this.serviceLeader});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.all(20.0),
      height: 70.0,
      color: Colors.grey,
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: [
          Icon(
            Icons.account_circle,
            size: 40.0,
            color: Colors.white,
          ),
          Container(
            margin: EdgeInsets.all(10.0),
          ),
          Expanded(
            child: Text(
              '$serviceLeader',
              style: textStyle,
            ),
          ),
        ],
      ),
    );
  }
}
