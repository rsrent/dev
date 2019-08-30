import 'dart:async';
import '../models/contract.dart';
import 'source.dart';

abstract class ContractSource extends Source {
  Future<Contract> fetchContract(int id);
  Future<List<Contract>> fetchContractsOfUser(int userId);
  Future<bool> createContract(Contract contract, int userId, int agreementId);
  Future<bool> updateContract(Contract contract);
}

class ContractRepository extends ContractSource {
  final List<ContractSource> sources;

  ContractRepository(this.sources);

  Future<Contract> fetchContract(int id) async {
    var result;
    for (var source in sources) {
      result = await source.fetchContract(id);
      if (result != null) {
        break;
      }
    }
    return result;
  }

  Future<List<Contract>> fetchContractsOfUser(int userId) async {
    var contracts;
    for (var source in sources) {
      contracts = await source.fetchContractsOfUser(userId);
      if (contracts != null) {
        break;
      }
    }
    return contracts;
  }

  Future<bool> createContract(Contract contract, int userId, int agreementId) =>
      sources[0].createContract(contract, userId, agreementId);

  Future<bool> updateContract(Contract contract) =>
      sources[0].updateContract(contract);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
