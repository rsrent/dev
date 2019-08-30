import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class LogApi extends LogSource {
  //Client _client;

  ClientController<Log> _client;
  String path = '${api.path}/api/Log';

  LogApi({
    http.Client client,
  }) {
    _client = ClientController<Log>(
        converter: (json) => Log.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<bool> updateLog(int logId, Log log) {
    return _client.put(
      '$path/$logId',
      body: log.toMap(),
    );
  }

  @override
  Future<Log> fetch(int logId) {
    return _client.get('$path/$logId');
  }

  @override
  Future<int> createLog(int projectItemId) {
    return _client.postId(
      '$path/CreateForProjectItem/$projectItemId',
    );
  }

  @override
  Future<List<Log>> fetchOfProjectItem(int projectItemId) {
    return _client.getMany('$path/GetOfProjectItem/$projectItemId');
  }

  // @override
  // Future<List<Log>> fetchLatestLogs(int count) {
  //   return _client.getMany('$path/GetLatest/$count', headers: api.headers());
  // }

  // @override
  // Future<int> createLog(Log post) {
  //   return _client.postId(
  //     '$path/Create',
  //     headers: api.headers(),
  //     body: post.toMap(),
  //   );
  // }
}
