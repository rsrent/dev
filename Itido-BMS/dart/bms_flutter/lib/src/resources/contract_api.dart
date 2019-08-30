import 'package:bms_dart/models.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'client_controller.dart';

class ContractApi extends ContractSource {
  ClientController<Contract> _client;

  String path = '${api.path}/api/Contract';

  ContractApi({
    http.Client client,
  }) {
    _client = ClientController(
        converter: (json) => Contract.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<Contract> fetchContract(int id) {
    return _client.get(
      '$path/Get/$id',
      headers: api.headers(),
    );
  }

  @override
  Future<List<Contract>> fetchContractsOfUser(int userId) {
    return _client.getMany(
      '$path/GetAllOfUser/$userId',
      headers: api.headers(),
    );
  }

  @override
  Future<bool> createContract(Contract contract, int userId, int agreementId) {
    return _client.postNoContent(
      '$path/Create/$userId/$agreementId',
      //body: contract.toMap(),
      body: json.encode(
        contract.toMap(),
      ),
      headers: api.headers(),
    );
  }

  @override
  Future<bool> updateContract(Contract contract) {
    return _client.put(
      '$path/Update',
      body: json.encode(
        contract.toMap(),
      ),
      headers: api.headers(),
    );
  }
}
