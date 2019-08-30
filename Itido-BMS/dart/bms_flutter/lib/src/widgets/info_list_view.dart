import 'package:flutter/material.dart';

import '../../components.dart';

class InfoListView extends StatelessWidget {
  final String info;
  final Widget child;

  const InfoListView({Key key, this.info, this.child}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return ListView(
      children: <Widget>[
        Padding(
          padding: const EdgeInsets.only(top: 20),
          child: Center(
              child: Column(
            children: <Widget>[
              if (child != null) child,
              Space(
                height: 50,
              ),
              if (info != null) Text(info),
            ],
          )),
        ),
      ],
    );
  }
}
