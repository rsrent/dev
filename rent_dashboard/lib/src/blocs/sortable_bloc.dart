import '../models/sortable_by.dart';
import '../models/workstatus.dart';
import '../models/enums.dart';

abstract class SortableBloc<T> {
  List<SortBy> sortableByList();
  String filteringBy = '';
  SortBy sortedBy = SortBy.Name;
  SortBy get beingSortedBy => sortedBy;

  List<T> loaded = List();

  sortBy(SortBy sb) {
    if (sortableByList().contains(sb)) {
      sortedBy = sb;
      addToSinkSorted(loaded);
    }
  }

  filterBy(String text) {
    filteringBy = text;
    addToSinkSorted(loaded);
  }




  Map<WorkStatus, bool> qualityReportStatusOptions = {
    WorkStatus.Unstarted: true,
    WorkStatus.Okay: true,
    WorkStatus.Critical: true,
    WorkStatus.Overdue: true,
  };
  Map<WorkStatus, bool> cleaningTaskStatusOptions = {
    WorkStatus.Unstarted: true,
    WorkStatus.Okay: true,
    WorkStatus.Critical: true,
    WorkStatus.Overdue: true,
  };
  Map<PlanType, bool> cleaningTaskPlanOptions = {PlanType.Normal: true, PlanType.Periodic: true, PlanType.FanCoil: true, PlanType.Vinduer: true, };

  Map<WorkStatus, bool> updateQualityReportStatusOptions(WorkStatus s, bool b) {
    if(qualityReportStatusOptions == null) 
      return null;

    if (s != null) {
      var val = qualityReportStatusOptions[s];
      print(val);
      //qualityReportStatusOptions[s] = !val;
      qualityReportStatusOptions[s] = b;
      addToSinkSorted(loaded);
    }
    return qualityReportStatusOptions;
  }

  Map<WorkStatus, bool> updateCleaningTaskStatusOptions(WorkStatus s, bool b) {
    if(cleaningTaskStatusOptions == null) 
      return null;

    if (s != null) {
      cleaningTaskStatusOptions[s] = b;
      addToSinkSorted(loaded);
    }
    return cleaningTaskStatusOptions;
  }

  Map<PlanType, bool> updateCleaningTaskPlanOptions(PlanType p, bool b) {
    if(cleaningTaskPlanOptions == null) 
      return null;

    if (p != null) {
      cleaningTaskPlanOptions[p] = b;
      addToSinkSorted(loaded);
    }
    return cleaningTaskPlanOptions; 
  }



  addToSinkSorted(List<T> list);
}