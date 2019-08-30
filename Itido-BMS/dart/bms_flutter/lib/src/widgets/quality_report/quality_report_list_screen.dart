import 'package:bms_dart/quality_report_list_bloc.dart';
import 'package:bms_flutter/src/widgets/bloc_list_half_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';

import 'quality_report_list.dart';

class QualityReportListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required QualityReportListBloc Function(BuildContext) blocBuilder,
    Function(QualityReport) onSelect,
    Widget floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: QualityReportListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final QualityReportListBloc Function(BuildContext) blocBuilder;
  final Function(QualityReport) onSelect;
  final Widget floatingActionButton;

  QualityReportListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _QualityReportListScreenState createState() =>
      _QualityReportListScreenState();
}

class _QualityReportListScreenState extends State<QualityReportListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<QualityReportListBloc, QualityReportListEvent,
        ListState<QualityReport>, QualityReport>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return QualityReportList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
