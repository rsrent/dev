import 'dart:async';

import 'package:bms_dart/query_result.dart';
import 'package:meta/meta.dart';

import '../models/work.dart';
import 'source.dart';

abstract class WorkSource extends Source {
  Future<QueryResult<List<Work>>> fetchWorks(
      {@required DateTime from, @required DateTime to});
  Future<QueryResult<List<Work>>> fetchWorksOfSignedInUser();
  Future<QueryResult<List<Work>>> fetchWorksOfUser(
      {@required int userId, @required DateTime from, @required DateTime to});
  Future<QueryResult<List<Work>>> fetchWorksOfProjectItem(
      {@required int projectItemId,
      @required DateTime from,
      @required DateTime to});
  Future<QueryResult<int>> createWork(Work work, int projectItemId);
  Future<QueryResult<bool>> updateWork(Work work);

  Future<QueryResult<bool>> addContract(int workId, int contractId);
  // Future<QueryResult<bool> register(int workId, int startTimeMins, int endTimeMins);
  Future<QueryResult<bool>> replace(int workId, int contractId);
  Future<QueryResult<bool>> removeContract(int workId);
  Future<QueryResult<bool>> removeReplacer(int workId);

  Future<QueryResult<bool>> register(int workId,
      {int startTimeMins, int endTimeMins});

  Future<QueryResult<bool>> inviteContractToWork(int workId, int contractId);
  Future<QueryResult<bool>> replyToWorkInvite(int workId, bool answer);
}

class WorkRepository extends WorkSource {
  final List<WorkSource> sources;

  WorkRepository(this.sources);

  Future<QueryResult<List<Work>>> fetchWorks(
      {@required DateTime from, @required DateTime to}) async {
    var items;
    for (var source in sources) {
      items = await source.fetchWorks(from: from, to: to);
      if (items != null) {
        break;
      }
    }
    return items;
  }

  Future<QueryResult<List<Work>>> fetchWorksOfUser(
      {@required int userId,
      @required DateTime from,
      @required DateTime to}) async {
    var items;
    for (var source in sources) {
      items = await source.fetchWorksOfUser(userId: userId, from: from, to: to);
      if (items != null) {
        break;
      }
    }
    return items;
  }

  Future<QueryResult<List<Work>>> fetchWorksOfSignedInUser() async {
    var items;
    for (var source in sources) {
      items = await source.fetchWorksOfSignedInUser();
      if (items != null) {
        break;
      }
    }
    return items;
  }

  Future<QueryResult<List<Work>>> fetchWorksOfProjectItem(
      {@required int projectItemId,
      @required DateTime from,
      @required DateTime to}) async {
    var items;
    for (var source in sources) {
      items = await source.fetchWorksOfProjectItem(
          projectItemId: projectItemId, from: from, to: to);
      if (items != null) {
        break;
      }
    }
    return items;
  }

  Future<QueryResult<int>> createWork(Work work, int projectItemId) =>
      sources[0].createWork(work, projectItemId);

  Future<QueryResult<bool>> updateWork(Work work) =>
      sources[0].updateWork(work);

  Future<QueryResult<bool>> addContract(int workId, int contractId) =>
      sources[0].addContract(workId, contractId);
  // Future<QueryResult<bool> register(int workId, int startTimeMins, int endTimeMins) =>
  //     sources[0].register(workId, startTimeMins, endTimeMins);
  Future<QueryResult<bool>> replace(int workId, int contractId) =>
      sources[0].replace(workId, contractId);

  Future<QueryResult<bool>> removeContract(int workId) =>
      sources[0].removeContract(workId);
  Future<QueryResult<bool>> removeReplacer(int workId) =>
      sources[0].removeReplacer(workId);

  Future<QueryResult<bool>> register(int workId,
          {int startTimeMins, int endTimeMins}) =>
      sources[0].register(workId,
          startTimeMins: startTimeMins, endTimeMins: endTimeMins);

  Future<QueryResult<bool>> inviteContractToWork(int workId, int contractId) =>
      sources[0].inviteContractToWork(workId, contractId);
  Future<QueryResult<bool>> replyToWorkInvite(int workId, bool answer) =>
      sources[0].replyToWorkInvite(workId, answer);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
