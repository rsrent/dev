import 'dart:async';
import '../models/log.dart';
import 'source.dart';

abstract class LogSource extends Source {
  Future<Log> fetch(int logId);
  Future<List<Log>> fetchOfProjectItem(int projectItemId);
  Future<int> createLog(int projectItemId);
  Future<bool> updateLog(int logId, Log log);
}

class LogRepository extends LogSource {
  final List<LogSource> sources;

  LogRepository(this.sources);

  Future<Log> fetch(int logId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(logId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<Log>> fetchOfProjectItem(int projectItemId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfProjectItem(projectItemId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<int> createLog(int projectItemId) =>
      sources[0].createLog(projectItemId);
  Future<bool> updateLog(int logId, Log log) =>
      sources[0].updateLog(logId, log);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
