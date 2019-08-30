import 'dart:async';
import '../models/address.dart';
import 'source.dart';

abstract class AddressSource extends Source {
  Future<Address> fetch(int addressId);
  Future<bool> update(int addressId, Address address);
}

class AddressRepository extends AddressSource {
  final List<AddressSource> sources;

  AddressRepository(this.sources);

  Future<Address> fetch(int addressId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(addressId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<bool> update(int addressId, Address address) =>
      sources[0].update(addressId, address);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
