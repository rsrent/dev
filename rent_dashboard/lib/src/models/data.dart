import 'enums.dart';
import 'workstatus.dart';
import 'workdone.dart';
import 'count_with_workstatus.dart';
import 'geoposition.dart';

abstract class CommonData {
  int customerId;
  int userId;
  final int locationsTotal;
  final int locationsWithoutTasks;
  final int locationsWithoutStaff;
  final int locationsWithoutServiceLeader;
  final int incompleteReports;
  final int incompleteMorework;
  final int tasksTotal;
  final double reportsYearly;
  final double dg;

  CommonData.fromJson(json)
      : locationsTotal = json['locationsTotal'],
        locationsWithoutTasks = json['locationsWithoutTasks'],
        locationsWithoutStaff = json['locationsWithoutStaff'],
        locationsWithoutServiceLeader = json['locationsWithoutServiceLeader'],
        incompleteReports = json['incompleteReports'],
        incompleteMorework = json['incompleteMorework'],
        tasksTotal = json['tasksTotal'],
        reportsYearly = json['reportsYearly'],
        dg = (json['dg'] == 'NaN' || json['dg'] == '-Infinity'
            ? 0.0
            : json['dg']);
}

class SimpleData extends CommonData {
  CountWithWorkStatusSet tasks;
  CountWithWorkStatusSet reports;

  SimpleData(json) : super.fromJson(json) {
    var tasksTotal = json['tasksTotal'];
    var tasksDelayed = json['tasksDelayed'];
    var tasksOkay = json['tasksOkay'];
    var tasksUnstarted = json['tasksUnstarted'];
    var tasksCritical = tasksTotal - tasksDelayed - tasksOkay - tasksUnstarted;
    tasks = CountWithWorkStatusSet(
        nums: List()
          ..add(tasksDelayed)
          ..add(tasksCritical)
          ..add(tasksOkay)
          ..add(tasksUnstarted));

    var reportsDelayed = json['reportsDelayed'];
    var reportsOkay = json['reportsOkay'];
    var reportsUnstarted = json['reportsUnstarted'];
    var reportsCritical =
        locationsTotal - reportsDelayed - reportsOkay - reportsUnstarted;
    reports = CountWithWorkStatusSet(
        nums: List()
          ..add(reportsDelayed)
          ..add(reportsCritical)
          ..add(reportsOkay)
          ..add(reportsUnstarted));
  }
}

class CompleteData extends CommonData {
  final CountWithWorkStatusSet allTasks;
  final CountWithWorkStatusSet windowTasks;
  final CountWithWorkStatusSet fanCoilTasks;
  final CountWithWorkStatusSet periodicTasks;
  final CountWithWorkStatusSet qualityReportStatus;
  //final WorkDoneSet yesterday;
  //final WorkDoneSet today;
  final List<GeoPosition> serviceLeaderGeoLocations; 

  CompleteData.fromJson(json)
      : allTasks = CountWithWorkStatusSet(json: json['allTasks']),
        windowTasks = CountWithWorkStatusSet(json: json['windowTasks']),
        fanCoilTasks = CountWithWorkStatusSet(json: json['fanCoilTasks']),
        periodicTasks = CountWithWorkStatusSet(json: json['periodicTasks']),
        qualityReportStatus =
            CountWithWorkStatusSet(json: json['qualityReportStatus']),
        //yesterday = WorkDoneSet(json['yesterday']),
        //today = WorkDoneSet(json['today']),
        serviceLeaderGeoLocations = List.from(json['serviceLeaderGeoPositions']
            .map((gp) => GeoPosition.fromJson(gp)))
          ..addAll(List.from(
              json['restGeoPositions'].map((gp) => GeoPosition.fromJson(gp)))),
        super.fromJson(json);
}
