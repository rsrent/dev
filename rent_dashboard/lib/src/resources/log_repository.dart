import 'dart:async';
import 'log_api.dart';
import '../models/log.dart';

class LogRepository extends LogSource {
  List<LogSource> sources = <LogSource>[
    LogApi(),
  ];

  Future<List<Log>> fetchLogs(int locationId, int customerId, int userId) async {
    var logs;
    for (var source in sources) {
      logs = await source.fetchLogs(locationId, customerId, userId);
      if (logs != null) {
        break;
      }
    }
    return logs;
  }
}

abstract class LogSource {
  Future<List<Log>> fetchLogs(int locationId, int customerId, int userId);
}