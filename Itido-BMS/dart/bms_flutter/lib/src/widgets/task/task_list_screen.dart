import 'package:bms_dart/task_list_bloc.dart';
import 'package:bms_flutter/src/widgets/bloc_list_half_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';

import 'task_list.dart';

class TaskListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required TaskListBloc Function(BuildContext) blocBuilder,
    Function(Task) onSelect,
    Widget floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: TaskListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final TaskListBloc Function(BuildContext) blocBuilder;
  final Function(Task) onSelect;
  final Widget floatingActionButton;

  TaskListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _TaskListScreenState createState() => _TaskListScreenState();
}

class _TaskListScreenState extends State<TaskListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<TaskListBloc, TaskListEvent, ListState<Task>,
        Task>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return TaskList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
