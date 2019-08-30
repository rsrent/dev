import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/query_client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'client_controller.dart';
import 'package:bms_dart/query_result.dart';

class WorkApi extends WorkSource {
  //ClientController<Work> _client;
  QueryClientController<Work> _client;
  String path = '${api.path}/api/Work';
  String workInvitePath = '${api.path}/api/WorkInvitation';

  WorkApi({
    http.Client client,
  }) {
    // _client = ClientController(
    //   converter: (json) => Work.fromJson(json),
    //   client: client,
    //   getHeaders: () => api.headers(),
    // );

    _client = QueryClientController(
      converter: (json) => Work.fromJson(json),
      client: client,
      getHeaders: () => api.headers(),
    );
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<QueryResult<bool>> addContract(int workId, int contractId) {
    return _client.put(
      '$path/AddContract/$workId/$contractId',
    );
  }

  @override
  Future<QueryResult<int>> createWork(Work work, int projectItemId) {
    return _client.postId(
      '$path/CreateForProjectItem/$projectItemId',
      body: work.toMap(),
    );
  }

  @override
  Future<QueryResult<List<Work>>> fetchWorks({DateTime from, DateTime to}) {
    return _client.getMany(
      '$path/GetWork' +
          (from != null ? '/${from.toString()}/${to.toString()}' : ''),
    );
  }

  @override
  Future<QueryResult<List<Work>>> fetchWorksOfProjectItem({
    int projectItemId,
    DateTime from,
    DateTime to,
  }) {
    return _client.getMany(
      '$path/GetOfProjectItem/$projectItemId' +
          (from != null ? '/${from.toString()}/${to.toString()}' : ''),
    );
  }

  @override
  Future<QueryResult<List<Work>>> fetchWorksOfSignedInUser() {
    return _client.getMany(
      '$path/GetWorkOfSignedInUser',
    );
  }

  @override
  Future<QueryResult<List<Work>>> fetchWorksOfUser(
      {int userId, DateTime from, DateTime to}) {
    return _client.getMany(
      '$path/GetWorkOfUser/$userId' +
          (from != null ? '/${from.toString()}/${to.toString()}' : ''),
    );
  }

  @override
  Future<QueryResult<bool>> register(int workId,
      {int startTimeMins, int endTimeMins}) {
    return _client.put(
      '$path/RegisterWork/$workId' +
          (startTimeMins != null ? '/$startTimeMins/$endTimeMins' : ''),
    );
  }

  @override
  Future<QueryResult<bool>> removeContract(int workId) {
    return _client.put(
      '$path/RemoveContract/$workId',
    );
  }

  @override
  Future<QueryResult<bool>> removeReplacer(int workId) {
    return _client.put(
      '$path/RemoveReplacer/$workId',
    );
  }

  @override
  Future<QueryResult<bool>> replace(int workId, int contractId) {
    return _client.put(
      '$path/AddReplacer/$workId/$contractId',
    );
  }

  @override
  Future<QueryResult<bool>> updateWork(Work work) {
    return _client.put(
      '$path/Update',
      body: work.toMap(),
    );
  }

  @override
  Future<QueryResult<bool>> inviteContractToWork(int workId, int contractId) {
    return _client.postNoContent('$workInvitePath/Create/$workId/$contractId');
  }

  @override
  Future<QueryResult<bool>> replyToWorkInvite(int workId, bool answer) {
    return _client.put('$workInvitePath/Update/$workId/$answer');
  }
}
