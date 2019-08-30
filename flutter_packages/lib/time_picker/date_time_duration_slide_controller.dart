import 'package:flutter/material.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;

class DateTimeDurationSlideController extends ChangeNotifier {
  bool draggingTimesEnabled = true;

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
  Animation durationSlideAnimation;

  Duration increaseSlideDuration = Duration(hours: 48 * 2);
  Duration decreaseSlideDuration;
  DateTime slideAnimationStartMinTime;
  DateTime slideAnimationStartMaxTime;
  DateTime slideAnimationStartStartTime;
  DateTime slideAnimationStartEndTime;

  Function done;

  Offset buttonOffset = Offset(0.0, 0.0);
  double xSlide = 0.0;

  DateTimeDurationSlideController({
    this.startTime,
    this.endTime,
    this.animationController,
    this.adjustAnimation,
    this.startSlideAnimation,
    this.endSlideAnimation,
    this.durationSlideAnimation,
    this.done,
  }) {
    //adjustTimes();
    adjustRange(startTime, endTime);

    adjustAnimation
      ..addStatusListener((s) {
        if (!adjustAnimationStarted && s == AnimationStatus.forward) {
          adjustAnimationStarted = true;
          animationStartMaxTime = maxTime;
          animationStartMinTime = minTime;

          var durationDistance = endTime.difference(startTime) * 2;
          /*
          var durationDistance = endTime.difference(startTime) * 1.0;
          animationMinDuration =
              minTime.difference(startTime.add(-durationDistance));
          animationMaxDuration =
              maxTime.difference(endTime.add(durationDistance));
              */

          //var durationDistance = Duration(hours: (24 * daysInDisplayMode()));
          animationMinDuration =
              (durationDistance - minTime.difference(startTime)) * 0.4;
          animationMaxDuration =
              (durationDistance - maxTime.difference(endTime)) * -0.4;

          var durationPaddingStart = minTime.difference(startTime);
          var durationPaddingEnd = maxTime.difference(endTime);

          var paddingDuration =
              (durationDistance - endTime.difference(startTime)) * 0.5;

          animationMinDuration = durationPaddingStart + paddingDuration;
          animationMaxDuration = durationPaddingEnd - paddingDuration;
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
            var newTime = slideAnimationStartStartTime
                .add(increaseSlideDuration * startSlideAnimation.value * -0.8);

            if (newTime.difference(endTime) <= Duration(hours: 24)) {
              startTime = newTime;
              adjustRange(
                  slideAnimationStartMinTime.add(
                      increaseSlideDuration * startSlideAnimation.value * -1.0),
                  slideAnimationStartMaxTime.add(
                      increaseSlideDuration * startSlideAnimation.value * 0.2));
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

  DateTime get adjustedStartTime => _adjustTime(startTime, true);
  DateTime get adjustedEndTime => _adjustTime(endTime, false);

  DateTime get adjustedMinTime => _adjustTime(minTime, true);
  DateTime get adjustedMaxTime => _adjustTime(maxTime, false);

  set setStartSlidePercentage(double draggingPercents) {
    startTime = minTime.add(visibleDuration * draggingPercents);
    notifyListeners();
  }

  set setEndSlidePercentage(double draggingPercents) {
    endTime = minTime.add(visibleDuration * draggingPercents);
    notifyListeners();
  }

  set setSlideX(double x) {
    xSlide = x;
    print(x);
    x > 200 ? done() : notifyListeners();

    if (x > 200) {
      done();
    } else {
      notifyListeners();
    }
  }

  setBoth(double topDraggingPercent, double bottomDraggingPercent) {
    var newStartDate = (minTime.add(visibleDuration * topDraggingPercent));
    var newEndDate = (minTime.add(visibleDuration * bottomDraggingPercent));

    startTime = newStartDate;
    endTime = newEndDate;
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

  DateTime _adjustTime(DateTime t, bool roundDown) {
    var mins = ((t.minute / 15.0).roundToDouble() * 15).round();
    return t.add(Duration(
        minutes: mins - t.minute,
        seconds: -t.second,
        milliseconds: -t.millisecond,
        microseconds: -t.microsecond));
  }
}

int daysThisMonth(int month) {
  if (month == 1) return 31;
  if (month == 2) return 28;
  if (month == 3) return 31;
  if (month == 4) return 30;
  if (month == 5) return 31;
  if (month == 6) return 30;
  if (month == 7) return 31;
  if (month == 8) return 31;
  if (month == 9) return 30;
  if (month == 10) return 31;
  if (month == 11) return 30;
  if (month == 12) return 31;
}
