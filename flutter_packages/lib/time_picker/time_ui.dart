import 'package:flutter/material.dart';
import 'slide_controller.dart';
import 'package:dart_packages/date_time_operations.dart';

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

        var buttonX = constraints.maxWidth / 2 - 170.0 + slideController.xSlide;
        var buttonY = top + ((bottom - top) / 2) - 36.0;

        slideController.buttonOffset = Offset(buttonX, buttonY);

        //print('offset: ${slideController.xSlide}');

        return Stack(
          children: <Widget>[
            Positioned(
              left: 10.0,
              top: top - 45.0,
              child: slideController.displayMode == SlideDisplayMode.Day
                  ? Time(
                      time: topTime,
                      text: '', //'MØDT\nIND',
                      positiveColor: positiveColor,
                      negativeColor: negativeColor,
                      isTop: true,
                    )
                  : Container(),
            ),
            Positioned(
              left: buttonX,
              top: buttonY,
              child: slideController.displayMode == SlideDisplayMode.Day
                  ? Row(
                      children: <Widget>[
                        Padding(
                          padding: const EdgeInsets.only(right: 16.0),
                          child: SizedBox(
                            height: 64.0,
                            width: 64.0,
                            child: OutlineButton(
                              borderSide: BorderSide(
                                  color: negativeColor.withAlpha(100),
                                  width: 8.0),
                              child: Icon(Icons.keyboard_arrow_right,
                                  color: negativeColor.withAlpha(100),
                                  size: 30.0),
                              onPressed: () {},
                              shape: new RoundedRectangleBorder(
                                borderRadius: new BorderRadius.circular(60.0),
                              ),
                            ),
                          ),
                        ),
                        Text(
                          durationText,
                          style: TextStyle(
                            fontWeight: FontWeight.w100,
                            fontSize: 60.0,
                            color: negativeColor.withAlpha(100),
                          ),
                        ),
                      ],
                    )
                  : Container(),
            ),
            Positioned(
              left: 10.0,
              top: bottom - 25.0,
              child: slideController.displayMode == SlideDisplayMode.Day
                  ? Time(
                      time: bottomTime,
                      positiveColor: positiveColor,
                      negativeColor: negativeColor,
                      text: '', //'GÅET\nHJEM',
                      isTop: false,
                    )
                  : Container(),
            ),
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
          Text(
            toHHmm(time),
            style: TextStyle(
              fontSize: 40.0,
              color: positiveColor,
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(top: 6.0, bottom: 8.0),
            child: Text(
              '', //toDDMM(time),
              style: TextStyle(
                fontSize: 14.0,
                color: positiveColor,
              ),
            ),
          )
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
