import 'dart:async';

import 'package:bms_dart/query_result.dart';
import 'package:meta/meta.dart';

import '../models/work.dart';
import 'source.dart';

abstract class WorkRegistrationSource extends Source {
  Future<QueryResult<bool>> register(int workId,
      {int startTimeMins, int endTimeMins});
}

class WorkRegistrationRepository extends WorkRegistrationSource {
  final List<WorkRegistrationSource> sources;

  WorkRegistrationRepository(this.sources);

  Future<QueryResult<bool>> register(int workId,
          {int startTimeMins, int endTimeMins}) =>
      sources[0].register(workId,
          startTimeMins: startTimeMins, endTimeMins: endTimeMins);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
