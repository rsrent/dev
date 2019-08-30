import 'package:flutter/material.dart';

class MultiAnimatedCrossFade extends StatelessWidget {
  final List<Widget> widgets;
  final int displayState;
  MultiAnimatedCrossFade(this.widgets, this.displayState);

  @override
  Widget build(BuildContext context) {
    return _buildWidgets(0, widgets);
  }

  _buildWidgets(int index, List<Widget> widgets) {
    //print('index $index');
    //print('length: ${widgets.length}');
    //print('selected: $displayState');

    return AnimatedCrossFade(
      firstChild: widgets[index],
      secondChild: widgets.length == (index + 2)
          ? widgets[index + 1]
          : _buildWidgets(index + 1, widgets),
      firstCurve: const Interval(0.0, 0.6, curve: Curves.fastOutSlowIn),
      secondCurve: const Interval(0.4, 1.0, curve: Curves.fastOutSlowIn),
      sizeCurve: Curves.fastOutSlowIn,
      crossFadeState: displayState == index
          ? CrossFadeState.showFirst
          : CrossFadeState.showSecond,
      duration: const Duration(milliseconds: 200),
    );
  }
}
/*
class MultiAnimatedCrossFade extends StatefulWidget {
  final List<Widget> widgets;
  final int displayState;
  MultiAnimatedCrossFade(this.widgets, this.displayState) {}

  _MultiAnimatedCrossFadeState createState() => _MultiAnimatedCrossFadeState();
}

class _MultiAnimatedCrossFadeState extends State<MultiAnimatedCrossFade> {
  int displayState = 0;

  @override
  void initState() {
    super.initState();
  }

  void updateState(int s) {
    setState(() {
      displayState = s;
    });
  }

  @override
  Widget build(BuildContext context) {
    return _buildWidgets(0, widget.widgets);
  }

  _buildWidgets(int index, List<Widget> widgets) {
    print('index');
    print(index);
    return AnimatedCrossFade(
      firstChild: widgets[index],
      secondChild: widgets.length == (index + 2)
          ? widgets[index + 1]
          : _buildWidgets(index + 1, widgets),
      firstCurve: const Interval(0.0, 0.6, curve: Curves.fastOutSlowIn),
      secondCurve: const Interval(0.4, 1.0, curve: Curves.fastOutSlowIn),
      sizeCurve: Curves.fastOutSlowIn,
      crossFadeState: displayState == index
          ? CrossFadeState.showFirst
          : CrossFadeState.showSecond,
      duration: const Duration(milliseconds: 200),
    );
  }
}
*/
