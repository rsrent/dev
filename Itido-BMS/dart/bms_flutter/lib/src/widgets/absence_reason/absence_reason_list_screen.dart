import 'package:bms_dart/absence_reason_list_bloc.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'absence_reason_list.dart';

class AbsenceReasonListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required AbsenceReasonListBloc Function(BuildContext) blocBuilder,
    Function(AbsenceReason) onSelect,
    FloatingActionButton floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: AbsenceReasonListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final AbsenceReasonListBloc Function(BuildContext) blocBuilder;
  final Function(AbsenceReason) onSelect;
  final FloatingActionButton floatingActionButton;

  AbsenceReasonListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _AbsenceReasonListScreenState createState() =>
      _AbsenceReasonListScreenState();
}

class _AbsenceReasonListScreenState extends State<AbsenceReasonListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<AbsenceReasonListBloc, AbsenceReasonListEvent,
        ListState<AbsenceReason>, AbsenceReason>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return AbsenceReasonList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
