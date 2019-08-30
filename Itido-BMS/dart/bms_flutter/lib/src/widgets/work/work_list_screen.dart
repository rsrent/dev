import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_list_bloc.dart';

import '../bloc_list_half_screen.dart';
import 'work_list.dart';
import 'package:flutter_packages/calendar_2.dart' as calendar;

class WorkListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required WorkListBloc Function(BuildContext) blocBuilder,
    Function(Work, WorkListBloc) onSelect,
    Function(Work, WorkListBloc) onSelectTime,
    Function(Work, WorkListBloc) onInviteAcceptSelect,
    Function(Work, WorkListBloc) onInviteDeclineSelect,
    FloatingActionButton floatingActionButton,
    bool showUser = true,
    bool showProject = true,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: WorkListScreen(
          onSelect: onSelect,
          onSelectTime: onSelectTime,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
          showUser: showUser,
          showProject: showProject,
          onInviteAcceptSelect: onInviteAcceptSelect,
          onInviteDeclineSelect: onInviteDeclineSelect,
        ),
      ),
    ));
  }

  final WorkListBloc Function(BuildContext) blocBuilder;
  final Function(Work, WorkListBloc) onSelect;
  final Function(Work, WorkListBloc) onSelectTime;
  final Function(Work, WorkListBloc) onInviteAcceptSelect;
  final Function(Work, WorkListBloc) onInviteDeclineSelect;
  final Widget floatingActionButton;

  final bool showUser;
  final bool showProject;

  WorkListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
    this.showUser = true,
    this.showProject = true,
    this.onSelectTime,
    this.onInviteAcceptSelect,
    this.onInviteDeclineSelect,
  }) : super(key: key);

  @override
  _WorkListScreenState createState() => _WorkListScreenState();
}

class _WorkListScreenState extends State<WorkListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<WorkListBloc, WorkListEvent, ListState<Work>,
        Work>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return WorkList(
          onSelect: widget.onSelect != null
              ? (work) => widget.onSelect(work, bloc)
              : null,
          onSelectTime: widget.onSelectTime != null
              ? (work) => widget.onSelectTime(work, bloc)
              : null,
          showProject: widget.showProject,
          showUser: widget.showUser,
          onInviteAcceptSelect: widget.onInviteAcceptSelect != null
              ? (work) => widget.onInviteAcceptSelect(work, bloc)
              : null,
          onInviteDeclineSelect: widget.onInviteDeclineSelect != null
              ? (work) => widget.onInviteDeclineSelect(work, bloc)
              : null,
          headerBuilder: (context) {
            return Column(
              children: <Widget>[
                FlatButton(
                  child: Text(bloc.withDate ? 'Få recent' : 'Mellem datoer'),
                  onPressed: () {
                    bloc.withDate = !bloc.withDate;
                    bloc.refresh();
                  },
                ),
                if (bloc.withDate)
                  InkWell(
                    child: Padding(
                      padding: const EdgeInsets.all(16),
                      child: Text(
                        '${Translations.of(context).dateString(bloc.from)} - ${Translations.of(context).dateString(bloc.to)}',
                        style: TextStyle(
                          fontSize: 16,
                          color: Theme.of(context).primaryColor,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                    onTap: () async {
                      var dates = await calendar
                          .pushCalendarScreenAsRangePicker(context,
                              firstSelected: bloc.from, lastSelected: bloc.to);
                      if (dates != null) {
                        bloc.from = dates.first;
                        bloc.to = dates.second;
                        bloc.refresh();
                      }
                    },
                  ),
              ],
            );
          },
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
