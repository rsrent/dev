//import 'enums.dart';
import 'package:flutter/material.dart';

enum WorkStatus { Overdue, Critical, Okay, Unstarted, Ignored }





WorkStatus status(DateTime time) {
  if (time == null) return WorkStatus.Unstarted;
  if (time.compareTo(DateTime.now().add(Duration(days: -1))) == -1)
    return WorkStatus.Overdue;
  if (time.compareTo(DateTime.now().add(Duration(days: 2))) == -1)
    return WorkStatus.Critical;
  return WorkStatus.Okay;
}


Color getWorkStatusBackgroundColor(WorkStatus status) {
    if (status == WorkStatus.Unstarted)
      return Colors.grey;
    else if (status == WorkStatus.Overdue)
      return Colors.red[400];
    else if (status == WorkStatus.Critical)
      return Colors.yellow;
    else
      return Colors.white;
  }

  Color getWorkStatusTextColor(WorkStatus status) {
    if (status == WorkStatus.Unstarted)
      return Colors.grey;
    else if (status == WorkStatus.Overdue)
      return Colors.red[400];
    else if (status == WorkStatus.Critical)
      return Colors.yellow[800];
    else
      return Colors.green;
  }


  Widget getStatusIcon(WorkStatus status) {
    if (status == WorkStatus.Unstarted)
      return Icon(Icons.pause_circle_filled, color: Colors.grey, size: 40.0,);
    else if (status == WorkStatus.Overdue)
      return Icon(Icons.report, color: Colors.red, size: 40.0,);
    else if (status == WorkStatus.Critical)
      return Icon(Icons.report, color: Colors.yellow[700], size: 40.0,);
    else //if (status == WorkStatus.Okay)
      return Icon(Icons.check_circle, color: Colors.green, size: 40.0,);
  }

  String getWorkStatusName(WorkStatus status) {
    if (status == WorkStatus.Unstarted)
      return "Ustarted";
    else if (status == WorkStatus.Overdue)
      return 'Forsinket';
    else if (status == WorkStatus.Critical)
      return 'Kritisk';
    else 
      return 'Okay';
  }
