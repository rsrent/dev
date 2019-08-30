import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_list_bloc.dart';
import 'package:bms_flutter/translations.dart';
// import 'package:bms_flutter_admin/src/screens/work_create_update_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_flutter/style.dart' as style;
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/widgets.dart';
import 'select_user_and_contract.dart';
import 'package:bms_flutter/src/widgets/work/widgets.dart';

class WorkDrawer extends StatefulWidget {
  static void show(BuildContext context, Work work, WorkListBloc bloc) {
    showModalBottomSheet(
      context: context,
      builder: (context) {
        return Container(
          height: 500,
          child: WorkDrawer(
            bloc: bloc,
            work: work,
          ),
        );
      },
    );
  }

  final Work work;
  final WorkListBloc bloc;

  const WorkDrawer({Key key, this.work, this.bloc}) : super(key: key);
  @override
  _WorkDrawerState createState() => _WorkDrawerState();
}

class _WorkDrawerState extends State<WorkDrawer> {
  @override
  Widget build(BuildContext context) {
    var work = widget.work;
    var bloc = widget.bloc;

    return BlocListener(
        bloc: bloc,
        listener: (BuildContext context, state) {
          setState(() {});
        },
        child: ListView(
          padding: EdgeInsets.only(bottom: 100),
          children: <Widget>[
            ListTile(
              title: Text(Translations.of(context).dateString(work.date)),
            ),
            IgnorePointer(child: WorkTile(work: work)),
            ListTile(
              title: Text(Translations.of(context).labelComment),
              subtitle: Text(work.note),
            ),
            if (!work.isTaken)
              ListTile(
                title: Text('${work.inviteCount} brugere inviteret til vagten'),
              ),
            ListTile(
              leading: Icon(Icons.edit),
              title: Text(Translations.of(context).titleUpdateWork),
              onTap: () {
                return Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) {
                      return WorkCreateUpdateScreen(workToUpdate: work);
                    },
                  ),
                );
              },
            ),
            if (work.isElegibleForRegistration)
              ListTile(
                leading: Icon(Icons.check),
                title: Text(Translations.of(context).optionRegisterWork),
                onTap: () {
                  registerWork(context, work, bloc);
                },
              ),
            if (work.isTaken && !work.isReplaced)
              ListTile(
                leading: Icon(Icons.remove_circle),
                title: Text(Translations.of(context).optionRemoveWorkUser),
                onTap: () => bloc.dispatch(WorkListRemoveUser(workId: work.id)),
              ),
            if (work.isReplaced)
              ListTile(
                leading: Icon(Icons.remove_circle_outline),
                title: Text(Translations.of(context).optionRemoveWorkReplacer),
                onTap: () =>
                    bloc.dispatch(WorkListRemoveReplacer(workId: work.id)),
              ),
            if (!work.isOwned && !work.isTaken)
              ListTile(
                leading: Icon(Icons.person),
                title: Text(Translations.of(context).optionFindWorkOwner),
                onTap: () async {
                  // print('work.locationId: ${work.location.id}');
                  var userContract = await selectUserContract(
                      context,
                      FetchOfProjectAvailableOnDate(
                          projectId: work.project.id, date: work.date));
                  //print('userContract: ${userContract.second.id}');
                  if (userContract != null) {
                    bloc.dispatch(WorkListAddUserContract(
                      work: work,
                      user: userContract.first,
                      contract: userContract.second,
                    ));
                  }
                },
              ),
            if (work.isOwned && !work.isTaken)
              ListTile(
                leading: Icon(Icons.person_outline),
                title: Text(Translations.of(context).optionFindWorkReplacer),
                onTap: () async {
                  var userContract = await selectUserContract(
                      context,
                      FetchOfProjectAvailableOnDate(
                          projectId: work.project.id, date: work.date));

                  if (userContract != null) {
                    bloc.dispatch(WorkListReplaceUserContract(
                      work: work,
                      user: userContract.first,
                      contract: userContract.second,
                    ));
                  }
                },
              ),
            if (!work.isTaken)
              ListTile(
                leading: Icon(Icons.person),
                title: Text('Inviter bruger til vagt'),
                onTap: () async {
                  // print('work.locationId: ${work.location.id}');
                  var userContract = await selectUserContract(
                      context,
                      FetchOfProjectAvailableOnDate(
                          projectId: work.project.id, date: work.date));
                  print('userContract: ${userContract.second.id}');
                  if (userContract != null) {
                    bloc.dispatch(WorkListInviteUserContract(
                      work: work,
                      user: userContract.first,
                      contract: userContract.second,
                    ));
                  }
                },
              ),
            ListTile(
              leading: Icon(Icons.keyboard_arrow_left),
              title: Text(Translations.of(context).buttonBack),
              onTap: () => Navigator.of(context).pop(),
            ),
          ],
        ));
  }
}
