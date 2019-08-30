import 'dart:async';
import 'package:bms_dart/query_result.dart';

import '../models/accident_report.dart';
import 'source.dart';

abstract class AccidentReportSource extends Source {
  Future<QueryResult<List<AccidentReport>>> fetchNew();
  Future<QueryResult<List<AccidentReport>>> fetchAccidentReportsOfUser(
      int userId);
  Future<QueryResult<int>> create(AccidentReport post, int userId);
  Future<QueryResult<bool>> replyToAccidentReport(
      int accidentReportId, bool isApproved);
}

class AccidentReportRepository extends AccidentReportSource {
  final List<AccidentReportSource> sources;

  AccidentReportRepository(this.sources);

  Future<List<AccidentReport>> fetchNew() async {
    var values;
    for (var source in sources) {
      values = await source.fetchNew();
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<AccidentReport>> fetchAccidentReportsOfUser(int userId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchAccidentReportsOfUser(userId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<int> create(AccidentReport post, int userId) =>
      sources[0].create(post, userId);

  Future<bool> replyToAccidentReport(int accidentReportId, bool isApproved) =>
      sources[0].replyToAccidentReport(accidentReportId, isApproved);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
