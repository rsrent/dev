import 'package:flutter/material.dart';

class DgHeader extends StatelessWidget {
  final double dg;
  DgHeader({this.dg});

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Text(
        '${(dg*100).toStringAsFixed(2)}%',
        style: TextStyle(
          fontSize: 50.0,
          color: Colors.black,
          fontWeight: FontWeight.normal,
        ),
      ),
    );
  }
}
