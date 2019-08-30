import 'sortable_bloc.dart';

import 'package:rxdart/rxdart.dart';
import '../models/customer.dart';
import '../resources/customer_repository.dart';
import '../models/sortable_by.dart';
import '../models/workstatus.dart';

class CustomerBloc extends CustomerSource with SortableBloc<Customer> {
  final _customerRepository = CustomerRepository();
  final _customerSubject = BehaviorSubject<List<Customer>>();
  Observable<List<Customer>> get customers => _customerSubject.stream;

  CustomerBloc() {
    cleaningTaskPlanOptions = null;
  }
  
  static List<SortBy> sortableBy = [
    SortBy.Name,
    SortBy.QualityReportStatus,
    SortBy.CleaningTaskStatus,
    SortBy.DG,
    SortBy.UnfinishedSetup,
    SortBy.UnfinishedTasks
  ];
  sortableByList() => sortableBy;

  addToSinkSorted(List<Customer> list) {
    loaded = list;

    list = List<Customer>.from(list.where((c) {
      return c.name.toLowerCase().contains(filteringBy.toLowerCase());
    }).map((c) {
      c.smallData.reports.delayed.include = qualityReportStatusOptions[WorkStatus.Overdue];
      c.smallData.reports.critical.include = qualityReportStatusOptions[WorkStatus.Critical];
      c.smallData.reports.okay.include = qualityReportStatusOptions[WorkStatus.Okay];
      c.smallData.reports.unstarted.include = qualityReportStatusOptions[WorkStatus.Unstarted];

      c.smallData.tasks.delayed.include = cleaningTaskStatusOptions[WorkStatus.Overdue];
      c.smallData.tasks.critical.include = cleaningTaskStatusOptions[WorkStatus.Critical];
      c.smallData.tasks.okay.include = cleaningTaskStatusOptions[WorkStatus.Okay];
      c.smallData.tasks.unstarted.include = cleaningTaskStatusOptions[WorkStatus.Unstarted];
      return c;
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

    _customerSubject.sink.add(list);
  }

  int inspected = 0;

  fetchCustomers() async {
    final customers = await _customerRepository.fetchCustomers();
    addToSinkSorted(customers);
    //_customerSubject.sink.add(customers);
  }

  dispose() {
    _customerSubject.close();
  }
}
