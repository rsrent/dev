import 'package:flutter/material.dart';

class Space extends StatelessWidget {
  final double width;
  final double height;

  const Space({Key key, this.width = 16, this.height = 16}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: width,
      height: height,
    );
  }
}
