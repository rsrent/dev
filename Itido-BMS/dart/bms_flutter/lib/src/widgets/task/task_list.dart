import 'package:bms_dart/task_list_bloc.dart';
import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class TaskList extends StatelessWidget {
  final Function(Task) onSelect;
  final Function(Task) onDelete;

  const TaskList({Key key, this.onSelect, this.onDelete}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final taskListBloc = BlocProvider.of<TaskListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: taskListBloc,
      builder: (context, ListState<Task> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          var tasks = (state as Loaded).items;
          if (tasks.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoTasks);
          }
          return ListView.separated(
            padding: EdgeInsets.only(top: 20, bottom: 200),
            itemBuilder: (BuildContext context, int index) {
              return Card(
                child: TaskTile(
                  task: tasks[index],
                  onSelect: onSelect,
                ),
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

class TaskTile extends StatelessWidget {
  final Task task;
  final Function(Task) onSelect;

  const TaskTile({
    Key key,
    @required this.task,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      title: Text((task.description ?? '') +
          (task.description != null && task.place != null ? ' - ' : '') +
          (task.place != null ? task.place : '')),
      onTap: onSelect != null ? () => onSelect(task) : null,
      subtitle: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: <Widget>[
          if (task.comment != null && task.comment.length > 0)
            ListTile(
              contentPadding: EdgeInsets.all(0),
              title: Text('comment'),
              subtitle: Text(task.comment ?? '-'),
            ),
          if (task.squareMeters != null && task.squareMeters > 0)
            ListTile(
              contentPadding: EdgeInsets.all(0),
              leading: Icon(Icons.photo_size_select_small),
              title: Text((task.squareMeters ?? '-').toString()),
            ),
          if (task.frequency != null)
            ListTile(
              contentPadding: EdgeInsets.all(0),
              leading: Icon(Icons.code),
              title: Text(task.frequency),
            ),
          if (task.timesOfYear != null)
            ListTile(
              contentPadding: EdgeInsets.all(0),
              leading: Icon(Icons.timeline),
              title: Text((task.timesOfYear ?? '-').toString()),
            ),
        ],
      ),
    );
  }
}
