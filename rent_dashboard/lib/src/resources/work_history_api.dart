import 'package:http/http.dart' show Client;
import 'dart:convert';
import 'dart:async';
import 'work_history_repository.dart';
import '../network.dart';

class WorkHistoryApi extends WorkHistorySource {
  Client client = Client();

  Future<Map<DateTime, List<int>>> fetchHistory(int locationId, int customerId, int userId, DateTime from, int daysBack) async {
    final response = await client.get(
      '${Network.root}/Dashboard/WorkHistory/$locationId/$customerId/$userId/$from/$daysBack',
      headers: Network.getHeaders(),
    );
    final map = json.decode(response.body); 

    Map<DateTime, List<int>> resultMap = Map();
    map.forEach((k,v) {
      resultMap[DateTime.parse(k)] = List<int>.from(v);
    });

    return resultMap;
  }
}
