import 'package:flutter/material.dart';

class NotificationBadge<T> extends StatelessWidget {
  final Widget child;
  final Stream<T> stream;
  final int Function(T) toNotifications;
  final AlignmentGeometry alignment;

  NotificationBadge(
      {this.child,
      this.stream,
      this.toNotifications,
      this.alignment: Alignment.topRight});

  @override
  Widget build(BuildContext context) {
    return Stack(
      overflow: Overflow.visible,
      children: <Widget>[
        child,
        Align(
          alignment: alignment,
          child: StreamBuilder(
            stream: stream,
            builder: (BuildContext context, AsyncSnapshot<T> snapshot) {
              if (snapshot.hasData) {
                var amount = toNotifications(snapshot.data);
                if (amount == 0) return Container();
                return Container(
                  width: 16,
                  height: 16,
                  child: Center(
                    child: Text(
                      '$amount',
                      style: TextStyle(color: Colors.white, fontSize: 10),
                    ),
                  ),
                  decoration: BoxDecoration(
                      //boxShadow: null,
                      color: Colors.red,
                      borderRadius:
                          BorderRadius.all(const Radius.circular(8.0))),
                );
              }
              return Container();
            },
          ),
          // top: 2,
          // right: floatRight ? 2 : null,
          // left: floatRight ? null : 2,
        )
      ],
    );
  }
}
