import 'package:http/http.dart' show Client;
import 'dart:convert';
import '../models/morework.dart';
import 'dart:async';
import 'morework_repository.dart';
import '../network.dart';

class MoreWorkApi extends MoreWorkSource {
  Client client = Client();

  Future<List<MoreWork>> fetchMoreWorks() async {
    final response = await client.get(
      '${Network.root}/Dashboard/MoreWorks',
      headers: Network.getHeaders(),
    );
    final moreworks = json.decode(response.body);
    return List.from(moreworks.map((j) {
      return MoreWork.fromJson(j);
    }));
  }
}
