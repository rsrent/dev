import 'package:bms_dart/quality_report_list_bloc.dart';
import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class QualityReportList extends StatelessWidget {
  final Function(QualityReport) onSelect;
  final Function(QualityReport) onDelete;

  const QualityReportList({Key key, this.onSelect, this.onDelete})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    final taskListBloc = BlocProvider.of<QualityReportListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: taskListBloc,
      builder: (context, ListState<QualityReport> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          var tasks = (state as Loaded).items;
          if (tasks.isEmpty) {
            return InfoListView(
                info: Translations.of(context).infoNoQualityReports);
          }
          return ListView.separated(
            padding: EdgeInsets.only(top: 20, bottom: 200),
            itemBuilder: (BuildContext context, int index) {
              return QualityReportTile(
                task: tasks[index],
                onSelect: onSelect,
              );
            },
            itemCount: tasks.length,
            separatorBuilder: (context, index) => Divider(),
          );
        }
      },
    );
  }
}

class QualityReportTile extends StatelessWidget {
  final QualityReport task;
  final Function(QualityReport) onSelect;

  const QualityReportTile({
    Key key,
    @required this.task,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      leading: Icon(task.completedTime != null ? Icons.check : Icons.remove),
      title: Text(Translations.of(context).dateString(task.createdTime) ?? '-'),
      onTap: onSelect != null ? () => onSelect(task) : null,
    );
  }
}
