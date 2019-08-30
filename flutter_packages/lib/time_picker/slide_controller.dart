import 'package:flutter/material.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;

class SlideController extends ChangeNotifier {
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

  SlideDisplayMode displayMode = SlideDisplayMode.Day;

  Offset buttonOffset = Offset(0.0, 0.0);
  double xSlide = 0.0;

  SlideController({
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
    /*
    if (displayMode == SlideDisplayMode.Day) {
      return startTime.difference(minTime).inSeconds /
          visibleDuration.inSeconds;
    } else if (displayMode == SlideDisplayMode.Week ||
        displayMode == SlideDisplayMode.Month) {
      return startTime
              .add(Duration(hours: -startTime.hour))
              .difference(minTime)
              .inSeconds /
          visibleDuration.inSeconds;
    } else if (displayMode == SlideDisplayMode.Year) {
      return startTime
              .add(Duration(hours: -startTime.hour, days: -endTime.day + 1))
              .difference(minTime)
              .inSeconds /
          visibleDuration.inSeconds;
    } */
  }

  get endSlidePercentage {
    return adjustedEndTime.difference(minTime).inSeconds /
        visibleDuration.inSeconds;
/*
    if (displayMode == SlideDisplayMode.Day) {
      return endTime.difference(minTime).inSeconds / visibleDuration.inSeconds;
    } else if (displayMode == SlideDisplayMode.Week ||
        displayMode == SlideDisplayMode.Month) {
      return endTime
              .add(Duration(hours: 24 - endTime.hour))
              .difference(minTime)
              .inSeconds /
          visibleDuration.inSeconds;
    } else if (displayMode == SlideDisplayMode.Year) {
      return endTime
              .add(Duration(hours: 24 - endTime.hour, days: 30 - endTime.day))
              .difference(minTime)
              .inSeconds /
          visibleDuration.inSeconds;
    } */
  }

  DateTime get adjustedStartTime => _adjustTime(startTime, true);
  DateTime get adjustedEndTime => _adjustTime(endTime, false);

  DateTime get adjustedMinTime => _adjustTime(minTime, true);
  DateTime get adjustedMaxTime => _adjustTime(maxTime, false);

  set setStartSlidePercentage(double draggingPercents) {
    /*
    var newTime = minTime.add(visibleDuration * draggingPercents);
    if (endTime.compareTo(newTime.add(Duration(minutes: 15))) == -1) {
      newTime = endTime.add(Duration(minutes: -15));
    }
    startTime = adjustTime(newTime);
    */
    startTime = minTime.add(visibleDuration * draggingPercents);
    notifyListeners();
  }

  set setEndSlidePercentage(double draggingPercents) {
    /*
    var newTime = minTime.add(visibleDuration * draggingPercents);
    if (startTime.compareTo(newTime.add(Duration(minutes: -15))) == 1) {
      newTime = startTime.add(Duration(minutes: 15));
    }
    endTime = adjustTime(newTime);
    */

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

    if (displayMode == SlideDisplayMode.Day) {
      startTime = newStartDate;
      endTime = newEndDate;
    } else if (displayMode == SlideDisplayMode.Week ||
        displayMode == SlideDisplayMode.Month) {
      startTime = DateTime(
          newStartDate.year,
          newStartDate.month.clamp(1, daysThisMonth(newStartDate.month)),
          newStartDate.day,
          startTime.hour,
          startTime.minute);
      endTime = DateTime(
          newStartDate.year,
          newStartDate.month.clamp(1, daysThisMonth(newStartDate.month)),
          newStartDate.day,
          endTime.hour,
          endTime.minute);
    } else if (displayMode == SlideDisplayMode.Year) {
      //var newDate = (minTime.add(visibleDuration * topDraggingPercent));
      startTime = DateTime(
          newStartDate.year,
          newStartDate.month.clamp(1, daysThisMonth(newStartDate.month)),
          startTime.day,
          startTime.hour,
          startTime.minute);
      endTime = DateTime(
          newStartDate.year,
          newStartDate.month.clamp(1, daysThisMonth(newStartDate.month)),
          startTime.day,
          endTime.hour,
          endTime.minute);
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
    //print('minDraggingPercent: $minDraggingPercent');
    //print('maxDraggingPercent: $maxDraggingPercent');

    var newMinDate = (oldMinTime.add(visibleDuration * minDraggingPercent));
    var newMaxDate = (oldMinTime.add(visibleDuration * maxDraggingPercent));

    minTime = newMinDate;
    maxTime = newMaxDate;
    visibleDuration = maxTime.difference(minTime);

    /*
    if (displayMode == SlideDisplayMode.Day) {
      startTime = newStartDate;
      endTime = newEndDate;
    } else if (displayMode == SlideDisplayMode.Week ||
        displayMode == SlideDisplayMode.Month) {
      
      startTime = DateTime(newStartDate.year, newStartDate.month.clamp(1, daysThisMonth(newStartDate.month)), newStartDate.day,
          startTime.hour, startTime.minute);
      endTime = DateTime(newStartDate.year, newStartDate.month.clamp(1, daysThisMonth(newStartDate.month)), newStartDate.day, endTime.hour,
          endTime.minute);
    } else if (displayMode == SlideDisplayMode.Year) {
      //var newDate = (minTime.add(visibleDuration * topDraggingPercent));
      startTime = DateTime(newStartDate.year, newStartDate.month.clamp(1, daysThisMonth(newStartDate.month)), startTime.day,
          startTime.hour, startTime.minute);
      endTime = DateTime(newStartDate.year, newStartDate.month.clamp(1, daysThisMonth(newStartDate.month)), startTime.day,
          endTime.hour, endTime.minute);
    } */
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

  changeViewMode(bool forward) {
    switch (displayMode) {
      case SlideDisplayMode.Day:
        displayMode = (forward ? SlideDisplayMode.Week : SlideDisplayMode.Day);
        break;
      case SlideDisplayMode.Week:
        displayMode = (forward ? SlideDisplayMode.Month : SlideDisplayMode.Day);
        break;
      case SlideDisplayMode.Month:
        displayMode = (forward ? SlideDisplayMode.Year : SlideDisplayMode.Week);
        break;
      case SlideDisplayMode.Year:
        displayMode =
            (forward ? SlideDisplayMode.Year : SlideDisplayMode.Month);
        break;
    }
/*
    if (days == 1) {
      days = forward ? 8 : 365;
    } else if (days == 8) {
      days = forward ? 31 : 1;
    } else if (days == 31) {
      days = forward ? 365 : 8;
    } else if (days == 365) {
      days = forward ? 1 : 31;
    } */
    animationController.forward(from: 0.0);
  }

  int daysInDisplayMode() {
    switch (displayMode) {
      case SlideDisplayMode.Day:
        return 1;
      case SlideDisplayMode.Week:
        return 7;
      case SlideDisplayMode.Month:
        return 30;
      case SlideDisplayMode.Year:
        return 365;
    }
    return 0;
  }

  DateTime _adjustTime(DateTime t, bool roundDown) {
    var mins = ((t.minute / 15.0).roundToDouble() * 15).round();
    var dayTime = t.add(Duration(
        minutes: mins - t.minute,
        seconds: -t.second,
        milliseconds: -t.millisecond,
        microseconds: -t.microsecond));

    if (displayMode == SlideDisplayMode.Day) {
      return dayTime;
    }
    var weekTime = dayTime.add(Duration(
      hours: roundDown ? -t.hour : (24 - t.hour),
    ));
    if (displayMode == SlideDisplayMode.Week) {
      return t.add(Duration(
          hours: roundDown ? -t.hour : (24 - t.hour),
          minutes: -t.minute,
          seconds: -t.second));

      //return weekTime;
    }
    if (displayMode == SlideDisplayMode.Month) {
      return t.add(Duration(
          hours: roundDown ? -t.hour : (24 - t.hour),
          minutes: -t.minute,
          seconds: -t.second));
    }

    var monthTime = weekTime.add(Duration(
      days: roundDown ? -t.day : (daysThisMonth(t.month) - t.day),
    ));
    if (displayMode == SlideDisplayMode.Year) {
      var newTime = t.add(Duration(
          days: roundDown ? (-t.day + 1) : (daysThisMonth(t.month) - t.day + 1),
          hours: -t.hour,
          minutes: -t.minute,
          seconds: -t.second));
      //print('roundDown? $roundDown ${dtOps.toDDMM(newTime)} ${dtOps.toHHmm(newTime)}');
      return newTime;
    }

    return null;
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

enum SlideDisplayMode { Day, Week, Month, Year }
