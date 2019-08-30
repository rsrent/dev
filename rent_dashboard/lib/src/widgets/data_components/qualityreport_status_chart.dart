import 'package:flutter/material.dart';
import 'package:charts_flutter/flutter.dart' as charts;
import '../../models/workstatus.dart';
//import 'package:charts_common/src/common/palette.dart' as p;
//import 'package:charts_common/src/common/color.dart' as c;
import '../../models/count_with_workstatus.dart';
import '../../blocs/location_provider.dart';
import '../../screens/locations_list.dart';
import '../../models/sortable_by.dart';

class QualityReportStatusChart extends StatelessWidget {
  final List<charts.Series<dynamic, String>> seriesList;
  final String title;

  QualityReportStatusChart(this.title, CountWithWorkStatusSet statusSet)
      : seriesList = [
          new charts.Series<CountWithWorkStatus, String>(
            id: 'Sales',
            colorFn: (s, i) => i == 0
                ? charts.MaterialPalette.red.shadeDefault
                : i == 1
                    ? charts.MaterialPalette.yellow.shadeDefault
                    : i == 2
                        ? charts.MaterialPalette.green.shadeDefault
                        : charts.MaterialPalette.gray.shadeDefault,
            domainFn: (s, i) => getWorkStatusName(s.status),
            /* i == 0
                ? 'Forsinket'
                : i == 1 ? 'Kritisk' : i == 2 ? 'Okay' : 'Ustarted',*/
            measureFn: (s, i) => s.occurences,
            data: statusSet.asList(),
          )
        ];

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        //Text(title),
        Expanded(
          child: charts.BarChart(
            seriesList,
            animate: false,
            selectionModels: [
              charts.SelectionModelConfig(
                type: charts.SelectionModelType.info,
                listener: (m) {
                  if (m.selectedDatum.isNotEmpty) {
                    CountWithWorkStatus workStatus =
                        m.selectedDatum.first.datum;
                    navigateToSelection(context, workStatus.status);
                  }
                },
              ),
            ],
          ),
        ),
      ],
    );
  }

  navigateToSelection(BuildContext context, WorkStatus status) {
    var oldBloc = LocationProvider.of(context);

    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) {
        return LocationProvider(
          customerId: oldBloc.customerId,
          userId: oldBloc.userId,
          child: LocationsList(
            prepareBloc: (b) {
              b.updateQualityReportStatusOptions(
                  WorkStatus.Overdue, WorkStatus.Overdue == status);
              b.updateQualityReportStatusOptions(
                  WorkStatus.Critical, WorkStatus.Critical == status);
              b.updateQualityReportStatusOptions(
                  WorkStatus.Okay, WorkStatus.Okay == status);
              b.updateQualityReportStatusOptions(
                  WorkStatus.Unstarted, WorkStatus.Unstarted == status);
              b.sortBy(SortBy.QualityReportStatus);
            },
          ),
        );
      }),
    );
  }
}
