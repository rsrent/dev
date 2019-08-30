import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/quality_report_item.dart';

@immutable
abstract class QualityReportItemListEvent extends Equatable {
  QualityReportItemListEvent([List props = const []]) : super(props);
}

class FetchOfQualityReport extends QualityReportItemListEvent {
  final int qualityReportId;
  FetchOfQualityReport({@required this.qualityReportId})
      : super([qualityReportId]);
  @override
  String toString() => 'FetchOfQualityReport';
}

class QualityReportItemListFetched extends QualityReportItemListEvent {
  final List<QualityReportItem> qualityReportItems;
  QualityReportItemListFetched({@required this.qualityReportItems})
      : super([qualityReportItems]);
  @override
  String toString() =>
      'QualityReportItemListFetched { qualityReportItems: ${qualityReportItems.length} }';
}

class UpdateQualityReportItem extends QualityReportItemListEvent {
  final QualityReportItem qualityReportItem;
  UpdateQualityReportItem({@required this.qualityReportItem})
      : super([qualityReportItem]);
  @override
  String toString() =>
      'UpdateQualityReportItem { qualityReportItem: ${qualityReportItem} }';
}
