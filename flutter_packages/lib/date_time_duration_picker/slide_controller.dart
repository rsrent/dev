import 'package:flutter/material.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;

class SlideController extends ChangeNotifier {
  bool draggingTimesEnabled = true;

  bool asDurationPicker = false;

  DateTime startMinBoundaryTime;
  DateTime startMaxBoundaryTime;

  DateTime startTime;
  DateTime endTime;

  DateTime oldMinTime;
  DateTime minTime;
  DateTime maxTime;
  Duration visibleDuration;

  bool adjustAnimationStarted = false;
  bool slideAnimationStarted = false;
  DateTime animationStartMinTime;
  DateTime animationStartMaxTime;

  Duration animationMinDuration;
  Duration animationMaxDuration;

  AnimationController animationController;
  Animation adjustAnimation;
  Animation startSlideAnimation;
  Animation endSlideAnimation;
  // Animation durationSlideAnimation;

  Duration increaseSlideDuration = Duration(hours: 5);
  Duration decreaseSlideDuration;
  DateTime slideAnimationStartMinTime;
  DateTime slideAnimationStartMaxTime;
  DateTime slideAnimationStartStartTime;
  DateTime slideAnimationStartEndTime;

  Function done;

  //Offset buttonOffset = Offset(0.0, 0.0);
  //double xSlide = 0.0;

  SlideController({
    this.asDurationPicker: false,
    this.startTime,
    this.endTime,
    this.startMinBoundaryTime,
    this.startMaxBoundaryTime,
    this.animationController,
    this.adjustAnimation,
    this.startSlideAnimation,
    this.endSlideAnimation,
    // this.durationSlideAnimation,
    this.done,
  }) {
    startTime = _adjustTime(startTime);
    endTime = _adjustTime(endTime);
    //adjustTimes();
    adjustRange(startTime, endTime);

    if (asDurationPicker == null) asDurationPicker = false;

    adjustAnimation
      ..addStatusListener((s) {
        if (!adjustAnimationStarted && s == AnimationStatus.forward) {
          adjustAnimationStarted = true;
          animationStartMaxTime = maxTime;
          animationStartMinTime = minTime;

          var actualDuration = endTime.difference(startTime);
          var screenDuration = actualDuration * 2;

          print(screenDuration);

          //*
          // if (screenDuration <= Duration(minutes: 30))
          //   screenDuration = Duration(minutes: 30);
          //*/
          // print(screenDuration);

          var whiteSpacePercentage =
              actualDuration.inSeconds / screenDuration.inSeconds;

          // print('whiteSpacePercentage $whiteSpacePercentage');

          //var durationDistance = endTime.difference(startTime) * 2;
          /*
          var durationDistance = endTime.difference(startTime) * 1.0;
          animationMinDuration =
              minTime.difference(startTime.add(-durationDistance));
          animationMaxDuration =
              maxTime.difference(endTime.add(durationDistance));
              */

          //var durationDistance = Duration(hours: (24 * daysInDisplayMode()));
          /*
          animationMinDuration =
              (screenDuration - minTime.difference(startTime)) * (whiteSpacePercentage / 2);
          animationMaxDuration =
              (screenDuration - maxTime.difference(endTime)) * -(whiteSpacePercentage / 2);
              */

          var durationPaddingStart = minTime.difference(startTime);
          var durationPaddingEnd = maxTime.difference(endTime);

          var paddingDuration = (screenDuration) * (whiteSpacePercentage / 1);

          animationMinDuration = durationPaddingStart + paddingDuration;
          animationMaxDuration = durationPaddingEnd - paddingDuration;

          print('animationMinDuration: $animationMinDuration');
          print('animationMaxDuration: $animationMaxDuration');
        }
        if (s == AnimationStatus.completed) {
          adjustAnimationStarted = false;
        }
      })
      ..addListener(() {
        if (adjustAnimationStarted) {
          adjustRange(
              animationStartMinTime
                  .add(animationMinDuration * (-adjustAnimation.value)),
              animationStartMaxTime
                  .add(animationMaxDuration * (-adjustAnimation.value)));
          notifyListeners();
        }
      });

    startSlideAnimation
      ..addStatusListener((s) {
        if ((s == AnimationStatus.forward || s == AnimationStatus.reverse) &&
            !slideAnimationStarted) {
          slideAnimationStarted = true;
          slideAnimationStartMaxTime = maxTime;
          slideAnimationStartMinTime = minTime;
          slideAnimationStartStartTime = startTime;
        } else if (s == AnimationStatus.completed ||
            s == AnimationStatus.dismissed) {
          slideAnimationStarted = false;

          notifyListeners();
        }
      })
      ..addListener(() {
        if (slideAnimationStarted) {
          if (startSlideAnimation.status == AnimationStatus.forward) {
            //var range = (endTime.difference(startTime).inMilliseconds);

            //startTime =
            //    startTime.subtract(Duration(seconds: (range * 0.0001).round()));

            //return;
            print('startSlideAnimation.value ${startSlideAnimation.value}');
            //var multiPlyer = startSlideAnimation.value;

            var range = (endTime.difference(startTime));

            range =
                range < increaseSlideDuration ? range : increaseSlideDuration;

            var durationIncreaser =
                range * startSlideAnimation.value * 10; //increaseSlideDuration
            var newTime =
                slideAnimationStartStartTime.add(durationIncreaser * -0.8);

            if (newTime.difference(endTime) <= Duration(hours: 24)) {
              startTime = newTime;
              adjustRange(
                  slideAnimationStartMinTime.add(durationIncreaser * -1.0),
                  slideAnimationStartMaxTime.add(durationIncreaser * 0.2));
            }
          }
          notifyListeners();
        }
      });

    endSlideAnimation
      ..addStatusListener((s) {
        if ((s == AnimationStatus.forward || s == AnimationStatus.reverse) &&
            !slideAnimationStarted) {
          slideAnimationStarted = true;
          slideAnimationStartMaxTime = maxTime;
          slideAnimationStartMinTime = minTime;
          slideAnimationStartEndTime = endTime;
        } else if (s == AnimationStatus.completed ||
            s == AnimationStatus.dismissed) {
          slideAnimationStarted = false;

          notifyListeners();
        }
      })
      ..addListener(() {
        if (slideAnimationStarted) {
          if (endSlideAnimation.status == AnimationStatus.forward) {
            var newTime = slideAnimationStartEndTime
                .add(increaseSlideDuration * endSlideAnimation.value * 0.8);

            if (startTime.difference(newTime) <= Duration(hours: 24)) {
              endTime = newTime;
              adjustRange(
                  slideAnimationStartMinTime.add(
                      increaseSlideDuration * endSlideAnimation.value * -0.2),
                  slideAnimationStartMaxTime.add(
                      increaseSlideDuration * endSlideAnimation.value * 1.0));
            }
          }
          notifyListeners();
        }
      });
  }

  get startSlidePercentage {
    return adjustedStartTime.difference(minTime).inSeconds /
        visibleDuration.inSeconds;
  }

  get endSlidePercentage {
    return adjustedEndTime.difference(minTime).inSeconds /
        visibleDuration.inSeconds;
  }

  DateTime get adjustedStartTime {
    if (startMinBoundaryTime != null &&
        startMinBoundaryTime.compareTo(startTime) > 0) {
      startTime = startMinBoundaryTime;
    }
    if (startMaxBoundaryTime != null &&
        startMaxBoundaryTime.compareTo(startTime) < 0) {
      startTime = startMaxBoundaryTime;
    }

    return _adjustTime(startTime);
  }

  DateTime get adjustedEndTime {
    if (endTime.compareTo(startTime.add(Duration(minutes: 5))) <= 0) {
      endTime = startTime.add(Duration(minutes: 5));
    }

    return _adjustTime(endTime);
  }

  DateTime get adjustedMinTime => _adjustTime(minTime);
  DateTime get adjustedMaxTime => _adjustTime(maxTime);

  set setStartSlidePercentage(double draggingPercents) {
    startTime = minTime.add(visibleDuration * draggingPercents);
    if (endTime.difference(startTime).inSeconds < 5 * 60) {
      startTime = endTime.add(Duration(minutes: -5));
    }
    //startTime = adjustedStartTime;
    notifyListeners();
  }

  set setEndSlidePercentage(double draggingPercents) {
    endTime = minTime.add(visibleDuration * draggingPercents);
    endTime = adjustedEndTime;
    notifyListeners();
  }

  // set setSlideX(double x) {
  //   xSlide = x;
  //   print(x);
  //   x > 200 ? done() : notifyListeners();

  //   if (x > 200) {
  //     done();
  //   } else {
  //     notifyListeners();
  //   }
  // }

  setBoth(double topDraggingPercent, double bottomDraggingPercent) {
    var newStartDate = (minTime.add(visibleDuration * topDraggingPercent));
    var newEndDate = (minTime.add(visibleDuration * bottomDraggingPercent));

    if (!outsideStartBoundary(newStartDate)) {
      startTime = newStartDate;
      endTime = newEndDate;
    }

    notifyListeners();
  }

  setDate(double p) {
    var newDate = (minTime.add(visibleDuration * p));
    startTime = DateTime(newDate.year, newDate.month, newDate.day,
        startTime.hour, startTime.minute);
    endTime = DateTime(
        newDate.year, newDate.month, newDate.day, endTime.hour, endTime.minute);
    //print("${dtOps.toDDMM(newDate)}");
    notifyListeners();
  }

  setRange(double minDraggingPercent, double maxDraggingPercent) {
    var newMinDate = (oldMinTime.add(visibleDuration * minDraggingPercent));
    var newMaxDate = (oldMinTime.add(visibleDuration * maxDraggingPercent));

    minTime = newMinDate;
    maxTime = newMaxDate;
    visibleDuration = maxTime.difference(minTime);
    notifyListeners();
  }

  adjustRange(DateTime min, DateTime max) {
    minTime = min;
    maxTime = max;
    visibleDuration = maxTime.difference(minTime);
  }

  incrementDate(int days) {
    startTime = startTime.add(Duration(days: days));
    endTime = endTime.add(Duration(days: days));
    notifyListeners();
    //adjustRange(startTime, endTime);
    animationController.forward(from: 0.0);
  }

  int daysInDisplayMode() {
    return 1;
  }

  DateTime _adjustTime(DateTime t) {
    //print('range: ${(maxTime.difference(minTime).inMinutes)}');

    //return t;
    var mins = 0;
    var range = (endTime.difference(startTime).inMinutes);
    if (range >= 120) {
      mins = ((t.minute / 15.0).roundToDouble() * 15).round();
    } else if (range >= 60) {
      mins = ((t.minute / 5.0).roundToDouble() * 5).round();
      // } else if (range >= 30) {
      //   mins = ((t.minute / 2.0).roundToDouble() * 2).round();
    } else {
      mins = ((t.minute / 1.0).roundToDouble() * 1).round();
    }
    var roundedTime = t;

    roundedTime = roundedTime.add(Duration(
        minutes: mins - roundedTime.minute,
        seconds: -roundedTime.second,
        milliseconds: -roundedTime.millisecond,
        microseconds: -roundedTime.microsecond));
    return roundedTime;
  }

  bool outsideStartBoundary(DateTime time) {
    if (startMinBoundaryTime != null &&
        startMinBoundaryTime.compareTo(time) > 0) {
      return true;
    }
    if (startMaxBoundaryTime != null &&
        startMaxBoundaryTime.compareTo(time) < 0) {
      print('Ye bby');
      return true;
    }
    return false;
  }
}
