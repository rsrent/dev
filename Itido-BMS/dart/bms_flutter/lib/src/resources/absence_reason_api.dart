import 'package:bms_dart/models.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'client_controller.dart';

class AbsenceReasonApi extends AbsenceReasonSource {
  ClientController<AbsenceReason> _client;

  String path = '${api.path}/api/AbsenceReason';

  AbsenceReasonApi({
    http.Client client,
  }) {
    _client = ClientController<AbsenceReason>(
        converter: (json) => AbsenceReason.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<AbsenceReason>> fetchAllAbsenceReasons() {
    return _client.getMany(
      '$path/GetAll',
      headers: api.headers(),
    );
  }

  @override
  Future<int> createAbsenceReason(AbsenceReason absenceReason) {
    return _client.postId(
      '$path/Create',
      body: json.encode(
        absenceReason.toMap(),
      ),
      headers: api.headers(),
    );
  }

  @override
  Future<bool> updateAbsenceReason(AbsenceReason absenceReason) {
    return _client.put(
      '$path/Update',
      body: json.encode(
        absenceReason.toMap(),
      ),
      headers: api.headers(),
    );
  }
}
