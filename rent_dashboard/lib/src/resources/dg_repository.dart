import 'dart:async';
import 'dg_api.dart';

class DgRepository extends DgSource {
  List<DgSource> sources = <DgSource>[
    DgApi(),
  ];

  Future<double> fetch(
      int locationId, int customerId, int userId) async {
    var map;
    for (var source in sources) {
      map = await source.fetch(locationId, customerId, userId);
      if (map != null) {
        break;
      }
    }
    return map;
  }
}

abstract class DgSource {
  Future<double> fetch(
      int locationId, int customerId, int userId);
}
