import 'package:flutter/material.dart';
import 'date_time_duration_picker/time_ui.dart';
import 'dart:async';
import 'date_time_duration_picker/time_drawer.dart';
import 'date_time_duration_picker/slide_controller.dart';
import 'date_time_duration_picker/slide_gesture_detector.dart';

Future<int> displayDurationPicker({
  BuildContext context,
  int minutes,
}) {
  Completer<int> _completer = Completer();
  Navigator.push(
    context,
    MaterialPageRoute(
        builder: (context) => TimePicker(
              datesSelected: (startTime, endTime) {
                if (!_completer.isCompleted) {
                  _completer.complete(endTime.difference(startTime).inMinutes);
                  Navigator.pop(context);
                }
              },
              positiveColor: Theme.of(context).primaryColor,
              negativeColor: Theme.of(context).scaffoldBackgroundColor,
              startTime: DateTime(2000, 1, 1),
              endTime: DateTime(2000, 1, 1).add(Duration(minutes: minutes)),
              asDurationPicker: true,
            )),
  );
  return _completer.future;
}

Future<List<DateTime>> displayDateTimeDurationPicker({
  BuildContext context,
  DateTime startTime,
  DateTime endTime,
  DateTime startMinBoundaryTime,
  DateTime startMaxBoundaryTime,
  bool asDurationPicker,
}) {
  Completer<List<DateTime>> _completer = Completer();

  Navigator.push(
    context,
    MaterialPageRoute(
        builder: (context) => TimePicker(
              datesSelected: (startTime, endTime) {
                if (!_completer.isCompleted) {
                  _completer.complete([startTime, endTime]);
                  Navigator.pop(context);
                }
              },
              positiveColor: Theme.of(context).primaryColor,
              negativeColor: Theme.of(context).scaffoldBackgroundColor,
              startTime: startTime,
              endTime: endTime,
              startMaxBoundaryTime: startMaxBoundaryTime,
              startMinBoundaryTime: startMinBoundaryTime,
              asDurationPicker: asDurationPicker,
            )),
  );
  return _completer.future;
}

class TimePicker extends StatefulWidget {
  final DateTime startTime;
  final DateTime endTime;
  final DateTime startMinBoundaryTime;
  final DateTime startMaxBoundaryTime;
  final bool asDurationPicker;
  final Color positiveColor;
  final Color negativeColor;
  final Function(DateTime, DateTime) datesSelected;
  TimePicker(
      {this.positiveColor,
      this.negativeColor,
      this.datesSelected,
      this.startTime,
      this.endTime,
      this.startMinBoundaryTime,
      this.startMaxBoundaryTime,
      this.asDurationPicker});

  @override
  _TimePickerState createState() => _TimePickerState();
}

class _TimePickerState extends State<TimePicker> with TickerProviderStateMixin {
  final paddingTop = 0.0;
  final paddingBottom = 0.0;

  AnimationController adjustAnimationController;
  Animation adjustAnimation;

  AnimationController topSlideAnimationController;
  Animation topSlideAnimation;

  AnimationController bottomSlideAnimationController;
  Animation bottomSlideAnimation;

  // AnimationController durationSlideAnimationController;
  // Animation durationSlideAnimation;

  SlideController slideController;

  @override
  void initState() {
    super.initState();

    adjustAnimationController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 300),
    );
    adjustAnimation = CurvedAnimation(
      curve: Curves.easeIn,
      parent: adjustAnimationController,
    );

    topSlideAnimationController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 30000),
    );
    topSlideAnimation = CurvedAnimation(
      curve: Curves.linear,
      parent: topSlideAnimationController,
    );

    bottomSlideAnimationController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 30000),
    );
    bottomSlideAnimation = CurvedAnimation(
      curve: Curves.linear,
      parent: bottomSlideAnimationController,
    );

    // durationSlideAnimationController = AnimationController(
    //   vsync: this,
    //   duration: Duration(milliseconds: 30000),
    // );
    // durationSlideAnimation = CurvedAnimation(
    //   curve: Curves.linear,
    //   parent: durationSlideAnimationController,
    // );

    slideController = SlideController(
      asDurationPicker: widget.asDurationPicker,
      startMinBoundaryTime: widget.startMinBoundaryTime,
      startMaxBoundaryTime: widget.startMaxBoundaryTime,
      startTime: widget.startTime != null
          ? widget.startTime
          : DateTime.now().add(Duration(hours: 1)),
      endTime: widget.endTime != null
          ? widget.endTime
          : widget.startTime != null
              ? widget.startTime.add(Duration(hours: 8))
              : DateTime.now().add(Duration(hours: 9)),
      animationController: adjustAnimationController,
      adjustAnimation: adjustAnimation,
      startSlideAnimation: topSlideAnimation,
      endSlideAnimation: bottomSlideAnimation,
      // durationSlideAnimation: durationSlideAnimation,
      done: () {
        if (widget.datesSelected != null) {
          widget.datesSelected(slideController.adjustedStartTime,
              slideController.adjustedEndTime);
        }
        /*
        Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => Scaffold(
                    appBar: AppBar(),
                  )),
        ); */
      },
    )..addListener(() {
        if (mounted) {
          setState(() {});
        }
      });

    adjustAnimationController.forward();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Column(
        children: <Widget>[
          Expanded(
            child: SlideDragger(
              slideController: slideController,
              paddingTop: paddingTop,
              paddingBottom: paddingBottom,
              animationController: adjustAnimationController,
              topSlideAnimationController: topSlideAnimationController,
              bottomSlideAnimationController: bottomSlideAnimationController,
              // durationSlideAnimationController:
              //     durationSlideAnimationController,
              child: Stack(
                children: [
                  TimeMarks(
                    slideController: slideController,
                    markerColor: widget.positiveColor,
                    backgroundColor: widget.negativeColor,
                  ),
                  ClipRect(
                    child: TimeMarks(
                      slideController: slideController,
                      markerColor: widget.negativeColor,
                      backgroundColor: widget.positiveColor,
                    ),
                    clipper: RectClipper(
                      slideController: slideController,
                      paddingTop: paddingTop,
                      paddingBottom: paddingBottom,
                    ),
                  ),
                  SliderTimeUi(
                    slideController: slideController,
                    positiveColor: widget.positiveColor,
                    negativeColor: widget.negativeColor,
                    paddingBottom: paddingBottom,
                    paddingTop: paddingTop,
                  ),
                  Column(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      AppBar(
                        title: Text(
                          '',
                          style: TextStyle(color: widget.positiveColor),
                        ),
                        backgroundColor: Colors.transparent,
                        elevation: 0.0,
                        brightness: Brightness.light,
                        iconTheme: IconThemeData(
                          color: Theme.of(context).primaryColor,
                        ),
                        leading: IconButton(
                          icon: Icon(
                            Icons.keyboard_arrow_left, //Icons.menu,
                            size: 40.0,
                          ),
                          onPressed: () {
                            Navigator.of(context).pop(true);
                          },
                        ),
                        actions: [],
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );

    /*
    return Scaffold(
      appBar: AppBar(
        title: Text(
          '538 Pilestræde',
          style: TextStyle(color: widget.negativeColor),
        ),
        backgroundColor: widget.positiveColor,
        elevation: 0.0,
        brightness: Brightness.light,
        iconTheme: IconThemeData(
          color: widget.negativeColor,
        ),
        leading: IconButton(
          icon: Icon(
            Icons.keyboard_arrow_left, //Icons.menu,
            size: 40.0,
          ),
          onPressed: () {
            // TODO:
            Navigator.of(context).pop(true);
          },
        ),
        actions: [
          //_buildTextButton('settings'.toUpperCase(), true),
          FlatButton(
            child: Text(
              'BEKRÆFT',
              style: TextStyle(color: widget.negativeColor),
            ),
            onPressed: () {},
          )
        ],
      ),
      body: SlideDragger(
        slideController: slideController,
        paddingTop: paddingTop,
        paddingBottom: paddingBottom,
        animationController: adjustAnimationController,
        topSlideAnimationController: topSlideAnimationController,
        bottomSlideAnimationController: bottomSlideAnimationController,
        durationSlideAnimationController: durationSlideAnimationController,
        child: Stack(
          children: [
            TimeMarks(
              slideController: slideController,
              markerColor: widget.positiveColor,
              backgroundColor: widget.negativeColor,
            ),
            ClipRect(
              child: TimeMarks(
                slideController: slideController,
                markerColor: widget.negativeColor,
                backgroundColor: widget.positiveColor,
              ),
              clipper: RectClipper(
                slideController: slideController,
                paddingTop: paddingTop,
                paddingBottom: paddingBottom,
              ),
            ),
            SliderTime(
              slideController: slideController,
              positiveColor: widget.positiveColor,
              negativeColor: widget.negativeColor,
              paddingBottom: paddingBottom,
              paddingTop: paddingTop,
            ),
          ],
        ),
      ),
    );
    */
  }
}

class RectClipper extends CustomClipper<Rect> {
  SlideController slideController;
  double paddingTop;
  double paddingBottom;
  RectClipper({
    this.slideController,
    this.paddingTop,
    this.paddingBottom,
  });

  @override
  Rect getClip(Size size) {
    final height = (size.height - paddingBottom) - paddingTop;

    final top =
        height - (height * (1.0 - slideController.startSlidePercentage));
    final bottom =
        height - (height * (1.0 - slideController.endSlidePercentage));
    return Rect.fromLTRB(0.0, top, size.width, bottom);
  }

  @override
  bool shouldReclip(CustomClipper<Rect> oldClipper) {
    return true;
  }
}
