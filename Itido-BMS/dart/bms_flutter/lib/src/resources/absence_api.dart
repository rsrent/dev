import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:bms_flutter/src/resources/query_client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class AbsenceApi extends AbsenceSource {
  //Client _client;

  QueryClientController<Absence> _client;
  String path = '${api.path}/api/Absence';

  AbsenceApi({
    http.Client client,
  }) {
    _client = QueryClientController<Absence>(
      converter: (json) => Absence.fromJson(json),
      client: client,
      getHeaders: () => api.headers(),
    );
  }

  @override
  void dispose() {
    _client.close();
  }

  // @override
  // Future<List<Absence>> fetchAllAbsences() {
  //   return _client.getMany(path, headers: api.headers());
  //   // if (response.statusCode == 200) {
  //   //   var result = json.decode(response.body);
  //   //   var list = List.castFrom(result);

  //   //   var absences = list.map((j) => Absence.fromJson(j)).toList();
  //   //   return absences;
  //   // }
  //   // return null;
  // }

  @override
  Future<QueryResult<List<Absence>>> fetchAbsencesOfUser(int userId) {
    return _client.getMany('$path/GetAllOfUser/$userId');
    // if (response.statusCode == 200) {
    //   var result = json.decode(response.body);
    //   var list = List.castFrom(result);

    //   var absences = list.map((j) => Absence.fromJson(j)).toList();
    //   return absences;
    // }
    // return null;
  }

  @override
  Future<QueryResult<int>> createAbsence(
      Absence absence, int userId, int absenceReasonId, bool isRequest) {
    return _client.postId(
      '$path/Create/$userId/$absenceReasonId/$isRequest',
      body: json.encode(
        absence.toMap(),
      ),
    );
  }

  @override
  Future<QueryResult<bool>> updateAbsence(Absence absence) {
    return _client.put(
      '$path/Update',
      body: json.encode(
        absence.toMap(),
      ),
    );
    // if (response.statusCode == 204) {
    //   return true;
    // }
    // return false;
  }

  @override
  Future<QueryResult<bool>> replyToAbsence(int absenceId, bool isApproved) {
    return _client.put(
      '$path/Reply/$absenceId/$isApproved',
    );
    // if (response.statusCode == 204) {
    //   return true;
    // }
    // return false;
  }

  @override
  Future<QueryResult<Absence>> fetch(int absenceId) {
    return _client.get(
      '$path/$absenceId',
    );
  }
}
