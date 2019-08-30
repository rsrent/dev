import 'package:bms_flutter/src/components/animated_transition.dart';
import 'package:dart_packages/streamer.dart';
import 'package:flutter/material.dart';

class TestWidget extends StatefulWidget {
  @override
  _TestWidgetState createState() => _TestWidgetState();
}

class _TestWidgetState extends State<TestWidget> {
  Streamer<int> index = Streamer(seedValue: 0);
  var colors = [
    Colors.yellow,
    Colors.red,
    Colors.blue,
    Colors.pink,
    Colors.green
  ];
  @override
  Widget build(BuildContext context) {
    return StreamBuilder<int>(
        stream: index.stream,
        builder: (context, snapshot) {
          return AnimatedTransition(
            child: TransitionWidget(
                child: Column(
                  children: <Widget>[
                    Expanded(
                      child: Container(
                        color: colors[index.value],
                        child:
                            Center(child: Text(colors[index.value].toString())),
                      ),
                    ),
                    RaisedButton(
                      child: Text('Update'),
                      onPressed: () {
                        var _index = index.value;
                        _index++;
                        if (_index >= colors.length) _index = 0;
                        index.update(_index);
                      },
                    ),
                  ],
                ),
                name: colors[index.value].toString()),
          );
        });
  }
}
