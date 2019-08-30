import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/quality_report.dart';

@immutable
abstract class QualityReportListEvent extends Equatable {
  QualityReportListEvent([List props = const []]) : super(props);
}

class QualityReportListFetchOfProjectItem extends QualityReportListEvent {
  final int projectItemId;
  QualityReportListFetchOfProjectItem({@required this.projectItemId})
      : super([projectItemId]);
  @override
  String toString() => 'QualityReportListFetchOfProjectItem';
}

class QualityReportListFetched extends QualityReportListEvent {
  final List<QualityReport> qualityReports;
  QualityReportListFetched({@required this.qualityReports})
      : super([qualityReports]);
  @override
  String toString() =>
      'QualityReportListFetched { qualityReports: ${qualityReports.length} }';
}

class QualityReportListCreateNew extends QualityReportListEvent {
  final int projectItemId;
  QualityReportListCreateNew({@required this.projectItemId})
      : super([projectItemId]);
  @override
  String toString() =>
      'QualityReportListCreateNew { projectItemId: $projectItemId }';
}

class QualityReportListComplete extends QualityReportListEvent {
  final int qualityReportId;
  QualityReportListComplete({@required this.qualityReportId})
      : super([qualityReportId]);
  @override
  String toString() =>
      'QualityReportListComplete { qualityReportId: $qualityReportId }';
}
