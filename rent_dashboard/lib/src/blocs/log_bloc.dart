import 'package:rxdart/rxdart.dart';
import '../models/log.dart';
import '../resources/log_repository.dart';

class LogBloc {
  final _logRepository = LogRepository();
  final _logSubject = BehaviorSubject<List<Log>>();
  get logs => _logSubject.stream;

  int locationId;
  int customerId;
  int userId;
  LogBloc({this.locationId: 0, this.customerId: 0, this.userId: 0});

  fetch() async {
    final logs = await _logRepository.fetchLogs(locationId, customerId, userId);
    _logSubject.sink.add(logs);
  }

  dispose() {
    _logSubject.close();
  }
}
