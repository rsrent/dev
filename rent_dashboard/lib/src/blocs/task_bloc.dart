import 'sortable_bloc.dart';
import 'package:rxdart/rxdart.dart';
import '../models/task.dart';
import '../resources/task_repository.dart';
import '../models/sortable_by.dart';
import '../models/workstatus.dart';
import '../models/enums.dart';

class TaskBloc extends SortableBloc<Task> {
  final _taskRepository = TaskRepository();
  final _taskSubject = BehaviorSubject<List<Task>>();
  get tasks => _taskSubject.stream;

  int locationId;
  int customerId;
  int userId;
  TaskBloc({this.locationId: 0, this.customerId: 0, this.userId: 0})
  {
    qualityReportStatusOptions = null;
  }

  static List<SortBy> sortableBy = [
    SortBy.Name,
    SortBy.CleaningTaskStatus,
  ];

  sortableByList() => sortableBy;

  addToSinkSorted(List<Task> list)
  {
    loaded = list;

    list = List<Task>.from(list.where((t) {
      var text = '${t.area} ${t.customerName}';
      //print(text);
      return text.toLowerCase().contains(filteringBy.toLowerCase());
    }).map((c) {
      return c;
    }).where((t) {

      //return true;

      //print(t.status());
      //print(cleaningTaskStatusOptions[WorkStatus.Overdue]); 

      //print(t.planType);

      if(!cleaningTaskStatusOptions[t.status()])
        return false;
 
        if(!cleaningTaskPlanOptions[t.planType])
        return false;

      /*
      if(!cleaningTaskStatusOptions[WorkStatus.Overdue] && t.status() == WorkStatus.Overdue)
        return false;
      if(!cleaningTaskStatusOptions[WorkStatus.Critical] && t.status() == WorkStatus.Critical)
        return false;
      if(!cleaningTaskStatusOptions[WorkStatus.Okay] && t.status() == WorkStatus.Okay)
        return false;
      if(!cleaningTaskStatusOptions[WorkStatus.Unstarted] && t.status() == WorkStatus.Unstarted)
        return false; 
      
      if(!cleaningTaskPlanOptions[2] && t.planType == PlanType.Vinduer)
        return false;
      if(!cleaningTaskPlanOptions[3] && t.planType == PlanType.FanCoil)
        return false;
      if(!cleaningTaskPlanOptions[4] && t.planType == PlanType.Periodic)
        return false;
        */

      return true;
    }));

    if (sortedBy == SortBy.Name)
      list.sort((l1, l2) => l1.area.compareTo(l2.area)); 

    if (sortedBy == SortBy.CleaningTaskStatus)
      list.sort((t1, t2) { 
        //if(t1.nextTime == null)
          //print('t1 is null');
        //if(t2.nextTime == null)
          //print('t2 is null');
        return t2.nextTime == null ? -1 : t1.nextTime == null ? 1 : t1.nextTime.compareTo(t2.nextTime); });

    
    _taskSubject.sink.add(list);
  }

  fetchTasks() async {
    final tasks = await _taskRepository.fetchTasks(locationId, customerId, userId);
    addToSinkSorted(tasks);
    
  }

  dispose() {
    _taskSubject.close();
  }
}
