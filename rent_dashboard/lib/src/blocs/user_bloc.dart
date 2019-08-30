import 'sortable_bloc.dart';

import 'package:rxdart/rxdart.dart';
import 'dart:async';
import '../models/user.dart';
import '../resources/user_repository.dart';
import '../models/sortable_by.dart';
import '../models/workstatus.dart';

class UserBloc extends SortableBloc<ServiceLeader> {
  final _userRepository = UserRepository();
  final _userSubject = BehaviorSubject<List<ServiceLeader>>();

  //Streams
  Observable<List<ServiceLeader>> get serviceLeaders => _userSubject.stream;

  static List<SortBy> sortableBy = [
        SortBy.Name,
        SortBy.QualityReportStatus,
        SortBy.CleaningTaskStatus,
        SortBy.DG,
        SortBy.UnfinishedSetup,
        SortBy.UnfinishedTasks
      ];
  sortableByList() => sortableBy;

  addToSinkSorted(List<ServiceLeader> list) {
    loaded = list;

    list = List<ServiceLeader>.from(list.where((c) {
      return c.name.toLowerCase().contains(filteringBy.toLowerCase());
    }).map((u) {
      u.smallData.reports.delayed.include = qualityReportStatusOptions[WorkStatus.Overdue];
      u.smallData.reports.critical.include = qualityReportStatusOptions[WorkStatus.Critical];
      u.smallData.reports.okay.include = qualityReportStatusOptions[WorkStatus.Okay];
      u.smallData.reports.unstarted.include = qualityReportStatusOptions[WorkStatus.Unstarted];

      u.smallData.tasks.delayed.include = cleaningTaskStatusOptions[WorkStatus.Overdue];
      u.smallData.tasks.critical.include = cleaningTaskStatusOptions[WorkStatus.Critical];
      u.smallData.tasks.okay.include = cleaningTaskStatusOptions[WorkStatus.Okay];
      u.smallData.tasks.unstarted.include = cleaningTaskStatusOptions[WorkStatus.Unstarted];
      return u;
    }));

    if (sortedBy == SortBy.Name)
      list.sort((l1, l2) => l1.name.compareTo(l2.name));

    if (sortedBy == SortBy.QualityReportStatus)
      list.sort(
          (l1, l2) => l1.smallData.reports.comparedTo(l2.smallData.reports));

    if (sortedBy == SortBy.CleaningTaskStatus)
      list.sort((l1, l2) => l1.smallData.tasks.comparedTo(l2.smallData.tasks));

    if (sortedBy == SortBy.DG)
      list.sort((l1, l2) => l1.smallData.dg.compareTo(l2.smallData.dg));

    if (sortedBy == SortBy.UnfinishedSetup)
      list.sort((l1, l2) => (l2.smallData.locationsWithoutServiceLeader +
              l2.smallData.locationsWithoutStaff +
              l2.smallData.locationsWithoutTasks)
          .compareTo(l1.smallData.locationsWithoutServiceLeader +
              l1.smallData.locationsWithoutStaff +
              l1.smallData.locationsWithoutTasks));

    if (sortedBy == SortBy.UnfinishedTasks)
      list.sort((l1, l2) =>
          (l2.smallData.incompleteMorework + l2.smallData.incompleteReports)
              .compareTo(l1.smallData.incompleteMorework +
                  l1.smallData.incompleteReports));

     _userSubject.sink.add(list);
    /*
    _userSubject.sink.add(List<ServiceLeader>.from(loaded.where((c) {
      return c.name.toLowerCase().contains(filteringBy.toLowerCase());
    }))); */
  }

  int inspected = 0;

  fetchServiceLeaders() async {
    var sls = await _userRepository.fetchServiceLeaders();
    addToSinkSorted(sls);
    //_userSubject.sink.add(sls);
  }

  dispose() {
    _userSubject.close();
  }
}
