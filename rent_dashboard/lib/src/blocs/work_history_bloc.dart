
import 'package:rxdart/rxdart.dart';
import '../resources/work_history_repository.dart';

class WorkHistoryBloc {
  final _workHistoryRepository = WorkHistoryRepository();
  final _workHistorySubject = BehaviorSubject<Map<DateTime, List<int>>>();
  Observable<Map<DateTime, List<int>>> get workHistory => _workHistorySubject.stream;

  int locationId;
  int customerId;
  int userId;
  DateTime from;
  int daysBack;
  WorkHistoryBloc({this.locationId: 0, this.customerId: 0, this.userId: 0, this.from, this.daysBack: 5}) {
    if(from == null)
      from = DateTime.now();
  }

  fetch() async {
    final workHistorys = await _workHistoryRepository.fetchHistory(locationId, customerId, userId, from, daysBack);
    _workHistorySubject.sink.add(workHistorys);
  }

  dispose() {
    _workHistorySubject.close();
  }
}
