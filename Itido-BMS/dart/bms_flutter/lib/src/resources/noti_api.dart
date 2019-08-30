import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class NotiApi extends NotiSource {
  //Client _client;

  ClientController<Noti> _client;
  String path = '${api.path}/api/Noti';

  NotiApi({
    http.Client client,
  }) {
    _client = ClientController<Noti>(
        converter: (json) => Noti.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<Noti>> fetchLatestNotis(int count) {
    return _client.getMany('$path/GetLatest/$count', headers: api.headers());
  }

  @override
  Future<bool> setNotiSeen(int id) {
    return _client.put(
      '$path/SetSeen/$id',
      headers: api.headers(),
    );
  }
}
