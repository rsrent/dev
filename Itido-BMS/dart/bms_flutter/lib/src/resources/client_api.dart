import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class ClientApi extends ClientSource {
  //Client _client;

  ClientController<Client> _client;
  String path = '${api.path}/api/Client';

  ClientApi({
    http.Client client,
  }) {
    _client = ClientController<Client>(
        converter: (json) => Client.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<Client>> fetchClients() {
    return _client.getMany('$path/GetClients', headers: api.headers());
  }
}
