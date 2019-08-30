import 'dart:async';
import '../models/location.dart';
import 'source.dart';

abstract class LocationSource extends Source {
  Future<Location> fetch(int locationId);
  Future<List<Location>> fetchAllLocations();
  Future<List<Location>> fetchAllLocationsOfCustomer(int customerId);

  Future<List<Location>> fetchAllLocationsOfUser(int userId);
  Future<List<Location>> fetchAllLocationsNotOfUser(int userId);
  Future<bool> addUserToLocations(int userId, List<int> locationIds);
  Future<bool> removeUserFromLocations(int userId, List<int> locationIds);

  Future<int> create(Location location, int customerId);
  Future<bool> update(Location location, int locationId);

  Future<bool> disable(int locationId);
  Future<bool> enable(int locationId);
}

class LocationRepository extends LocationSource {
  final List<LocationSource> sources;

  LocationRepository(this.sources);

  Future<Location> fetch(int locationId) async {
    var location;
    for (var source in sources) {
      location = await source.fetch(locationId);
      if (location != null) {
        break;
      }
    }
    return location;
  }

  Future<List<Location>> fetchAllLocations() async {
    var locations;
    for (var source in sources) {
      locations = await source.fetchAllLocations();
      if (locations != null) {
        break;
      }
    }
    return locations;
  }

  Future<List<Location>> fetchAllLocationsOfCustomer(int customerId) async {
    var locations;
    for (var source in sources) {
      locations = await source.fetchAllLocationsOfCustomer(customerId);
      if (locations != null) {
        break;
      }
    }
    return locations;
  }

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }

  @override
  Future<bool> addUserToLocations(int userId, List<int> locationIds) =>
      sources[0].addUserToLocations(userId, locationIds);

  @override
  Future<bool> removeUserFromLocations(int userId, List<int> locationIds) =>
      sources[0].removeUserFromLocations(userId, locationIds);

  @override
  Future<List<Location>> fetchAllLocationsNotOfUser(int userId) async {
    var locations;
    for (var source in sources) {
      locations = await source.fetchAllLocationsNotOfUser(userId);
      if (locations != null) {
        break;
      }
    }
    return locations;
  }

  @override
  Future<List<Location>> fetchAllLocationsOfUser(int userId) async {
    var locations;
    for (var source in sources) {
      locations = await source.fetchAllLocationsOfUser(userId);
      if (locations != null) {
        break;
      }
    }
    return locations;
  }

  @override
  Future<int> create(Location location, int customerId) =>
      sources[0].create(location, customerId);

  @override
  Future<bool> update(Location location, int locationId) =>
      sources[0].update(location, locationId);

  Future<bool> disable(int locationId) => sources[0].disable(locationId);
  Future<bool> enable(int locationId) => sources[0].enable(locationId);
}
