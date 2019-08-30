import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/quality_report.dart';
import 'package:bms_dart/repositories.dart';

class QualityReportListBloc
    extends Bloc<QualityReportListEvent, ListState<QualityReport>>
    with Refreshable {
  final QualityReportRepository _qualityReportRepository =
      repositoryProvider.qualityReportRepository();

  final QualityReportListEvent Function() _refreshEven;

  QualityReportListBloc(this._refreshEven) {
    refresh();
  }

  @override
  ListState<QualityReport> get initialState => Loading<QualityReport>();

  @override
  Stream<ListState<QualityReport>> mapEventToState(
    QualityReportListEvent event,
  ) async* {
    if (event is QualityReportListFetchOfProjectItem) {
      _qualityReportRepository.fetchOfProjectItem(event.projectItemId).then(
          (qualityReports) => dispatch(
              QualityReportListFetched(qualityReports: qualityReports)));
    }

    if (event is QualityReportListFetched) {
      final items = event.qualityReports;
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is QualityReportListCreateNew) {
      _qualityReportRepository.create(event.projectItemId);
    }
    if (event is QualityReportListComplete) {
      _qualityReportRepository.complete(event.qualityReportId);
    }
  }

  @override
  void refresh() {
    dispatch(_refreshEven());
  }
}
