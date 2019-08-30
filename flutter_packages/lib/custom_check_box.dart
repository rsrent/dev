import 'package:flutter/material.dart';
//import '../../style.dart' as style;

class CustomCheckBox extends StatelessWidget {
  final bool active;
  final Function(bool) update;
  final Widget activeChild;

  CustomCheckBox({this.active, this.update, this.activeChild});

  @override
  Widget build(BuildContext context) {
    return Stack(
      alignment: Alignment.center,
      children: <Widget>[
        Container(
            width: 30.0,
            height: 30.0,
            decoration: BoxDecoration(
              border: Border.all(color: Colors.grey, width: 0.5),
              shape: BoxShape.circle,
            ),
            child: AnimatedCrossFade(
              firstChild: Container(
                  width: 30.0,
                  height: 30.0,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: Theme.of(context).primaryColor,
                  ),
                  child: activeChild ??
                      Icon(
                        Icons.check,
                        color: Colors.white,
                      )),
              secondChild: Container(
                width: 30.0,
                height: 30.0,
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: Colors.grey[200],
                  //border: Border.all(width: 0, color: Colors.grey),
                ),
              ),
              firstCurve: const Interval(0.0, 0.6, curve: Curves.fastOutSlowIn),
              secondCurve:
                  const Interval(0.4, 1.0, curve: Curves.fastOutSlowIn),
              sizeCurve: Curves.fastOutSlowIn,
              crossFadeState:
                  active ? CrossFadeState.showFirst : CrossFadeState.showSecond,
              duration: const Duration(milliseconds: 200),
            )),
        SizedBox(
          width: 40.0,
          height: 40.0,
          child: FlatButton(
              onPressed: () {
                update(!active);
              },
              child: Container(),
              splashColor: Colors.transparent,
              highlightColor: Colors.transparent),
        ),
      ],
    );
  }
}
