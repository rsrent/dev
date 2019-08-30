import 'package:flutter/material.dart';
import 'package:meta/meta.dart';
import 'dart:math';

class ExplainOverlay {
  var radius = 600.0;
  List<ExplainItem> _items;

  ExplainOverlay(this.vsync);
  TickerProvider vsync;

  List<Function> animationControllerStarters = List();
  List<Function> animationControllerEnders = List();
  OverlayEntry _backGroundOverlay;

  AnimationController backgroundAnimation;

  show(BuildContext context, List<ExplainItem> items) async {
    _items = items;
    animationControllerStarters = List();
    animationControllerEnders = List();

    backgroundAnimation = AnimationController(
        vsync: vsync, duration: Duration(microseconds: 300));
    var animation =
        CurvedAnimation(curve: Curves.easeIn, parent: backgroundAnimation);

    _backGroundOverlay = OverlayEntry(builder: (context) {
      return GestureDetector(
        child: Stack(
          children: <Widget>[
            AnimatedBuilder(
              animation: backgroundAnimation,
              builder: (BuildContext context, Widget child) {
                return Opacity(
                  opacity: animation.value,
                  child: Container(
                    color: Colors.black.withAlpha(100),
                  ),
                );
              },
            ),
          ]..addAll(buildList(context)),
        ),
        onTap: () {
          //_backGroundOverlay.remove();
        },
      );
    });
    Overlay.of(context).insertAll([_backGroundOverlay]);

    backgroundAnimation.addStatusListener((s) {
      if (s == AnimationStatus.completed) {
        animationControllerStarters[0]();
      }
      if (s == AnimationStatus.dismissed) {
        _backGroundOverlay.remove();
      }
    });

    backgroundAnimation.forward();

    Future.delayed(Duration(milliseconds: 100)).then((v) {
      print('Lenght???: ${animationControllerStarters.length}');
    });
  }

  List<Positioned> buildList(BuildContext context) {
    var list = List<Positioned>();

    for (int i = 0; i < _items.length; i++) {
      var item = _items[i];
      list.addAll(addOverlay(context, item, () {
        if (i + 1 < _items.length)
          animationControllerStarters[i + 1]();
        else
          backgroundAnimation.reverse();
      }));
    }
    return list;
  }

  List<Positioned> addOverlay(
      BuildContext context, ExplainItem item, Function() onEnded) {
    var controller = AnimationController(
        vsync: vsync, duration: Duration(milliseconds: 500));

    var animation = CurvedAnimation(parent: controller, curve: Curves.bounceIn);

    controller.addStatusListener((status) {
      if (status == AnimationStatus.forward) {}
      if (status == AnimationStatus.dismissed) {
        onEnded();
      }
    });

    RenderBox renderBox = item.key.currentContext.findRenderObject();
    final position = renderBox.localToGlobal(Offset.zero);

    animationControllerStarters.add(() {
      controller.forward();
    });

    animationControllerEnders.add(() {
      controller.reverse();
    });

    var targetRadius = max(renderBox.size.width, renderBox.size.height);

    return [
      Positioned(
        left: position.dx - (radius / 2) + (renderBox.size.width / 2),
        top: position.dy - (radius / 2) + (renderBox.size.height / 2),
        child: GestureDetector(
          child: AnimatedBuilder(
            animation: controller,
            builder: (BuildContext context, Widget child) {
              return Transform.scale(
                scale: animation.value,
                child: Container(
                  width: radius,
                  height: radius,
                  padding: EdgeInsets.all(radius / 6),
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: Theme.of(context).primaryColor,
                  ),
                  child: Stack(
                    children: <Widget>[
                      Align(
                        alignment: Alignment.center,
                        child: Container(
                          width: targetRadius * 2,
                          height: targetRadius * 2,
                          decoration: BoxDecoration(
                            shape: BoxShape.circle,
                            color: Colors.white.withAlpha(50),
                          ),
                        ),
                      ),
                      Align(
                        //heightFactor: 0.5,
                        //widthFactor: 0.5,
                        alignment: item.alignment,
                        child: Material(
                            child: Container(
                                width: radius / 3,
                                //margin: EdgeInsets.all(50),
                                color: Theme.of(context).primaryColor,
                                child: Text(
                                  item.text,
                                  style: TextStyle(fontSize: 20),
                                ))),
                      ),
                    ],
                  ),
                ),
              );
            },
          ),
          onTap: () {
            controller.reverse();
            if (item.action != null) item.action();
          },
        ),
      ),
      Positioned(
        left: position.dx,
        top: position.dy,
        child: AnimatedBuilder(
          animation: controller,
          builder: (BuildContext context, Widget child) {
            return Opacity(
              opacity: animation.value,
              child: IgnorePointer(
                child: item.builder(context),
              ),
            );
          },
        ),
      )
    ];
  }
}

class ExplainItem {
  GlobalKey key;
  WidgetBuilder builder;
  Alignment alignment;
  String text;
  Color color;
  Function action;
  ExplainItem({
    @required this.key,
    @required this.builder,
    @required this.alignment,
    @required this.text,
    this.color,
    this.action,
  });
}