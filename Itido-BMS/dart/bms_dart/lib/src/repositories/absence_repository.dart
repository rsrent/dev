import 'dart:async';

import 'package:bms_dart/query_result.dart';

import '../models/absence.dart';
import 'source.dart';

abstract class AbsenceSource extends Source {
  Future<QueryResult<Absence>> fetch(int absenceId);
  Future<QueryResult<List<Absence>>> fetchAbsencesOfUser(int userId);
  Future<QueryResult<int>> createAbsence(
      Absence absence, int userId, int absenceReasonId, bool asRequest);
  Future<QueryResult<bool>> updateAbsence(Absence absence);
  Future<QueryResult<bool>> replyToAbsence(int absenceId, bool isApproved);
}

class AbsenceRepository extends AbsenceSource {
  final List<AbsenceSource> sources;

  AbsenceRepository(this.sources);

  // Future<List<Absence>> fetchAllAbsences() async {
  //   var absences;
  //   for (var source in sources) {
  //     absences = await source.fetchAllAbsences();
  //     if (absences != null) {
  //       break;
  //     }
  //   }
  //   return absences;
  // }

  Future<QueryResult<List<Absence>>> fetchAbsencesOfUser(int userId) async {
    var absences;
    for (var source in sources) {
      absences = await source.fetchAbsencesOfUser(userId);
      if (absences != null) {
        break;
      }
    }
    return absences;
  }

  Future<QueryResult<int>> createAbsence(
          Absence absence, int userId, int absenceReasonId, bool asRequest) =>
      sources[0].createAbsence(absence, userId, absenceReasonId, asRequest);

  Future<QueryResult<bool>> updateAbsence(Absence absence) =>
      sources[0].updateAbsence(absence);

  Future<QueryResult<bool>> replyToAbsence(int absenceId, bool isApproved) =>
      sources[0].replyToAbsence(absenceId, isApproved);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }

  @override
  Future<QueryResult<Absence>> fetch(int absenceId) =>
      sources[0].fetch(absenceId);
}
