import 'dart:async';
import '../models/noti.dart';
import 'source.dart';

abstract class NotiSource extends Source {
  Future<List<Noti>> fetchLatestNotis(int count);
  Future<bool> setNotiSeen(int id);
}

class NotiRepository extends NotiSource {
  final List<NotiSource> sources;

  NotiRepository(this.sources);

  Future<List<Noti>> fetchLatestNotis(int count) async {
    var values;
    for (var source in sources) {
      values = await source.fetchLatestNotis(count);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<bool> setNotiSeen(int id) => sources[0].setNotiSeen(id);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
