import 'dart:math';

import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/components/pop_up_dialogs.dart';
import 'package:bms_flutter/src/components/project_name.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_list_bloc.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../../style.dart' as style;
import 'package:dart_packages/date_time_operations.dart' as dtOps;

class WorkList extends StatelessWidget {
  final Function(Work) onSelect;
  final Function(Work) onSelectTime;
  final Function(Work) onInviteAcceptSelect;
  final Function(Work) onInviteDeclineSelect;

  final bool showUser;
  final bool showProject;

  final WidgetBuilder headerBuilder;

  const WorkList({
    Key key,
    this.onSelect,
    this.onSelectTime,
    this.onInviteAcceptSelect,
    this.onInviteDeclineSelect,
    this.showUser = true,
    this.showProject = true,
    this.headerBuilder,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final workListBloc = BlocProvider.of<WorkListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: workListBloc,
      builder: (context, ListState<Work> state) {
        if (state is Failure) {
          return InfoListView(
            info: Translations.of(context).infoErrorLoading,
            child: headerBuilder(context),
          );
        }

        if (state is Loaded<Work>) {
          if (state.items.isEmpty) {
            return InfoListView(
              info: Translations.of(context).infoNoWorks,
              child: headerBuilder(context),
            );
          }

          return ListView.builder(
            padding: EdgeInsets.only(top: 8, bottom: 80),
            itemCount: state.items.length * 2 + (headerBuilder != null ? 1 : 0),
            itemBuilder: (BuildContext context, int index) {
              if (headerBuilder != null) {
                if (index == 0) {
                  return headerBuilder(context);
                } else {
                  index--;
                }
              }

              var _index = index ~/ 2;
              var item = state.items[_index];
              var isHeader = index % 2 == 0;

              var showHeader = (_index == 0 ||
                  !dtOps.isSameDate(state.items[_index - 1].date, item.date));

              if (isHeader) {
                if (!showHeader) return Container();
                var dateText =
                    '${Translations.of(context).dateString(item.date)}';
                if (dtOps.isSameDate(DateTime.now(), item.date)) {
                  dateText = Translations.of(context).infoToday;
                }
                if (dtOps.isSameDate(
                    DateTime.now().subtract(Duration(days: 1)), item.date)) {
                  dateText = Translations.of(context).infoYesterday;
                }
                if (dtOps.isSameDate(
                    DateTime.now().add(Duration(days: 1)), item.date)) {
                  dateText = Translations.of(context).infoTomorrow;
                }

                return Container(
                  padding: EdgeInsets.only(left: 16, top: 8, bottom: 4),
                  child: Text(dateText),
                );
              }

              return Padding(
                padding: const EdgeInsets.only(bottom: 8.0),
                child: WorkTile(
                  work: item,
                  onSelect: onSelect,
                  onSelectTime: onSelectTime,
                  onInviteAcceptSelect: onInviteAcceptSelect,
                  onInviteDeclineSelect: onInviteDeclineSelect,
                  showUser: showUser,
                  showProject: showProject,
                ),
              );
            },
          );
        }
      },
    );
  }
}

class WorkTile extends StatelessWidget {
  final Work work;
  final Function(Work) onSelect;
  final Function(Work) onSelectTime;
  final Function(Work) onInviteAcceptSelect;
  final Function(Work) onInviteDeclineSelect;
  final bool showUser;
  final bool showProject;

  const WorkTile({
    Key key,
    @required this.work,
    this.showUser = true,
    this.showProject = true,
    this.onSelect,
    this.onSelectTime,
    this.onInviteAcceptSelect,
    this.onInviteDeclineSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    double height = showProject && showUser ? 130 : showUser ? 90 : 90;

    if (work.isInvited) height = max(height, 110);

    //height = 240;

    Color color = Theme.of(context).primaryColor;
    if (!work.isTaken) color = style.declineRed;
    if (work.isLate) color = style.declineRed;
    if (work.isInvited) color = Theme.of(context).primaryColor;
    var replacementWidget = work.isOwnerAbsent
        ? Text(
            work.isOwned
                ? work.user.displayName +
                    ' (${Translations.of(context).infoAbsent})'
                : Translations.of(context).infoWorkMissingOwner,
            maxLines: 1,
            overflow: TextOverflow.ellipsis,
            style: TextStyle(fontStyle: FontStyle.italic),
          )
        : null;

    return InkWell(
      child: Container(
        height: height,
        color: color,
        // decoration: BoxDecoration(
        //     border: work.isPartOfWorkContract
        //         ? Border(top: BorderSide(width: 1, color: Colors.grey))
        //         : null),
        // padding: const EdgeInsets.only(right: 6),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            Expanded(
              child: Container(
                padding: EdgeInsets.only(right: 8, top: 8, bottom: 8),
                decoration: BoxDecoration(
                  color: work.isPartOfWorkContract
                      ? Color(0xFFF1F9FF)
                      : Color(0xFFdde4eb),
                ),
                child: Row(
                  children: <Widget>[
                    Expanded(
                      child: (showProject && !showUser)
                          ? Padding(
                              padding: EdgeInsets.only(top: 8),
                              child: ListTile(
                                title: ProjectName(project: work.project),
                                // subtitle: Text(work.location.address),
                                // subtitle: replacementWidget,
                              ),
                            )
                          : Column(
                              crossAxisAlignment: CrossAxisAlignment.stretch,
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: <Widget>[
                                if (showProject)
                                  Padding(
                                    padding: EdgeInsets.only(left: 16),
                                    child: ProjectName(project: work.project),
                                  ),
                                // if (showLocation)
                                //   Padding(
                                //     padding: EdgeInsets.only(
                                //         left: 16, top: 8, bottom: 4),
                                //     child: Text(work.location.address),
                                //   ),
                                ListTile(
                                  contentPadding: EdgeInsets.only(left: 16),
                                  leading: work.isTaken ? CircleAvatar() : null,
                                  title: Text(
                                    work.isInvited
                                        ? 'Du er inviteret til denne vagt'
                                        : !work.isTaken
                                            ? work.isOwned
                                                ? Translations.of(context)
                                                    .infoWorkMissingReplacer
                                                : Translations.of(context)
                                                    .infoWorkMissingOwner
                                            : work.isReplaced
                                                ? work.workReplacement.contract
                                                    .user.displayName
                                                : (work.user.displayName +
                                                    (work.isUserAbsent
                                                        ? ' (${Translations.of(context).infoAbsent})'
                                                        : '')),
                                    maxLines: 2,
                                    overflow: TextOverflow.ellipsis,
                                    style: TextStyle(
                                        color: work.isInvited
                                            ? color
                                            : work.isUserAbsent || !work.isTaken
                                                ? style.declineRed
                                                : null,
                                        fontWeight: work.isInvited
                                            ? FontWeight.bold
                                            : null),
                                  ),
                                  subtitle: replacementWidget,
                                )
                              ],
                            ),
                    ),
                    Container(
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: <Widget>[
                          Text(dtOps.minsToHHmm(work.startTimeMins)),
                          Text(dtOps.minsToHHmm(work.endTimeMins)),
                        ],
                      ),
                    ),
                    //_buildColorBar(context),
                  ],
                ),
              ),
            ),
            if (work.isRegistered)
              SizedBox(
                //padding: EdgeInsets.only(left: 16, right: 8),
                width: 54,
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: <Widget>[
                    Text(
                      dtOps.minsToHHmm(work.workRegistration.startTimeMins),
                      style: TextStyle(color: Colors.white),
                    ),
                    Text(
                      dtOps.minsToHHmm(work.workRegistration.endTimeMins),
                      style: TextStyle(color: Colors.white),
                    ),
                  ],
                ),
              ),
            if (work.isElegibleForRegistration && work.canRegisterWork)
              SizedBox(
                // padding: EdgeInsets.only(left: 10),
                width: 54,
                child: Center(
                  child: FloatingActionButton(
                    heroTag: null,
                    mini: true,
                    backgroundColor: Colors.white,
                    onPressed:
                        onSelectTime != null ? () => onSelectTime(work) : null,
                    child: Icon(
                      Icons.timer,
                      color: color,
                    ),
                  ),
                ),
              ),
            if (work.isInvited)
              SizedBox(
                // padding: EdgeInsets.only(left: 10),
                width: 54,
                child: Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: <Widget>[
                      FloatingActionButton(
                        heroTag: null,
                        mini: true,
                        backgroundColor: Colors.white,
                        onPressed: onInviteAcceptSelect != null
                            ? () => onInviteAcceptSelect(work)
                            : null,
                        child: Icon(
                          Icons.check,
                          color: style.acceptGreen,
                        ),
                      ),
                      FloatingActionButton(
                        heroTag: null,
                        mini: true,
                        backgroundColor: Colors.white,
                        onPressed: onInviteDeclineSelect != null
                            ? () => onInviteDeclineSelect(work)
                            : null,
                        child: Icon(
                          Icons.cancel,
                          color: style.declineRed,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            if (!work.isRegistered &&
                !work.isElegibleForRegistration &&
                !work.isInvited)
              SizedBox(width: 6)
          ],
        ),
      ),
      onTap: onSelect != null ? () => onSelect(work) : null,
    );
  }
}
