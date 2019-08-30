import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class AddressApi extends AddressSource {
  ClientController<Address> _client;
  String path = '${api.path}/api/Address';

  AddressApi({
    http.Client client,
  }) {
    _client = ClientController<Address>(
        converter: (json) => Address.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<bool> update(int addressId, Address address) {
    return _client.put(
      '$path/Update/$addressId',
      body: address.toMap(),
    );
  }

  @override
  Future<Address> fetch(int addressId) {
    return _client.get('$path/Get/$addressId');
  }
}
