import 'package:flutter/material.dart';
import 'package:dart_packages/date_time_operations.dart';
import 'slide_controller.dart';

class SliderTimeUi extends StatelessWidget {
  final SlideController slideController;
  final Color positiveColor;
  final Color negativeColor;
  final double paddingTop;
  final double paddingBottom;

  SliderTimeUi({
    this.slideController,
    this.positiveColor,
    this.negativeColor,
    this.paddingTop,
    this.paddingBottom,
  });

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (BuildContext context, BoxConstraints constraints) {
        final height = constraints.maxHeight - paddingTop - paddingBottom;
        final top =
            height - (height * (1.0 - slideController.startSlidePercentage));
        final bottom =
            height - (height * (1.0 - slideController.endSlidePercentage));

        final topTime = slideController.adjustedStartTime;
        final bottomTime = slideController.adjustedEndTime;

        var duration = (bottomTime.difference(topTime));
        var durationText = duration.toString().substring(0, 5);
        if (duration.inHours < 10)
          durationText = '0${duration.toString().substring(0, 4)}';

        // var buttonX = constraints.maxWidth / 2 - 170.0 + slideController.xSlide;
        // var buttonY = top + ((bottom - top) / 2) - 16.0;

        var center = top + ((bottom - top) / 2);

        // slideController.buttonOffset = Offset(buttonX, buttonY);

        //print('offset: ${slideController.xSlide}');

        return Stack(
          children: <Widget>[
            Positioned(
              left: 10.0,
              top: top - 45.0,
              child: (slideController.asDurationPicker ?? false)
                  ? Container()
                  : Time(
                      time: topTime,
                      text: '', //'MØDT\nIND',
                      positiveColor: positiveColor,
                      negativeColor: negativeColor,
                      isTop: true,
                    ),
            ),
            Positioned(
              left: constraints.maxWidth / 2 - 5,
              top: top - 2.0,
              child: slideController.asDurationPicker
                  ? Container()
                  : Icon(
                      Icons.drag_handle,
                      color: negativeColor.withAlpha(100),
                    ),
            ),
            Positioned(
              left: 10,
              top: center,
              child: FractionalTranslation(
                translation: Offset(0, -0.5),
                child: Text(
                  durationText,
                  style: TextStyle(
                    fontWeight: FontWeight.w100,
                    fontSize: 40.0,
                    color: negativeColor.withAlpha(100),
                  ),
                ),
              ),
            ),
            Positioned(
              left: constraints.maxWidth / 2 - 5,
              top: center,
              child: FractionalTranslation(
                translation: Offset(0, -0.5),
                child: Icon(
                  Icons.gps_fixed,
                  color: negativeColor.withAlpha(100),
                ),
              ),
            ),
            Positioned(
              left: 10.0,
              top: bottom - 25.0,
              child: slideController.asDurationPicker
                  ? Container()
                  : Time(
                      time: bottomTime,
                      positiveColor: positiveColor,
                      negativeColor: negativeColor,
                      text: '', //'GÅET\nHJEM',
                      isTop: false,
                    ),
            ),
            Positioned(
              left: constraints.maxWidth / 2 - 5,
              top: bottom - 22.0,
              child: Icon(
                Icons.drag_handle,
                color: negativeColor.withAlpha(100),
              ),
            ),
            Positioned(
              bottom: 40,
              right: 20,
              child: FloatingActionButton(
                onPressed: () {
                  slideController.done();
                },
                child: Icon(Icons.check),
                backgroundColor: Theme.of(context).primaryColor,
              ),
            )
          ],
        );
      },
    );
  }
}

class Time extends StatelessWidget {
  final DateTime time;
  final String text;
  final Color positiveColor;
  final Color negativeColor;
  final bool isTop;

  Time({
    this.time,
    this.text,
    this.positiveColor,
    this.negativeColor,
    this.isTop,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: widgets(),
    );
  }

  List<Widget> widgets() {
    var ws = <Widget>[
      Row(
        crossAxisAlignment:
            isTop ? CrossAxisAlignment.end : CrossAxisAlignment.start,
        children: [
          Padding(
            padding: const EdgeInsets.only(top: 6.0, bottom: 8.0, right: 6),
            child: Text(
              'kl',
              style: TextStyle(
                fontSize: 20.0,
                color: positiveColor,
              ),
            ),
          ),
          Text(
            toHHmm(time),
            style: TextStyle(
              fontSize: 40.0,
              color: positiveColor,
            ),
          ),
        ],
      ),
      Divider(
        height: 10.0,
        color: Colors.transparent,
      ),
      Text(
        text,
        style: TextStyle(
          color: negativeColor,
        ),
      ),
    ];

    return isTop ? ws : List.of(ws.reversed);
  }
}
