/// Timeseries chart example
import 'package:charts_flutter/flutter.dart' as charts;
import 'package:flutter/material.dart';
import '../../blocs/work_history_provider.dart';

class WorkHistoryChart extends StatelessWidget {
  final List<charts.Series<TimeSeriesSales, DateTime>> seriesList;

  WorkHistoryChart(Map<DateTime, List<int>> map) : seriesList = [
    createSeries(map, 0),
    createSeries(map, 1),
    createSeries(map, 2),
    //createSeries(map, 3),
    //createSeries(map, 4),
  ];

  static charts.Series<TimeSeriesSales, DateTime> createSeries(
      Map<DateTime, List<int>> data, int index) {
    var list = List<TimeSeriesSales>();

    data.forEach((k, v) {
      list.add(TimeSeriesSales(k, v[index]));
    });

    return charts.Series<TimeSeriesSales, DateTime>(
      id: index == 0 ? 'Logs' : index == 1 ? 'Reporter' : index == 2 ? 'Vinduer' : 'Opgaver $index',
      //colorFn: (_, __) => charts.MaterialPalette.blue.shadeDefault,
      domainFn: (TimeSeriesSales sales, _) => sales.time,
      measureFn: (TimeSeriesSales sales, _) => sales.amount,
      data: list,
    );
  }

  @override
  Widget build(BuildContext context) {
    return charts.TimeSeriesChart(
          seriesList,
          animate: false,
          dateTimeFactory: const charts.LocalDateTimeFactory(),
          behaviors: [new charts.SeriesLegend()],
        );
  }
}

class TimeSeriesSales {
  final DateTime time;
  final int amount;

  TimeSeriesSales(this.time, this.amount);
}
