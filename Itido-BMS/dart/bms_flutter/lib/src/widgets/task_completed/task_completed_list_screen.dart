import 'package:bms_dart/task_completed_list_bloc.dart';
import 'package:bms_flutter/src/widgets/bloc_list_half_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';

import 'task_completed_list.dart';

class TaskCompletedListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required TaskCompletedListBloc Function(BuildContext) blocBuilder,
    Function(TaskCompleted) onSelect,
    Widget floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: TaskCompletedListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final TaskCompletedListBloc Function(BuildContext) blocBuilder;
  final Function(TaskCompleted) onSelect;
  final Widget floatingActionButton;

  TaskCompletedListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _TaskCompletedListScreenState createState() =>
      _TaskCompletedListScreenState();
}

class _TaskCompletedListScreenState extends State<TaskCompletedListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<TaskCompletedListBloc, TaskCompletedListEvent,
        ListState<TaskCompleted>, TaskCompleted>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return TaskCompletedList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
