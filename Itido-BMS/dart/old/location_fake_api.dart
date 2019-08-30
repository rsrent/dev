import 'package:bms_dart/models.dart';
import 'dart:async';
import 'package:bms_dart/repositories.dart';
import 'client_faker.dart';

class LocationFakeApi extends LocationSource {
  ClientFaker _faker = ClientFaker<Location>(
      generator: (i) => Location(
            id: i,
            name: '323 Hellerup',
          ));

  @override
  void dispose() {}

  @override
  Future<List<Location>> fetchAllLocations() => _faker.getMany();

  @override
  Future<List<Location>> fetchAllLocationsOfCustomer(int customerId) =>
      _faker.getMany();

  @override
  Future<bool> addUserToLocations(int userId, List<int> locationIds) {
    // TODO: implement addUserToLocations
    return null;
  }

  @override
  Future<List<Location>> fetchAllLocationsNotOfUser(int userId) {
    // TODO: implement fetchAllLocationsNotOfUser
    return null;
  }

  @override
  Future<List<Location>> fetchAllLocationsOfUser(int userId) {
    // TODO: implement fetchAllLocationsOfUser
    return null;
  }

  @override
  Future<bool> removeUserFromLocations(int userId, List<int> locationIds) {
    // TODO: implement removeUserFromLocations
    return null;
  }

  @override
  Future<Location> fetch(int locationId) {
    // TODO: implement fetch
    return null;
  }

  @override
  Future<int> create(Location location, int customerId) {
    // TODO: implement create
    return null;
  }

  @override
  Future<bool> update(Location location, int locationId) {
    // TODO: implement update
    return null;
  }

  @override
  Future<bool> disable(int locationId) {
    // TODO: implement disable
    return null;
  }

  @override
  Future<bool> enable(int locationId) {
    // TODO: implement enable
    return null;
  }
}
