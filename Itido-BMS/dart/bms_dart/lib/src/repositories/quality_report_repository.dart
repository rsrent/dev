import 'dart:async';
import '../models/quality_report.dart';
import '../models/quality_report_item.dart';
import 'source.dart';

abstract class QualityReportSource extends Source {
  Future<QualityReport> fetch(int qualityReportId);
  Future<List<QualityReport>> fetchOfProjectItem(int projectItemId);
  Future<int> create(int projectItemId, {QualityReport qualityReport});
  Future<bool> complete(int qualityReportId);
  Future<bool> update(int qualityReportId, QualityReport qualityReport);
  Future<List<QualityReportItem>> fetchItemsOfQualityReport(
      int qualityReportId);
  Future<bool> updateQualityReportItem(
      int qualityReportItemId, QualityReportItem qualityReportItem);
}

class QualityReportRepository extends QualityReportSource {
  final List<QualityReportSource> sources;
  QualityReportRepository(this.sources);

  Future<QualityReport> fetch(int qualityReportId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(qualityReportId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<QualityReport>> fetchOfProjectItem(int projectItemId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfProjectItem(projectItemId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<QualityReportItem>> fetchItemsOfQualityReport(
      int qualityReportId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchItemsOfQualityReport(qualityReportId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<int> create(int projectItemId, {QualityReport qualityReport}) =>
      sources[0].create(projectItemId, qualityReport: qualityReport);
  Future<bool> complete(int qualityReportId) =>
      sources[0].complete(qualityReportId);
  Future<bool> update(int qualityReportId, QualityReport qualityReport) =>
      sources[0].update(qualityReportId, qualityReport);
  Future<bool> updateQualityReportItem(
          int qualityReportItemId, QualityReportItem qualityReportItem) =>
      sources[0]
          .updateQualityReportItem(qualityReportItemId, qualityReportItem);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
