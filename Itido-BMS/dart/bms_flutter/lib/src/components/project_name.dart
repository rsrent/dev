import 'package:bms_dart/models.dart';
import 'package:flutter/material.dart';

class ProjectName extends StatelessWidget {
  final Project project;

  const ProjectName({Key key, @required this.project}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    var path = project.path.fold<String>('', (acc, val) => '$acc$val, ');
    if (path.length > 2) path = path.substring(0, path.length - 2);
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: <Widget>[
        Text(
          path,
          style: TextStyle(fontWeight: FontWeight.w300),
        ),
        Text(
          project.name,
        ),
      ],
    );
  }
}
