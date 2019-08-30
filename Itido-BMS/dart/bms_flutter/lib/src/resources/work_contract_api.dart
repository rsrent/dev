import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';
import 'package:http/http.dart' as http show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'client_controller.dart';
import 'query_client_controller.dart';

class WorkContractApi extends WorkContractSource {
  QueryClientController<WorkContract> _client;
  String path = '${api.path}/api/WorkContract';

  WorkContractApi({
    http.Client client,
  }) {
    _client = QueryClientController(
      converter: (json) => WorkContract.fromJson(json),
      client: client,
      getHeaders: () => api.headers(),
    );
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<QueryResult<bool>> addContract(int workContractId, int contractId) {
    return _client.put(
      '$path/AddContract/$workContractId/$contractId',
    );
  }

  @override
  Future<QueryResult<int>> createWorkContract(
      WorkContract workContract, int projectItemId) {
    return _client.postId(
      '$path/CreateForProjectItem/$projectItemId',
      body: workContract.toMap(),
    );
  }

  @override
  Future<QueryResult<WorkContract>> fetch(int workContractId) {
    return _client.get(
      '$path/Get/$workContractId',
    );
  }

  @override
  Future<QueryResult<List<WorkContract>>> fetchWorkContractsOfProjectItem(
      int projectItemId) {
    return _client.getMany(
      '$path/GetOfProjectItem/$projectItemId',
    );
  }

  @override
  Future<QueryResult<List<WorkContract>>> fetchWorkContractsOfUser(int userId) {
    return _client.getMany(
      '$path/GetWorkContractsOfUser/$userId',
    );
  }

  @override
  Future<QueryResult<bool>> removeContract(int workContractId) {
    return _client.put(
      '$path/RemoveContract/$workContractId',
    );
  }

  @override
  Future<QueryResult<bool>> updateWorkContract(WorkContract workContract) {
    return _client.put(
      '$path/Update',
      body: workContract.toMap(),
    );
  }
}
