import 'dart:async';

import '../models/absence_reason.dart';
import 'source.dart';

abstract class AbsenceReasonSource extends Source {
  Future<List<AbsenceReason>> fetchAllAbsenceReasons();
  Future<int> createAbsenceReason(AbsenceReason absenceReason);
  Future<bool> updateAbsenceReason(AbsenceReason absenceReason);
}

class AbsenceReasonRepository extends AbsenceReasonSource {
  final List<AbsenceReasonSource> sources;

  AbsenceReasonRepository(this.sources);

  Future<List<AbsenceReason>> fetchAllAbsenceReasons() async {
    var contracts;
    for (var source in sources) {
      contracts = await source.fetchAllAbsenceReasons();
      if (contracts != null) {
        break;
      }
    }
    return contracts;
  }

  Future<int> createAbsenceReason(AbsenceReason absenceReason) =>
      sources[0].createAbsenceReason(absenceReason);

  Future<bool> updateAbsenceReason(AbsenceReason absenceReason) =>
      sources[0].updateAbsenceReason(absenceReason);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
