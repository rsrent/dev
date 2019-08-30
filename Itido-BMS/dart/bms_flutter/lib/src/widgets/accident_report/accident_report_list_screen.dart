import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/accident_report_list_bloc.dart';
import 'accident_report_list.dart';

class AccidentReportListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required AccidentReportListBloc Function(BuildContext) blocBuilder,
    Function(AccidentReport) onSelect,
    FloatingActionButton floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        body: AccidentReportListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final AccidentReportListBloc Function(BuildContext) blocBuilder;
  final Function(AccidentReport) onSelect;
  final FloatingActionButton floatingActionButton;

  AccidentReportListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _AccidentReportListScreenState createState() =>
      _AccidentReportListScreenState();
}

class _AccidentReportListScreenState extends State<AccidentReportListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<AccidentReportListBloc, AccidentReportListEvent,
        ListState<AccidentReport>, AccidentReport>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return AccidentReportList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
