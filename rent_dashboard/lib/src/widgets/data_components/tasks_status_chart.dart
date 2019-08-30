import 'package:flutter/material.dart';
import 'package:charts_flutter/flutter.dart' as charts;
import '../../models/workstatus.dart';
//import 'package:charts_common/src/common/color.dart' as chartColor;
import '../../models/count_with_workstatus.dart';

import '../../screens/tasks_list.dart';
import '../../blocs/task_provider.dart';
import '../../models/sortable_by.dart';

class TasksStatusChart extends StatelessWidget {
  final List<charts.Series<CountWithWorkStatus, String>> seriesList;
  final String title;

  TasksStatusChart(this.title, List<CountWithWorkStatusSet> sets)
      : seriesList = [
          createSeries(sets[0].asList(), 0),
          createSeries(sets[1].asList(), 1),
          createSeries(sets[2].asList(), 2),
        ];

  static charts.Series<CountWithWorkStatus, String> createSeries(
      List<CountWithWorkStatus> list, int index) {
    return new charts.Series<CountWithWorkStatus, String>(
      id: '$index',
      colorFn: (_, i) => statusColor(i, index),
      domainFn: (s, _) => getWorkStatusName(s.status),
      measureFn: (s, _) => s.occurences,
      data: list,
    );
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Text(title),
        Expanded(
          child: charts.BarChart(
            seriesList,
            animate: false,
            barGroupingType: charts.BarGroupingType.stacked,
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
}

statusColor(int index, int group) {
  var char = group == 0 ? 'f' : group == 1 ? 'd' : 'b';

  return index == 0
      ? charts.Color.fromHex(code: '#${char}45942')
      : index == 1
          ? charts.Color.fromHex(code: '#${char}fc35b')
          : index == 2
              ? charts.Color.fromHex(code: '#60${char}060')
              : charts.Color
                  .fromHex(code: '#${7+group}f${7+group}f${7+group}f');
}

navigateToSelection(BuildContext context, WorkStatus status) {
  var oldBloc = TaskProvider.of(context);

  Navigator.push(
    context,
    MaterialPageRoute(builder: (context) {
      return TaskProvider(
        customerId: oldBloc.customerId,
        userId: oldBloc.userId,
        child: TasksList(
          prepareBloc: (b) {
            b.updateCleaningTaskStatusOptions(
                WorkStatus.Overdue, WorkStatus.Overdue == status);
            b.updateCleaningTaskStatusOptions(
                WorkStatus.Critical, WorkStatus.Critical == status);
            b.updateCleaningTaskStatusOptions(
                WorkStatus.Okay, WorkStatus.Okay == status);
            b.updateCleaningTaskStatusOptions(
                WorkStatus.Unstarted, WorkStatus.Unstarted == status);
            b.sortBy(SortBy.CleaningTaskStatus);
          },
        ),
      );
    }),
  );
}
