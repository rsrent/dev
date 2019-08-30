import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/absence_list_bloc.dart';
import 'absence_list.dart';

class AbsenceListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required AbsenceListBloc Function(BuildContext) blocBuilder,
    Function(Absence) onSelect,
    FloatingActionButton floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        body: AbsenceListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final AbsenceListBloc Function(BuildContext) blocBuilder;
  final Function(Absence) onSelect;
  final FloatingActionButton floatingActionButton;

  AbsenceListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _AbsenceListScreenState createState() => _AbsenceListScreenState();
}

class _AbsenceListScreenState extends State<AbsenceListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<AbsenceListBloc, AbsenceListEvent,
        ListState<Absence>, Absence>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return AbsenceList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
