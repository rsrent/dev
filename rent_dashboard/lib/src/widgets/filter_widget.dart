import 'package:flutter/material.dart';
import '../models/sortable_by.dart';
import 'dart:core';
import 'dart:math';
import '../models/workstatus.dart';
import '../models/enums.dart';

typedef SortBy CurrentSort();

//typedef Future<List<Location>> ListFromSource(LocationSource s);

class FliterWidget extends StatefulWidget {
  final List<SortBy> sortBy;
  final Function(SortBy) updateSortBy;
  final Function(String) updateFilterBy;
  final CurrentSort currentSort;
  final Widget child;

  final Map<WorkStatus, bool> Function(WorkStatus, bool)
      updateQualityReportStatusOptions;
  final Map<WorkStatus, bool> Function(WorkStatus, bool)
      updateCleaningTaskStatusOptions;
  final Map<PlanType, bool> Function(PlanType, bool) updateCleaningTaskPlanOptions;

  FliterWidget({
    this.sortBy,
    this.updateSortBy,
    this.updateFilterBy,
    this.currentSort,
    this.child,
    this.updateQualityReportStatusOptions,
    this.updateCleaningTaskStatusOptions,
    this.updateCleaningTaskPlanOptions,
  });
  @override
  _FliterWidgetState createState() => _FliterWidgetState();
}

class _FliterWidgetState extends State<FliterWidget>
    with TickerProviderStateMixin {
  Animation<double> animation;
  AnimationController animationController;

  bool dismissing = false;

  bool animatingSubOptions = false;
  //bool doneAnimatingSubOptions = false;
  //Animation<double> subAnimation;
  //AnimationController subAnimationController;

  //Map<WorkStatus, bool> subOptions;
  //Map<int, bool> subSubOptions;

  Map<WorkStatus, bool> Function(WorkStatus, bool) subOptions;
  Map<PlanType, bool> Function(PlanType, bool) subSubOptions;

  _FliterWidgetState() {
    animationController = AnimationController(
      vsync: this,
      duration: Duration(milliseconds: 300),
    );

    animation = CurvedAnimation(
      curve: Curves.easeOut,
      parent: animationController,
    )..addStatusListener((s) {
        if (s == AnimationStatus.dismissed) {
          if (dismissing) {
            animatingSubOptions = false;
            //doneAnimatingSubOptions = false;
            subOptions = null;
          } else if (subOptions != null && !dismissing) {
            animatingSubOptions = true;
            animationController.forward();
          }
        }
      });
  }

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: [
        /*
        Column(
          children: [
            Container(
              margin: EdgeInsets.all(8.0),
              child: TextField(
                onChanged: widget.updateFilterBy,
                decoration: InputDecoration(
                  hintText: 'Søg efter navn',
                  icon: Icon(Icons.search),
                ),
              ),
              height: 50.0,
            ),
            Expanded(
              child: widget.child,
            ),
          ],
          mainAxisSize: MainAxisSize.max,
        ),*/
        widget.child,
        AnimatedBuilder(
          animation: animation,
          builder: (context, child) {
            return Container(
              width: 2000.0,
              height: 2000.0,
              color: animationController.value == 0.0
                  ? null
                  : Color(0x000000).withAlpha(
                      (animationController.value * 200.0)
                          .round()
                          .clamp(0, 255)),
              child: Stack(
                alignment: Alignment.bottomRight,
                overflow: Overflow.visible,
                children: <Widget>[]
                  ..add(_buildDissmissBackground())
                  ..addAll(_buildSupOptionButtons(subOptions, 0.0))
                  ..addAll(_buildSupOptionButtons(subSubOptions, 140.0))
                  ..addAll(_buildButtons())
                  ..add(child),
              ),
            );
          },
          child: Transform.translate(
            offset: Offset(-20.0, -20.0),
            child: FloatingActionButton(
              onPressed: () {
                if (animationController.status == AnimationStatus.dismissed) {
                  dismissing = false;
                  animationController.forward();
                } else if (animationController.status ==
                    AnimationStatus.completed) {
                  dismissing = true;
                  animationController.reverse();
                }
              },
              backgroundColor: Colors.teal,
              child: Icon(Icons.search),
            ),
          ),
        ),
      ],
    );
  }

  _buildButtons() {
    List<Widget> widgets = List();

    if (animatingSubOptions) return widgets;

    int index = 1;

    widget.sortBy.forEach((s) {
      var button = Positioned(
        child: Stack(
          overflow: Overflow.visible,
          alignment: Alignment.center,
          children: [
            SizedBox(
              height: 40.0,
              width: 40.0,
              child: RaisedButton(
                onPressed: () {
                  _setSubOptions(s);
                  animationController.reverse();
                  widget.updateSortBy(s);
                },
                color: widget.currentSort() != s
                    ? Theme.of(context).primaryColor
                    : Colors.white,
                shape: CircleBorder(),
              ),
            ),
            IgnorePointer(
              child: getButtonLogo(s),
            ),
            Positioned(
              right: 50.0,
              child: Text(
                getButtonText(s),
                textAlign: TextAlign.right,
                style: TextStyle(
                  color: Colors.white.withAlpha(
                      ((animationController.value - 0.9).clamp(0.0, 1.0) * 2550)
                          .round()),
                ),
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
              ),
            )
          ],
        ),
        bottom: (50.0 * index) * (animation.value * animation.value) + 34.0,
        right: 28.0, //(80.0 * index) * animation.value,
      );
      widgets.add(button);
      index++;
    });

    return widgets;
  }

  _buildDissmissBackground() {
    return Positioned(
      right: 0.0,
      bottom: 0.0,
      child: SizedBox.fromSize(
        child: FlatButton(
          onPressed: () {
            dismissing = true;
            animationController.reverse();
          },
        ),
        size: Size(2000.0 * animationController.value,
            2000.0 * animationController.value),
      ),
    );
  }

  Widget getButtonLogo(SortBy s) {
    var color = widget.currentSort() == s
        ? Theme.of(context).primaryColor
        : Colors.white;
    var textStyle =
        TextStyle(color: color, fontWeight: FontWeight.bold, fontSize: 20.0);

    switch (s) {
      case SortBy.Name:
        return Text('N', style: textStyle);
      case SortBy.CleaningTaskStatus:
        return Icon(Icons.assignment, color: color);
      case SortBy.QualityReportStatus:
        return Icon(Icons.assignment, color: color);
      case SortBy.DG:
        return Icon(Icons.attach_money, color: color);
      case SortBy.ServiceLeader:
        return Icon(Icons.face, color: color);
      case SortBy.UnfinishedSetup:
        return Icon(Icons.warning, color: color);
      case SortBy.UnfinishedTasks:
        return Icon(Icons.assignment_late, color: color);
    }
    return Text('');
  }

  String getButtonText(SortBy s) {
    switch (s) {
      case SortBy.Name:
        return 'Navn';
      case SortBy.CleaningTaskStatus:
        return 'Opgavestatus';
      case SortBy.QualityReportStatus:
        return 'Kvalitetsrapportstatus';
      case SortBy.DG:
        return 'Dækningsgrad';
      case SortBy.ServiceLeader:
        return 'Service leder';
      case SortBy.UnfinishedSetup:
        return 'Setup mangler';
      case SortBy.UnfinishedTasks:
        return 'Uafsluttede opgaver';
    }
    return '';
  }

  _setSubOptions(SortBy s) {
    if (s == SortBy.QualityReportStatus) {
      subOptions = widget.updateQualityReportStatusOptions;
      subSubOptions = null;
    } else if (s == SortBy.CleaningTaskStatus) {
      subOptions = widget.updateCleaningTaskStatusOptions;
      subSubOptions = widget.updateCleaningTaskPlanOptions;
    } else {
      subOptions = null;
      subSubOptions = null;
    }
  }

  _buildSupOptionButtons(subOps, double xOffSet) {
    List<Widget> widgets = List();

    if (subOps == null || subOps(null, null) == null || !animatingSubOptions) return widgets;

    int index = 1;

    subOps(null, null).forEach((s, v) {
      var button = Positioned(
        child: Stack(
          overflow: Overflow.visible,
          alignment: Alignment.center,
          children: [
            SizedBox(
              height: 40.0,
              width: 40.0,
              child: RaisedButton(
                onPressed: () {
                  subOps(s, !v);
                  setState(() {});
                  //doneAnimatingSubOptions = true;
                  //animationController.reverse();
                  //widget.updateSortBy(s);
                },
                color: getSubStringColor(s, v),
                shape: CircleBorder(),
              ),
            ),
            IgnorePointer(
              child: Icon(Icons.warning),
            ),
            Positioned(
              right: 50.0,
              child: Text(
                getSubStringText(s),
                textAlign: TextAlign.right,
                style: TextStyle(
                  color: Colors.white.withAlpha(
                      ((animationController.value - 0.9).clamp(0.0, 1.0) * 2550)
                          .round()),
                ),
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
              ),
            )
          ],
        ),
        bottom: (50.0 * index) * (animation.value * animation.value) + 34.0,
        right: 28.0 +
            (xOffSet * animation.value), //(80.0 * index) * animation.value,
      );
      widgets.add(button);
      index++;
    });

    return widgets;
  }

  Color getSubStringColor(dynamic s, v) {
    if(v == false) return Colors.white;

    if (s == WorkStatus.Overdue) return Colors.red;
    if (s == WorkStatus.Critical) return Colors.yellow;
    if (s == WorkStatus.Okay) return Colors.green;
    if (s == WorkStatus.Unstarted) return Colors.grey;
    if (s == PlanType.Vinduer) return Colors.lightBlue;
    if (s == PlanType.FanCoil) return Colors.lightGreen;
    if (s == PlanType.Periodic) return Colors.orange;
  }

  String getSubStringText(dynamic s)
  {
    if (s == WorkStatus.Overdue) return 'Forsinket';
    if (s == WorkStatus.Critical) return 'Kritisk';
    if (s == WorkStatus.Okay) return 'Okay';
    if (s == WorkStatus.Unstarted) return 'Ikke started';
    if (s == PlanType.Vinduer) return 'Vinduer';
    if (s == PlanType.FanCoil) return 'Fan coil';
    if (s == PlanType.Periodic) return 'Periodisk';
    if (s == PlanType.Normal) return 'Normal';
  }
}
