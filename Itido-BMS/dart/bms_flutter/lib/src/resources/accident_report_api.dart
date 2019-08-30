import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class AccidentReportApi extends AccidentReportSource {
  //Client _client;

  ClientController<AccidentReport> _client;
  String path = '${api.path}/api/AccidentReport';

  AccidentReportApi({
    http.Client client,
  }) {
    _client = ClientController<AccidentReport>(
        converter: (json) => AccidentReport.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<AccidentReport>> fetchNew() {
    return _client.getMany('$path/GetNew', headers: api.headers());
  }

  @override
  Future<int> create(AccidentReport post, int userId) {
    return _client.postId(
      '$path/Create/$userId',
      headers: api.headers(),
      body: post.toMap(),
    );
  }

  @override
  Future<List<AccidentReport>> fetchAccidentReportsOfUser(int userId) {
    return _client.getMany('$path/GetAllOfUser/$userId',
        headers: api.headers());
  }

  @override
  Future<bool> replyToAccidentReport(int accidentReportId, bool isApproved) {
    return _client.put(
      '$path/Reply/$accidentReportId/$isApproved',
      headers: api.headers(),
    );
  }
}
