import 'package:bms_dart/project_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';

import 'package:bms_flutter/src/widgets/project/widgets.dart';

Widget userProjectsEditFloatingActionButton(int userId) {
  return Builder(
    builder: (context) {
      return Column(
        children: <Widget>[
          FloatingActionButton(
            heroTag: null,
            child: Icon(Icons.add),
            onPressed: () => showListOfProjectsToAdd(context, userId),
          ),
          Space(height: 8),
          FloatingActionButton(
            heroTag: null,
            child: Icon(Icons.remove),
            onPressed: () => showListOfProjectsToRemove(context, userId),
          ),
        ],
      );
    },
  );
}

void showListOfProjectsToRemove(BuildContext context, int userId) {
  ProjectListScreen.show(
    context,
    blocBuilder: (context) => ProjectListBloc(
      () => ProjectListFetchOfUser(userId: userId),
    )..toggleSelectable(),
    onManySelected: (bloc, projects) =>
        onProjectsSelectManyToRemove(context, userId, bloc, projects),
  );
}

void onProjectsSelectManyToRemove(
  BuildContext context,
  int userId,
  ProjectListBloc bloc,
  List<Project> projects,
) {
  showModalBottomSheet(
    context: context,
    builder: (context) {
      return Container(
        height: 200,
        child: ListView(
          children: <Widget>[
            ListTile(
              leading: Icon(Icons.remove),
              title: Text('Remove user from projects'),
              onTap: () {
                bloc.dispatch(RemoveUserFromSelected(userId: userId));
                Navigator.of(context).pop();
              },
            ),
            ListTile(
              leading: Icon(Icons.arrow_back),
              title: Text(Translations.of(context).buttonBack),
              onTap: () {
                Navigator.of(context).pop();
              },
            ),
          ],
        ),
      );
    },
  );
}

void showListOfProjectsToAdd(BuildContext context, int userId) {
  ProjectListScreen.show(
    context,
    blocBuilder: (context) => ProjectListBloc(
      () => ProjectListFetchNotOfUser(userId: userId),
    )..toggleSelectable(),
    onManySelected: (bloc, projects) =>
        onProjectsSelectManyToAdd(context, userId, bloc, projects),
  );
}

void onProjectsSelectManyToAdd(
  BuildContext context,
  int userId,
  ProjectListBloc bloc,
  List<Project> projects,
) {
  showModalBottomSheet(
    context: context,
    builder: (context) {
      return Container(
        height: 200,
        child: ListView(
          children: <Widget>[
            ListTile(
              leading: Icon(Icons.add),
              title: Text('Add user to projects'),
              onTap: () {
                bloc.dispatch(AddUserToSelected(userId: userId));
              },
            ),
            ListTile(
              leading: Icon(Icons.arrow_back),
              title: Text(Translations.of(context).buttonBack),
              onTap: () {
                Navigator.of(context).pop();
              },
            ),
          ],
        ),
      );
    },
  );
}
