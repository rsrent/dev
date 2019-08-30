import 'package:flutter/material.dart';
import 'time_ui.dart';
import 'dart:async';
import 'time_drawer.dart';
import 'slide_controller.dart';
import 'slide_gesture_detector.dart';

Future<List<DateTime>> displayDateTimePicker(
    {BuildContext context, DateTime startTime, DateTime endTime}) {
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
            )),
  );
  return _completer.future;
}

class TimePicker extends StatefulWidget {
  final DateTime startTime;
  final DateTime endTime;
  final Color positiveColor;
  final Color negativeColor;
  final Function(DateTime, DateTime) datesSelected;
  TimePicker(
      {this.positiveColor,
      this.negativeColor,
      this.datesSelected,
      this.startTime,
      this.endTime});

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

  AnimationController durationSlideAnimationController;
  Animation durationSlideAnimation;

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

    durationSlideAnimationController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 30000),
    );
    durationSlideAnimation = CurvedAnimation(
      curve: Curves.linear,
      parent: durationSlideAnimationController,
    );

    slideController = SlideController(
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
      durationSlideAnimation: durationSlideAnimation,
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
        setState(() {});
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
              durationSlideAnimationController:
                  durationSlideAnimationController,
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
                          '538 Pilestræde',
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
