import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/quality_report_item.dart';
import 'package:bms_dart/repositories.dart';

class QualityReportItemListBloc
    extends Bloc<QualityReportItemListEvent, ListState<QualityReportItem>>
    with Refreshable {
  final QualityReportRepository _qualityReportRepository =
      repositoryProvider.qualityReportRepository();

  final QualityReportItemListEvent Function() _refreshEven;

  QualityReportItemListBloc(this._refreshEven) {
    refresh();
  }

  @override
  ListState<QualityReportItem> get initialState => Loading<QualityReportItem>();

  @override
  Stream<ListState<QualityReportItem>> mapEventToState(
    QualityReportItemListEvent event,
  ) async* {
    if (event is FetchOfQualityReport) {
      _qualityReportRepository
          .fetchItemsOfQualityReport(event.qualityReportId)
          .then((qualityReportItems) => dispatch(QualityReportItemListFetched(
              qualityReportItems: qualityReportItems)));
    }

    if (event is QualityReportItemListFetched) {
      final items = event.qualityReportItems;
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is UpdateQualityReportItem) {
      var item = event.qualityReportItem;
      _qualityReportRepository
          .updateQualityReportItem(item.id, item)
          .then((_) => refresh());
    }
  }

  @override
  void refresh() {
    dispatch(_refreshEven());
  }
}
