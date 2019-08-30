import 'package:bms_dart/models.dart';
import 'package:http/http.dart' show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'client_controller.dart';

class LocationApi extends LocationSource {
  ClientController<Location> _client;

  String path = '${api.path}/api/Location';

  LocationApi({
    Client client,
  }) {
    _client = ClientController(
      converter: (json) => Location.fromJson(json),
      client: client,
      getHeaders: () => api.headers(),
    );
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<Location> fetch(int locationId) {
    return _client.get(
      '$path/$locationId',
      headers: api.headers(),
    );
  }

  @override
  Future<List<Location>> fetchAllLocations() {
    return _client.getMany(
      path,
    );
  }

  @override
  Future<List<Location>> fetchAllLocationsOfCustomer(int customerId) {
    return _client.getMany(
      '$path/GetForCustomer/$customerId',
    );
  }

  @override
  Future<bool> addUserToLocations(int userId, List<int> locationIds) {
    return _client.put(
      '$path/AddUserToLocations/$userId',
      body: locationIds,
    );
  }

  @override
  Future<bool> removeUserFromLocations(int userId, List<int> locationIds) {
    return _client.put(
      '$path/RemoveUserFromLocations/$userId',
      body: locationIds,
    );
  }

  @override
  Future<List<Location>> fetchAllLocationsOfUser(int userId) {
    return _client.getMany(
      '$path/GetUsersLocations/$userId',
    );
  }

  @override
  Future<List<Location>> fetchAllLocationsNotOfUser(int userId) {
    return _client.getMany(
      '$path/GetNotUsersLocations/$userId',
    );
  }

  @override
  Future<int> create(Location location, int customerId) {
    return _client.postId(
      '$path/Create/$customerId',
      body: location.toMap(),
    );
  }

  @override
  Future<bool> update(Location location, int locationId) {
    return _client.put(
      '$path/Update/$locationId',
      body: location.toMap(),
    );
  }

  @override
  Future<bool> enable(int id) => _client.put('$path/Enable/$id');
  @override
  Future<bool> disable(int id) => _client.put('$path/Disable/$id');
}
