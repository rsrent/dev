import 'package:bms_dart/models.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'client_controller.dart';

class AgreementApi extends AgreementSource {
  ClientController<Agreement> _client;

  String path = '${api.path}/api/Agreement';

  AgreementApi({
    http.Client client,
  }) {
    _client = ClientController<Agreement>(
        converter: (json) => Agreement.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<Agreement>> fetchAllAgreements() {
    return _client.getMany(
      '$path/GetAll',
      headers: api.headers(),
    );
  }

  @override
  Future<int> createAgreement(Agreement agreement) {
    return _client.postId(
      '$path/Create',
      body: json.encode(
        agreement.toMap(),
      ),
      headers: api.headers(),
    );
  }

  @override
  Future<bool> updateAgreement(Agreement agreement) {
    return _client.put(
      '$path/Update',
      body: json.encode(
        agreement.toMap(),
      ),
      headers: api.headers(),
    );
  }
}
