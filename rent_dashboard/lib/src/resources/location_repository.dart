import 'dart:async';
import '../models/location.dart';
import 'location_api.dart';

typedef Future<List<Location>> ListFromSource(LocationSource s);

class LocationRepository extends LocationSource {
  List<LocationSource> sources = <LocationSource>[
    LocationApi(),
  ];


/*
  Future<List<Location>> _fetchLocations(int customerId, int userId) async {
    var locations;
    for (var source in sources) {
      locations = await source.fetchLocations(customerId, userId);
      if (locations != null) {
        break;
      }
    }
    return locations;
  }
*/
  


  Future<List<Location>> _fetchLocations(ListFromSource listFromSource) async {
    var locations;
    for (var source in sources) {
      locations = await listFromSource(source);
      if (locations != null) {
        break;
      }
    }
    return locations;
  }

  Future<List<Location>> fetchLocations(int customerId, int userId) async {
    return await _fetchLocations((s) => s.fetchLocations(customerId, userId));
  }
  Future<List<Location>> fetchLocationsWithoutPlan(int customerId, int userId) async {
    return await _fetchLocations((s) => s.fetchLocationsWithoutPlan(customerId, userId));
  }
  Future<List<Location>> fetchLocationsWithoutStaff(int customerId, int userId) async {
    return await _fetchLocations((s) => s.fetchLocationsWithoutStaff(customerId, userId));
  }
  Future<List<Location>> fetchLocationsWithoutServiceLeader(int customerId) async {
    return await _fetchLocations((s) => s.fetchLocationsWithoutServiceLeader(customerId));
  }
}

abstract class LocationSource {
  Future<List<Location>> fetchLocations(int customerId, int userId);
  Future<List<Location>> fetchLocationsWithoutPlan(int customerId, int userId);
  Future<List<Location>> fetchLocationsWithoutStaff(int customerId, int userId);
  Future<List<Location>> fetchLocationsWithoutServiceLeader(int customerId);
}
