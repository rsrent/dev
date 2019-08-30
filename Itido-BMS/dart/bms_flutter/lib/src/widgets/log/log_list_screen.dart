import 'package:bms_dart/blocs.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'log_list.dart';

class LogListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required LogListBloc Function(BuildContext) blocBuilder,
    Function(Log) onSelect,
    Widget floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        body: LogListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final LogListBloc Function(BuildContext) blocBuilder;
  final Function(Log) onSelect;
  final Widget floatingActionButton;

  LogListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _LogListScreenState createState() => _LogListScreenState();
}

class _LogListScreenState extends State<LogListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<LogListBloc, LogListEvent, ListState<Log>, Log>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return LogList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
