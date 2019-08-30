import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';

import 'package:bms_flutter/src/widgets/user/widgets.dart';

Widget projectUsersEditFloatingActionButton(int projectId) {
  return Builder(
    builder: (context) {
      return Column(
        children: <Widget>[
          FloatingActionButton(
            heroTag: null,
            child: Icon(Icons.add),
            onPressed: () => showListOfUsersToAdd(context, projectId),
          ),
          Space(height: 8),
          FloatingActionButton(
            heroTag: null,
            child: Icon(Icons.remove),
            onPressed: () => showListOfUsersToRemove(context, projectId),
          ),
        ],
      );
    },
  );
}

void showListOfUsersToRemove(BuildContext context, int projectId) {
  UserListScreen.show(
    context,
    blocBuilder: (context) => UserListBloc(
      () => FetchOfProject(projectId: projectId),
    )..toggleSelectable(),
    onManySelected: (bloc, users) =>
        onLocationsSelectManyToRemove(context, projectId, bloc, users),
  );
}

void onLocationsSelectManyToRemove(
  BuildContext context,
  int projectId,
  UserListBloc bloc,
  List<User> users,
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
              title: Text('Remove users from project'),
              onTap: () {
                bloc.dispatch(RemoveSelectedFromProject(projectId: projectId));
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

void showListOfUsersToAdd(BuildContext context, int projectId) {
  UserListScreen.show(
    context,
    blocBuilder: (context) => UserListBloc(
      () => FetchAll(),
    )..toggleSelectable(),
    onManySelected: (bloc, users) =>
        onLocationsSelectManyToAdd(context, projectId, bloc, users),
  );
}

void onLocationsSelectManyToAdd(
  BuildContext context,
  int projectId,
  UserListBloc bloc,
  List<User> users,
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
              title: Text('Add users from project'),
              onTap: () {
                bloc.dispatch(AddSelectedToProject(projectId: projectId));
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
