import 'sortable_bloc.dart';

import '../resources/location_repository.dart';
import 'package:rxdart/rxdart.dart';
import '../models/location.dart';
import '../models/sortable_by.dart';
import '../models/workstatus.dart';

class LocationBloc extends SortableBloc<Location> {
  static List<SortBy> sortableBy = [
    SortBy.Name,
    SortBy.QualityReportStatus,
    SortBy.CleaningTaskStatus,
    SortBy.ServiceLeader,
    SortBy.DG,
    SortBy.UnfinishedSetup,
    SortBy.UnfinishedTasks
  ];

  sortableByList() => sortableBy;

  final _locationRepository = LocationRepository();
  final _locationSubject = BehaviorSubject<List<Location>>();
  Observable<List<Location>> get locations => _locationSubject.stream;

  //SortBy sortedBy = SortBy.Name;
  //List<Location> loadedLocations = List();

  LocationBlocScope scope;
  int customerId;
  int userId;
  LocationBloc({this.scope: LocationBlocScope.Locations, this.customerId: 0, this.userId: 0});

  //SortBy get beingSortedBy => sortedBy;

  fetch() async {
    switch (scope) {
      case LocationBlocScope.Locations:
        await _fetchLocations();
        break;
      case LocationBlocScope.LocationsWithoutPlan:
        await _fetchLocationsWithoutPlan();
        break;
      case LocationBlocScope.LocationsWithoutServiceLeader:
        await _fetchLocationsWithoutServiceLeader();
        break;
      case LocationBlocScope.LocationsWithoutStaff:
        await _fetchLocationsWithoutStaff();
        break;
    }
  }

/*
  sortBy(SortBy sortedBy) {
    if (sortableBy.contains(sortedBy)) {
      sortedBy = sortedBy;
      _addToSink(loadedLocations);
    }
  }
*/
  addToSinkSorted(List<Location> list) {
    loaded = list;

    list = List<Location>.from(list.where((c) {
      return c.name.toLowerCase().contains(filteringBy.toLowerCase());
    }).map((u) {
      u.smallData.tasks.delayed.include =
          cleaningTaskStatusOptions[WorkStatus.Overdue];
      u.smallData.tasks.critical.include =
          cleaningTaskStatusOptions[WorkStatus.Critical];
      u.smallData.tasks.okay.include =
          cleaningTaskStatusOptions[WorkStatus.Okay];
      u.smallData.tasks.unstarted.include =
          cleaningTaskStatusOptions[WorkStatus.Unstarted];
      return u;
    }).where((l) {
      if (!qualityReportStatusOptions[WorkStatus.Overdue] &&
          status(l.nextQualityReport) == WorkStatus.Overdue) return false;
      if (!qualityReportStatusOptions[WorkStatus.Critical] &&
          status(l.nextQualityReport) == WorkStatus.Critical) return false;
      if (!qualityReportStatusOptions[WorkStatus.Okay] &&
          status(l.nextQualityReport) == WorkStatus.Okay) return false;
      if (!qualityReportStatusOptions[WorkStatus.Unstarted] &&
          status(l.nextQualityReport) == WorkStatus.Unstarted) return false;
      return true;
    }));

    if (sortedBy == SortBy.Name)
      list.sort((l1, l2) => l1.name.compareTo(l2.name));

    if (sortedBy == SortBy.QualityReportStatus)
      list.sort((l1, l2) => l1.nextQualityReport == null
          ? 1
          : l2.nextQualityReport == null
              ? -1
              : l1.nextQualityReport.compareTo(l2.nextQualityReport));

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

    _locationSubject.sink.add(list);
    /*
    _locationSubject.sink.add(List<Location>.from(loaded.where((c) {
      return c.name.toLowerCase().contains(filteringBy.toLowerCase());
    })));*/
  }

  _fetchLocations() async {
    addToSinkSorted(
        await _locationRepository.fetchLocations(customerId, userId));
  }

  _fetchLocationsWithoutPlan() async {
    addToSinkSorted(await _locationRepository.fetchLocationsWithoutPlan(
        customerId, userId));
  }

  _fetchLocationsWithoutStaff() async {
    addToSinkSorted(await _locationRepository.fetchLocationsWithoutStaff(
        customerId, userId));
  }

  _fetchLocationsWithoutServiceLeader() async {
    addToSinkSorted(await _locationRepository
        .fetchLocationsWithoutServiceLeader(customerId));
  }

  dispose() {
    _locationSubject.close();
  }
}

enum LocationBlocScope {
  Locations,
  LocationsWithoutPlan,
  LocationsWithoutStaff,
  LocationsWithoutServiceLeader
}
