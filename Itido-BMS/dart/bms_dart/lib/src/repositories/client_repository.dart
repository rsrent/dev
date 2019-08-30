import 'dart:async';
import '../models/client.dart';
import 'source.dart';

abstract class ClientSource extends Source {
  Future<List<Client>> fetchClients();
}

class ClientRepository extends ClientSource {
  final List<ClientSource> sources;

  ClientRepository(this.sources);

  Future<List<Client>> fetchClients() async {
    var values;
    for (var source in sources) {
      values = await source.fetchClients();
      if (values != null) {
        break;
      }
    }
    return values;
  }

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
