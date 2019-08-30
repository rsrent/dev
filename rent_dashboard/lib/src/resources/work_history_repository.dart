import 'dart:async';
import 'work_history_api.dart';

class WorkHistoryRepository extends WorkHistorySource {
  List<WorkHistorySource> sources = <WorkHistorySource>[
    WorkHistoryApi(),
  ];

  Future<Map<DateTime, List<int>>> fetchHistory(int locationId, int customerId,
      int userId, DateTime from, int daysBack) async {
    var map;
    for (var source in sources) {
      map = await source.fetchHistory(
          locationId, customerId, userId, from, daysBack);
      if (map != null) {
        break;
      }
    }
    return map;
  }
}

abstract class WorkHistorySource {
  Future<Map<DateTime, List<int>>> fetchHistory(
      int locationId, int customerId, int userId, DateTime from, int daysBack);
}
