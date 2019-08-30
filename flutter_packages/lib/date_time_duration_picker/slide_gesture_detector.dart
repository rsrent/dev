import 'package:flutter/material.dart';
import 'slide_controller.dart';
import 'dart:async';
//import 'package:vibrate/vibrate.dart';

class SlideDragger extends StatefulWidget {
  final SlideController slideController;
  final double paddingTop;
  final double paddingBottom;
  final AnimationController animationController;
  final AnimationController topSlideAnimationController;
  final AnimationController bottomSlideAnimationController;
  // final AnimationController durationSlideAnimationController;

  final Widget child;
  SlideDragger({
    this.slideController,
    this.paddingTop,
    this.paddingBottom,
    this.child,
    this.animationController,
    this.topSlideAnimationController,
    this.bottomSlideAnimationController,
    // this.durationSlideAnimationController,
  });
  @override
  _SlideDraggerState createState() => _SlideDraggerState();
}

enum SlideDraggerOperation {
  Checking,
  DraggingTop,
  DraggingCenter,
  DraggingBottom,
  DraggingButton,
}

class _SlideDraggerState extends State<SlideDragger> {
  SlideDraggerOperation operation = SlideDraggerOperation.Checking;
  Offset startDrag;
  double startTopDragPercent;
  double startBottomDragPercent;
  double maxHeight;

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      child: widget.child,
      onScaleStart: _onScaleStart,
      onScaleUpdate: _onScaleUpdate,
      onScaleEnd: _onScaleEnd,
    );
  }

  void setOperation(SlideDraggerOperation o) {
    if (operation != SlideDraggerOperation.DraggingButton) operation = o;

    print(operation);
  }

  void _onScaleStart(ScaleStartDetails details) {
    print('_onScaleStart');

    operation = SlideDraggerOperation.Checking;
    startDrag = Offset(details.focalPoint.dx, details.focalPoint.dy);
    startTopDragPercent = widget.slideController.startSlidePercentage;
    startBottomDragPercent = widget.slideController.endSlidePercentage;
    widget.slideController.oldMinTime = widget.slideController.minTime;
    RenderBox getBox = context.findRenderObject();
    var local = getBox.globalToLocal(details.focalPoint);
    maxHeight = getBox.size.height;
    var startPercent = (local.dy / maxHeight);

    print('1: ${details.focalPoint}');
    // print('2: ${widget.slideController.buttonOffset}');
    // print(
    //     '2: ${(details.focalPoint - widget.slideController.buttonOffset).distance}');

    // if ((details.focalPoint - widget.slideController.buttonOffset).distance <
    //     80) {
    //   print('btn??');
    //   setOperation(SlideDraggerOperation.DraggingButton);
    // }

    var distFromTop = (startTopDragPercent - startPercent).abs();
    var distFromBottom = (startBottomDragPercent - startPercent).abs();

    if ((distFromBottom.abs() - distFromTop.abs()).abs() < 0.2 &&
        !widget.slideController.asDurationPicker) {
      setOperation(SlideDraggerOperation.DraggingCenter);
    } else {
      if (distFromBottom > distFromTop &&
          !widget.slideController.asDurationPicker) {
        setOperation(SlideDraggerOperation.DraggingTop);
      } else if (distFromBottom < distFromTop) {
        setOperation(SlideDraggerOperation.DraggingBottom);
      }
    }
  }

  void _onScaleUpdate(ScaleUpdateDetails details) {
    final dragDistance = startDrag.dy - details.focalPoint.dy;
    final sliderHeight =
        context.size.height - widget.paddingTop - widget.paddingBottom;
    final dragPercent = dragDistance / sliderHeight;

    // if (operation == SlideDraggerOperation.DraggingButton) {
    //   print("Button update");
    //   widget.slideController.setSlideX = details.focalPoint.dx - startDrag.dx;
    //   return;
    // }

    if (operation == SlideDraggerOperation.DraggingCenter) {
      var topsNewPercentage = startTopDragPercent - (dragPercent);
      var bottomsNewPercentage = startBottomDragPercent - (dragPercent);
      widget.slideController.setBoth(topsNewPercentage, bottomsNewPercentage);
    }

    if (operation == SlideDraggerOperation.DraggingTop &&
        !widget.slideController.asDurationPicker) {
      var newPercentage = startTopDragPercent - dragPercent;

      if (newPercentage < 0.10 || maxHeight * newPercentage < 150) {
        if (!widget.topSlideAnimationController.isAnimating) {
          widget.topSlideAnimationController.forward(from: 0.0);
        }
      } else {
        if (widget.topSlideAnimationController.isAnimating) {
          widget.topSlideAnimationController.forward(from: 1.0);
        }
        widget.slideController.setStartSlidePercentage = newPercentage;
      }
    }

    if (operation == SlideDraggerOperation.DraggingBottom) {
      var newPercentage = startBottomDragPercent - dragPercent;
      if (newPercentage > 0.9 ||
          (maxHeight) - maxHeight * newPercentage < 120) {
        if (!widget.bottomSlideAnimationController.isAnimating) {
          widget.bottomSlideAnimationController.forward(from: 0.0);
        }
      } else {
        if (widget.bottomSlideAnimationController.isAnimating) {
          widget.bottomSlideAnimationController.forward(from: 1.0);
        }
        widget.slideController.setEndSlidePercentage = newPercentage;
      }
    }
  }

  void _onScaleEnd(ScaleEndDetails s) {
    // if (widget.slideController.xSlide < 200) {
    //   widget.slideController.setSlideX = 0.0;
    // } else {
    //   Future.delayed(Duration(seconds: 1), () {
    //     widget.slideController.setSlideX = 0.0;
    //   });
    // }
    widget.topSlideAnimationController.forward(from: 1.0);
    widget.bottomSlideAnimationController.forward(from: 1.0);

    if (operation == SlideDraggerOperation.DraggingTop ||
        operation == SlideDraggerOperation.DraggingCenter ||
        operation == SlideDraggerOperation.DraggingBottom) {
      widget.animationController.forward(from: 0.0);
    }
  }

  void ifDayViewElse(dayFunction, notDayFunction, details) {
    dayFunction(details);
  }
}
