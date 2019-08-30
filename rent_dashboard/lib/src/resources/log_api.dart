import 'package:http/http.dart' show Client;
import 'dart:convert';
import '../models/log.dart';
import 'dart:async';
import 'log_repository.dart';
import '../network.dart';

class LogApi extends LogSource {
  Client client = Client();

  Future<List<Log>> fetchLogs(int locationId, int customerId, int userId) async {
    final response = await client.get(
      '${Network.root}/Dashboard/Logs/$locationId/$customerId/$userId',
      headers: Network.getHeaders(),
    );
    final logs = json.decode(response.body);
    return List.from(logs.map((j) {
      return Log.fromJson(j);
    }));
  }
}
