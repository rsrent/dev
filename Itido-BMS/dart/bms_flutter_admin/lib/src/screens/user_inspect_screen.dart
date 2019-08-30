import 'package:bms_dart/accident_report_list_bloc.dart';
import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/contract_list_bloc.dart';
import 'package:bms_dart/absence_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/project_list_bloc.dart';
import 'package:bms_dart/work_contract_list_bloc.dart';
import 'package:bms_dart/work_list_bloc.dart';
import 'package:bms_flutter_admin/src/widgets/user_locations_edit.dart';
import 'package:bms_flutter_admin/src/widgets/work_contract_drawer.dart';
import 'package:bms_flutter_admin/src/widgets/work_drawer.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:bms_flutter/translations.dart';
// import 'location_inspect_screen.dart';

import 'package:bms_flutter/src/widgets/project/widgets.dart';
import 'package:bms_flutter/src/widgets/user/widgets.dart';
import 'package:bms_flutter/src/widgets/absence/widgets.dart';
import 'package:bms_flutter/src/widgets/contract/widgets.dart';
import 'package:bms_flutter/src/widgets/accident_report/widgets.dart';
import 'package:bms_flutter/src/widgets/work/widgets.dart';
import 'package:bms_flutter/src/widgets/work_contract/widgets.dart';

import 'project_inspect_screen.dart';

class UserInspectScreen extends StatefulWidget {
  static void show(BuildContext context, int userId, {int startIndex}) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (context) =>
            UserInspectScreen(userId: userId, startIndex: startIndex),
      ),
    );
  }

  final int userId;
  final int startIndex;

  const UserInspectScreen({Key key, @required this.userId, this.startIndex})
      : super(key: key);

  @override
  _UserInspectScreenState createState() => _UserInspectScreenState();
}

class _UserInspectScreenState extends State<UserInspectScreen> {
  var queryResultBloc = QueryResultBloc();

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      builder: (context) =>
          UserInspectBloc(widget.userId)..dispatch(UserInspectEventFetch()),
      child: Builder(
        builder: (context) {
          var bloc = BlocProvider.of<UserInspectBloc>(context);
          return AnimatedBlocBuilder(
            bloc: bloc,
            builder: (context, state) {
              if (state is LoadedUserInspectState) {
                return _build(context, state.user);
              } else if (state is ErrorUserInspectState) {
                return ErrorLoadingScreen(
                  info: Translations.of(context).infoErrorLoading,
                  onRetre: () => bloc.dispatch(UserInspectEventFetch()),
                );
              } else {
                return LoadingScreen();
              }
            },
          );
        },
      ),
    );
  }

  Widget _build(BuildContext context, User user) {
    return Scaffold(
      body: QueryResultScreen(
        blocs: [
          queryResultBloc,
        ],
        child: InspectScreen(
          startIndex: widget.startIndex,
          backgroundColor: Colors.white,
          title: user.displayName,
          child: _Background(user: user),
          items: [
            InspectScreenItem(
              icon: Icon(Icons.work),
              child: WorkListScreen(
                blocBuilder: (context) => WorkListBloc(
                  () => WorkListFetchOfUser(userId: widget.userId),
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                ),
                showUser: false,
                onSelect: (work, bloc) => WorkDrawer.show(context, work, bloc),
                onSelectTime: (work, bloc) => registerWork(context, work, bloc),
              ),
            ),
            InspectScreenItem(
              icon: Icon(Icons.contact_mail),
              child: WorkContractListScreen(
                blocBuilder: (context) => WorkContractListBloc(
                  () => WorkContractListFetchOfUser(userId: widget.userId),
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                ),
                showUser: false,
                onSelect: (workContract, bloc) =>
                    WorkContractDrawer.show(context, workContract, bloc),
              ),
            ),
            InspectScreenItem(
              icon: Icon(Icons.person_outline),
              child: AbsenceListScreen(
                blocBuilder: (context) => AbsenceListBloc(
                  () => AbsenceListFetchOfUser(userId: user.id),
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                ),
                onSelect: (absence) {
                  AbsenceCreateUpdateScreen.show(context, absence: absence);
                },
                floatingActionButton: FloatingActionButton(
                  heroTag: null,
                  child: Icon(Icons.add),
                  onPressed: () {
                    AbsenceCreateUpdateScreen.show(context, userId: user.id);
                  },
                ),
              ),
            ),
            InspectScreenItem(
              icon: Icon(Icons.assignment_ind),
              child: ContractListScreen(
                blocBuilder: (context) => ContractListBloc(
                  () => ContractListFetchOfUser(userId: user.id),
                ),
                onSelect: (contract) {
                  ContractCreateUpdateScreen.show(context, contract: contract);
                },
                floatingActionButton: FloatingActionButton(
                  heroTag: null,
                  child: Icon(Icons.add),
                  onPressed: () {
                    ContractCreateUpdateScreen.show(context, userId: user.id);
                  },
                ),
              ),
            ),
            InspectScreenItem(
              icon: Icon(Icons.warning),
              child: AccidentReportListScreen(
                blocBuilder: (context) => AccidentReportListBloc(
                  () => AccidentReportListFetchOfUser(userId: user.id),
                ),
                onSelect: (accidentReport) {
                  AccidentReportCreateUpdateScreen.show(context,
                      accidentReport: accidentReport);
                },
                floatingActionButton: FloatingActionButton(
                  heroTag: null,
                  child: Icon(Icons.add),
                  onPressed: () =>
                      showAccidenReportCreateModule(context, user.id),
                ),
              ),
            ),
            InspectScreenItem(
              icon: Icon(Icons.group_work),
              child: ProjectListScreen(
                blocBuilder: (context) => ProjectListBloc(
                  () => ProjectListFetchOfUser(userId: widget.userId),
                ),
                floatingActionButton:
                    userProjectsEditFloatingActionButton(user.id),
                onSelect: (location) =>
                    ProjectInspectScreen.show(context, location.id),
              ),
            ),
          ],
        ),
      ),
    );
  }

  static void showAccidenReportCreateModule(BuildContext context, int userId) {
    showModalBottomSheet(
      context: context,
      builder: (context) {
        return Container(
          height: 200,
          child: ListView(
            children: <Widget>[
              ListTile(
                title: Text(Translations.of(context).titleCreateAccidentReport),
                leading: Icon(Icons.add),
                onTap: () => AccidentReportCreateUpdateScreen.show(context,
                    userId: userId,
                    accidentReportType: AccidentReportType.Accident),
              ),
              ListTile(
                title: Text(
                    Translations.of(context).titleCreateAlmostAccidentReport),
                leading: Icon(Icons.add),
                onTap: () => AccidentReportCreateUpdateScreen.show(context,
                    userId: userId,
                    accidentReportType: AccidentReportType.AlmostAccident),
              ),
            ],
          ),
        );
      },
    );
  }
}

class _Background extends StatefulWidget {
  final User user;

  const _Background({Key key, @required this.user}) : super(key: key);

  @override
  __BackgroundState createState() => __BackgroundState();
}

class __BackgroundState extends State<_Background> {
  @override
  Widget build(BuildContext context) {
    var user = widget.user;
    var bloc = BlocProvider.of<UserInspectBloc>(context);
    return ListView(
      padding: EdgeInsets.only(bottom: 200, left: 24, right: 24, top: 24),
      children: <Widget>[
        CircleAvatar(
          radius: 60,
        ),
        ListTile(
          leading: Icon(Icons.edit),
          title: Text('Opdater'),
          onTap: () {
            UserCreateUpdateScreen.show(context, user: user).then((updated) {
              if (updated) {
                bloc.dispatch(UserInspectEventFetch());
              }
            });
          },
        ),
        Divider(),
        if (user.disabled)
          ListTile(
            leading: Icon(Icons.clear),
            title: Text('Ikke aktiv'),
            subtitle: Text('Tryk for at aktivere'),
            //onTap: () => bloc.dispatch(EnableDisableLocation()),
          ),
        if (!user.disabled)
          ListTile(
            leading: Icon(Icons.check),
            title: Text('Aktiv'),
            subtitle: Text('Tryk for at deaktivere'),
            //onTap: () => bloc.dispatch(EnableDisableLocation()),
          ),
        Divider(color: Colors.white),
        ListTile(
          title: Text('Navn'),
          subtitle: Text(user.displayName),
        ),
        if (user.email != null)
          ListTile(
            title: Text('Email'),
            subtitle: Text(user.email),
          ),
        if (user.phone != null)
          ListTile(
            title: Text('Phone'),
            subtitle: Text(user.phone),
          ),
      ],
    );
  }
}
