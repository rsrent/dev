import 'dart:async';
import 'morework_api.dart';
import '../models/morework.dart';

class MoreWorkRepository extends MoreWorkSource {
  List<MoreWorkSource> sources = <MoreWorkSource>[
    MoreWorkApi(),
  ];

  Future<List<MoreWork>> fetchMoreWorks() async {
    var moreworks;
    for (var source in sources) {
      moreworks = await source.fetchMoreWorks();
      if (moreworks != null) {
        break;
      }
    }
    return moreworks;
  }
}

abstract class MoreWorkSource {
  Future<List<MoreWork>> fetchMoreWorks();
}