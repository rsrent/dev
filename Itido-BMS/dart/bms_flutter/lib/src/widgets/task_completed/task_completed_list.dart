import 'package:bms_dart/task_completed_list_bloc.dart';
import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class TaskCompletedList extends StatelessWidget {
  final Function(TaskCompleted) onSelect;
  final Function(TaskCompleted) onDelete;

  const TaskCompletedList({Key key, this.onSelect, this.onDelete})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    final taskCompletedListBloc =
        BlocProvider.of<TaskCompletedListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: taskCompletedListBloc,
      builder: (context, ListState<TaskCompleted> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          var taskCompleteds = (state as Loaded).items;
          if (taskCompleteds.isEmpty) {
            return InfoListView(
                info: Translations.of(context).infoNoTaskCompleteds);
          }
          return ListView.separated(
            padding: EdgeInsets.only(top: 20, bottom: 200),
            itemBuilder: (BuildContext context, int index) {
              return TaskCompletedTile(
                taskCompleted: taskCompleteds[index],
                onSelect: onSelect,
              );
            },
            itemCount: taskCompleteds.length,
            separatorBuilder: (context, index) => Divider(),
          );
        }
      },
    );
  }
}

class TaskCompletedTile extends StatelessWidget {
  final TaskCompleted taskCompleted;
  final Function(TaskCompleted) onSelect;

  const TaskCompletedTile({
    Key key,
    @required this.taskCompleted,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      title: Text(
        Translations.of(context).dateString(taskCompleted.completedDate),
      ),
      subtitle:
          taskCompleted.comment != null ? Text(taskCompleted.comment) : null,
      onTap: onSelect != null ? () => onSelect(taskCompleted) : null,
    );
  }
}
