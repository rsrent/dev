import 'package:flutter/material.dart';
import 'slide_controller.dart';
import 'dart:ui' as ui;
import 'dart:math' as math;
//import '../generic/datetimefunctions.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;

class TimeMarks extends StatelessWidget {
  final SlideController slideController;
  final Color markerColor;
  final Color backgroundColor;

  TimeMarks({
    this.slideController,
    this.markerColor,
    this.backgroundColor,
  });

  @override
  Widget build(BuildContext context) {
    return CustomPaint(
      painter: _TimeMarksPainter(
        slideController: slideController,
        backgroundColor: backgroundColor,
        markerColor: markerColor,
      ),
      child: Container(),
    );
  }
}

class _TimeMarksPainter extends CustomPainter {
  final paragraphStyle = new ui.ParagraphStyle(
    fontSize: 60.0,
    textAlign: TextAlign.start,
  );

  final DateTime tomorrow = DateTime.now().add(Duration(days: 1));
  final DateTime today = DateTime.now();
  final DateTime yesterday = DateTime.now().add(Duration(days: -1));

  final Paint markPaint;
  final Paint backgroundPaint;
  final SlideController slideController;

  final markerWidth = 20.0;

  _TimeMarksPainter({
    this.slideController,
    Color markerColor,
    Color backgroundColor,
  })  : markPaint = Paint()
          ..color = markerColor
          ..strokeWidth = 1.0
          ..style = PaintingStyle.stroke,
        backgroundPaint = Paint()
          ..color = backgroundColor
          ..style = PaintingStyle.fill;

  ui.Paragraph buildParagraph(String text, double fontsize) {
    final paragraphBuilder = new ui.ParagraphBuilder(paragraphStyle)
      ..pushStyle(new ui.TextStyle(color: markPaint.color, fontSize: fontsize))
      ..addText(text);
    final paragraph = paragraphBuilder.build();
    paragraph.layout(new ui.ParagraphConstraints(width: 100.0));
    return paragraph;
  }

  ui.Paragraph buildParagraphFromStyle(String text, ui.TextStyle style) {
    final paragraphBuilder = new ui.ParagraphBuilder(paragraphStyle)
      ..pushStyle(style)
      ..addText(text);
    final paragraph = paragraphBuilder.build();
    paragraph.layout(new ui.ParagraphConstraints(width: 200.0));
    return paragraph;
  }

  @override
  void paint(Canvas canvas, Size size) {
    canvas.drawRect(
      Rect.fromLTWH(0.0, 0.0, size.width, size.height),
      backgroundPaint,
    );

    dayDrawer(canvas, size);
  }

  @override
  bool shouldRepaint(CustomPainter oldDelegate) {
    return true;
  }

  void dayDrawer(Canvas canvas, Size size) {
    var minTime = slideController.minTime;

    //var firstMarkerTime = adjustTime(minTime);
    var firstMarkerTime = dtOps.adjustTime(minTime);

    if (firstMarkerTime.compareTo(minTime) == -1) {
      firstMarkerTime = firstMarkerTime.add(Duration(minutes: 15));
    }

    var wholeHourOffset = ((firstMarkerTime.minute) / 15).round();
    if (wholeHourOffset == 1)
      wholeHourOffset = 3;
    else if (wholeHourOffset == 3) wholeHourOffset = 1;

    var lineSkip = 0 + (wholeHourOffset % 2 == 1 ? 1 : 0);
    //print('lineSkip $lineSkip');

    var markerTopOffsetY = (firstMarkerTime.difference(minTime).inSeconds /
            slideController.visibleDuration.inSeconds) *
        size.height;

    var markerCount =
        //((slideController.visibleDuration.inMinutes.abs() / 1.0).round());
        ((slideController.visibleDuration.inMinutes.abs() / 15.0).round());

    var markerBottomOffsetY = (slideController.maxTime
                .difference(firstMarkerTime
                    .add(Duration(minutes: 15 * (markerCount - 1))))
                .inSeconds /
            slideController.visibleDuration.inSeconds) *
        size.height;

    //print('markerCount =  $markerCount');
    var markerGap = (size.height - markerTopOffsetY - markerBottomOffsetY) /
        math.max(1, markerCount - 1);

    for (int i = 0; i < markerCount; i++) {
      var markY = (markerGap * i) + markerTopOffsetY;
      //print('markY =  $markerGap * $i + $markerTopOffsetY');
      var time = firstMarkerTime.add(Duration(minutes: 15 * i));

      if (slideController.startMinBoundaryTime != null &&
              time.compareTo(slideController.startMinBoundaryTime) < 0 ||
          slideController.asDurationPicker &&
              time.compareTo(slideController.startTime) < 0) continue;

      if (i % 4 == wholeHourOffset &&
          time.difference(slideController.adjustedStartTime).abs() >
              Duration(minutes: 5) &&
          time.difference(slideController.adjustedEndTime).abs() >
              Duration(minutes: 5)) {
        final paragraphStyle = new ui.ParagraphStyle(
          fontSize: 60.0,
          textAlign: TextAlign.end,
        );
        final paragraphBuilder = new ui.ParagraphBuilder(paragraphStyle)
          ..pushStyle(new ui.TextStyle(color: markPaint.color, fontSize: 12.0))
          ..addText(dtOps.toHHmm(time));
        final paragraph = paragraphBuilder.build();
        paragraph.layout(new ui.ParagraphConstraints(width: 50.0));

        canvas.drawLine(
          Offset(size.width - (markerWidth / 2), markY),
          Offset(size.width, markY),
          markPaint,
        );

        canvas.drawParagraph(paragraph,
            Offset(size.width - 52.0 - (markerWidth / 2), markY - 7.0));
      } else if ((markerGap > 8.0 || i % 2 == lineSkip) && markY != null) {
        // print('size.width - markerWidth: ${size.width - markerWidth}');
        // print('size.width: ${size.width}');
        // print('markY: $markY');

        canvas.drawLine(
          Offset(size.width - markerWidth, markY),
          Offset(size.width, markY),
          markPaint,
        );
      }
    }
  }
}
