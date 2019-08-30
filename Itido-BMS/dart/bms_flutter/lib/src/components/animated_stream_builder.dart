import 'package:flutter/material.dart';

import 'animated_transition.dart';

typedef AnimatedStreamWidgetBuilder<V> = dynamic Function(
    BuildContext context, V data);

class AnimatedStreamBuilder<V> extends StatelessWidget {
  final AnimatedStreamWidgetBuilder<V> builder;

  final Stream<V> stream;
  final V initialData;
  final bool callOnNull;

  const AnimatedStreamBuilder({
    Key key,
    @required this.builder,
    @required this.stream,
    this.initialData,
    this.callOnNull = false,
  })  : assert(builder != null),
        super(key: key);

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: stream,
      initialData: initialData,
      builder: (BuildContext context, AsyncSnapshot<V> snapshot) {
        var child;
        TransitionWidget widget;

        if (snapshot.hasData || callOnNull) {
          var data = snapshot.data;

          child = builder(context, data);

          if (child is Widget) {
            var name = data.toString();
            print('AnimatedStreamBuilder: $name');
            widget = TransitionWidget(
              child: child,
              name: name,
            );
          } else if (child is TransitionWidget) {
            widget = child;
            print('AnimatedStreamBuilder: ${widget.name}');
          }
        }

        if (widget == null) {
          widget = AnimatedTransition.emptyTransition;
        }

        return AnimatedTransition(
          duration: Duration(milliseconds: 500),
          revealType: RevealType.SlideInFromBottom,
          curve: Curves.easeOut,
          revealTypeReverse: RevealType.SlideInFromBottom,
          curveReverse: Curves.easeIn,
          child: widget,
        );
      },
    );
  }
}
