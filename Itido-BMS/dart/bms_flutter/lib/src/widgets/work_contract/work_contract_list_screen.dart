import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_contract_list_bloc.dart';

import '../bloc_list_half_screen.dart';
import 'work_contract_list.dart';
import 'package:flutter_packages/calendar_2.dart' as calendar;

class WorkContractListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required WorkContractListBloc Function(BuildContext) blocBuilder,
    Function(WorkContract, WorkContractListBloc) onSelect,
    FloatingActionButton floatingActionButton,
    bool showUser = true,
    bool showProject = true,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: WorkContractListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
          showUser: showUser,
          showProject: showProject,
        ),
      ),
    ));
  }

  final WorkContractListBloc Function(BuildContext) blocBuilder;
  final Function(WorkContract, WorkContractListBloc) onSelect;
  final Widget floatingActionButton;

  final bool showUser;
  final bool showProject;

  WorkContractListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
    this.showUser = true,
    this.showProject = true,
  }) : super(key: key);

  @override
  _WorkContractListScreenState createState() => _WorkContractListScreenState();
}

class _WorkContractListScreenState extends State<WorkContractListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<WorkContractListBloc, WorkContractListEvent,
        ListState<WorkContract>, WorkContract>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return WorkContractList(
          onSelect: widget.onSelect != null
              ? (workContract) => widget.onSelect(workContract, bloc)
              : null,
          showProject: widget.showProject,
          showUser: widget.showUser,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
