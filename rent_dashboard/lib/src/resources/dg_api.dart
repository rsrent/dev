import 'package:http/http.dart' show Client;
import 'dart:convert';
import 'dart:async';
import 'dg_repository.dart';
import '../network.dart';

class DgApi extends DgSource {
  Client client = Client();

  Future<double> fetch(int locationId, int customerId, int userId) async {
    final response = await client.get(
      '${Network.root}/Dashboard/Dg/$locationId/$customerId/$userId',
      headers: Network.getHeaders(),
    );
    final dg = json.decode(response.body);
    if (dg == "NaN") return 0.0;
    return dg;
  }
}
