import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';

class SelectableCircularAvatar extends StatelessWidget {
  final String name;
  final bool selectable;
  final bool selected;
  final Color backgroundColor;

  const SelectableCircularAvatar({
    Key key,
    @required this.selectable,
    @required this.selected,
    this.name,
    this.backgroundColor,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 50,
      height: 50,
      child: AnimatedTransition(
        animateFirst: false,
        revealType: RevealType.Scale,
        child: buildLeading(context),
      ),
    );
  }

  TransitionWidget buildLeading(BuildContext context) {
    if (selectable && selected) {
      return TransitionWidget(
        name: 'selected',
        child: CircleAvatar(
          child: Icon(Icons.check),
          backgroundColor: Theme.of(context).primaryColor,
        ),
      );
    } else if (selectable) {
      return TransitionWidget(
        name: 'not selected',
        child: CircleAvatar(
          child: Icon(
            Icons.check,
            color: Colors.grey,
          ),
          backgroundColor: backgroundColor ?? Colors.grey[200],
        ),
      );
    } else {
      return TransitionWidget(
        name: 'normal',
        child: CircleAvatar(
          child: Text((name ?? ' ').substring(0, 1)),
          backgroundColor: backgroundColor ?? Colors.grey[200],
        ),
      );
    }
  }
}
