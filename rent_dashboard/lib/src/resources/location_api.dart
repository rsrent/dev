import 'package:http/http.dart' show Client;
import 'dart:async';
import '../models/location.dart';
import 'location_repository.dart';
import 'dart:convert';
import '../network.dart';

class LocationApi extends LocationSource {
  Client client = Client();

  Future<List<Location>> fetchLocations(int customerId, int userId) async {
    return await _fetchLocations('Locations/$customerId/$userId');
  }

  Future<List<Location>> fetchLocationsWithoutPlan(
      int customerId, int userId) async {
    return await _fetchLocations('LocationsWithoutPlan/$customerId/$userId');
  }

  Future<List<Location>> fetchLocationsWithoutStaff(
      int customerId, int userId) async {
    return await _fetchLocations('LocationsWithoutStaff/$customerId/$userId');
  }

  Future<List<Location>> fetchLocationsWithoutServiceLeader(
      int customerId) async {
    return await _fetchLocations('LocationsWithoutServiceLeader/$customerId');
  }

  Future<List<Location>> _fetchLocations(String route) async {
    final response = await client.get(
      '${Network.root}/Dashboard/$route',
      headers: Network.getHeaders(),
    );
    final locations = json.decode(response.body);
    return List.from(locations.map((j) {
      return Location.fromJson(j);
    }));
  }
}
