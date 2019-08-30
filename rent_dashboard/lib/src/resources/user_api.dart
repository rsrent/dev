import 'package:http/http.dart' show Client;
import 'dart:convert';
import 'dart:async';
import 'user_repository.dart';
import '../models/user.dart';
import '../network.dart';

class UserApi extends UserSource {
  Client client = Client();

  Future<List<ServiceLeader>> fetchServiceLeaders() async {
    final response = await client.get(
      '${Network.root}/Dashboard/Users',
      headers: Network.getHeaders(),
    );
    if (response.statusCode == 200) {
      var jsons = json.decode(response.body);
      return List.from(jsons.map((j) {
        return ServiceLeader.fromJson(j);
      }));
    }
    return null;
  }
}
