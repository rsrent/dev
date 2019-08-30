import 'dart:async';

import 'package:bms_dart/query_result.dart';
import 'package:meta/meta.dart';

import '../models/work_contract.dart';
import 'source.dart';

abstract class WorkContractSource extends Source {
  Future<QueryResult<int>> createWorkContract(
      WorkContract workContract, int projectItemId);
  Future<QueryResult<bool>> updateWorkContract(WorkContract workContract);

  Future<QueryResult<WorkContract>> fetch(int workContractId);
  Future<QueryResult<List<WorkContract>>> fetchWorkContractsOfUser(int userId);
  Future<QueryResult<List<WorkContract>>> fetchWorkContractsOfProjectItem(
      int projectItemId);

  Future<QueryResult<bool>> addContract(int workContractId, int contractId);
  Future<QueryResult<bool>> removeContract(int workContractId);
}

class WorkContractRepository extends WorkContractSource {
  final List<WorkContractSource> sources;

  WorkContractRepository(this.sources);

  Future<QueryResult<WorkContract>> fetch(int workContractId) async {
    var items;
    for (var source in sources) {
      items = await source.fetch(workContractId);
      if (items != null) {
        break;
      }
    }
    return items;
  }

  Future<QueryResult<List<WorkContract>>> fetchWorkContractsOfUser(
      int userId) async {
    var items;
    for (var source in sources) {
      items = await source.fetchWorkContractsOfUser(userId);
      if (items != null) {
        break;
      }
    }
    return items;
  }

  Future<QueryResult<List<WorkContract>>> fetchWorkContractsOfProjectItem(
      int projectItemId) async {
    var items;
    for (var source in sources) {
      items = await source.fetchWorkContractsOfProjectItem(projectItemId);
      if (items != null) {
        break;
      }
    }
    return items;
  }

  Future<QueryResult<int>> createWorkContract(
          WorkContract workContract, int projectItemId) =>
      sources[0].createWorkContract(workContract, projectItemId);

  Future<QueryResult<bool>> updateWorkContract(WorkContract workContract) =>
      sources[0].updateWorkContract(workContract);

  Future<QueryResult<bool>> addContract(int workContractId, int contractId) =>
      sources[0].addContract(workContractId, contractId);

  Future<QueryResult<bool>> removeContract(int workContractId) =>
      sources[0].removeContract(workContractId);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
