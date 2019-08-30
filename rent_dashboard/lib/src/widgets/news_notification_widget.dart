import 'package:flutter/material.dart';
import '../blocs/data_provider.dart';
import 'dart:async';

class NewsNotificationWidget extends StatelessWidget {
  final Widget child;

  NewsNotificationWidget({this.child});

  @override
  Widget build(BuildContext context) {
    return Material(
      child: Stack(
        alignment: Alignment.center,
        children: [
          child,
          _NewsNotificationAnimationWidget(),
        ],
      ),
    );
  }
}

class _NewsNotificationAnimationWidget extends StatefulWidget {
  @override
  _NewsNotificationAnimationWidgetState createState() =>
      _NewsNotificationAnimationWidgetState();
}

class _NewsNotificationAnimationWidgetState
    extends State<_NewsNotificationAnimationWidget>
    with SingleTickerProviderStateMixin {
  AnimationController introAnimationController;
  Animation introAnimation;

  _NewsNotificationAnimationWidgetState() {
    introAnimationController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 1000),
    );
    introAnimation = CurvedAnimation(
      parent: introAnimationController,
      curve: Curves.elasticInOut,
    );
  }

  @override
  Widget build(BuildContext context) {
    var bloc = DataProvider.of(context);
    return StreamBuilder(
      stream: bloc.newNews,
      builder: (context, snapshot) {
        if (!snapshot.hasData || !snapshot.data) return Container();

        introAnimationController.forward();

        return Container(
          child: AnimatedBuilder(
            animation: introAnimation,
            builder: (context, child) {
              return Transform.scale(
                scale: introAnimation.value,
                child: Container(
                  width: 360.0,
                  height: 360.0,
                  decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: Colors.teal,
                      boxShadow: [BoxShadow()]),
                  child: FlatButton(
                    child: Center(
                      child: Text(
                        'News',
                        style: TextStyle(color: Colors.white, fontSize: 40.0),
                      ),
                    ),
                    onPressed: () {
                      introAnimationController.reverse();
                      Future.delayed(Duration(milliseconds: 1000), () {
                        bloc.latestNewsSeen();
                      });
                    },
                  ),
                ),
              );
            },
          ),
        );
      },
    );
  }
}
