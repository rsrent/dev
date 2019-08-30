import 'package:bms_dart/models.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/project_item_list_bloc.dart';

void showProjectItems(BuildContext context, int projectId) {
  Navigator.of(context).push(
    MaterialPageRoute(
      builder: (context) {
        return ProjectItemList(
          projectId: projectId,
        );
      },
    ),
  );
}

class ProjectItemList extends StatefulWidget {
  final int projectId;

  const ProjectItemList({Key key, this.projectId}) : super(key: key);
  @override
  _ProjectItemListState createState() => _ProjectItemListState();
}

class _ProjectItemListState extends State<ProjectItemList> {
  List<ProjectItemAccess> projectItemAccesses = List<ProjectItemAccess>();
  // List<String> accessChanges;
  Map<String, bool> loading = {};
  ProjectItemListBloc _bloc;
  List<ProjectItem> projectItems;
  List<ProjectRole> projectRoles;

  @override
  void initState() {
    Future.delayed(Duration.zero, () async {
      print(0);
      print(1);
      projectRoles =
          await repositoryProvider.projectRoleRepository().fetchProjectRoles();
      print(2);
      print(3);
      _bloc = ProjectItemListBloc(
        () =>
            ProjectItemListFetchDetailedOfProject(projectId: widget.projectId),
      );
      setState(() {});
    });

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    if (projectRoles == null)
      return Container(
        child: Center(
          child: CircularProgressIndicator(),
        ),
      );
    return Scaffold(
      appBar: AppBar(),
      body: BlocListener(
        bloc: _bloc,
        listener: (context, state) {
          if (state is Loaded<ProjectItem>) {
            // projectItems = state.items;
            // if (accessChanges == null) {
            //   accessChanges = state.items.map((pi) => '${pi.access}').toList();
            //   loading = List.generate(state.items.length, (i) => false);
            // }

            state.items.forEach((item) {
              loading['${item.id}'] = false;

              item.projectItemAccesses.forEach((access) {
                if (!projectItemAccesses.any((pia) =>
                    pia.projectItemID == access.projectItemID &&
                    pia.projectRoleID == access.projectRoleID)) {
                  projectItemAccesses.add(ProjectItemAccess(
                    projectItemID: access.projectItemID,
                    projectRoleID: access.projectRoleID,
                    read: access.read,
                    write: access.write,
                  ));
                }
              });
            });

            // loading = List.generate(
            //     state.items.length,
            //     (i) => state.items[i].access == accessChanges[i]
            //         ? false
            //         : loading[i]);
          }
        },
        child: BlocProvider(
          builder: (context) => _bloc,
          child: AnimatedBlocBuilder(
            bloc: _bloc,
            builder: (context, ListState<ProjectItem> state) {
              var currentState = state;

              if (state is Failure) {
                return InfoListView(
                    info: Translations.of(context).infoErrorLoading);
              }

              if (currentState is Loaded<ProjectItem>) {
                projectItems = currentState.items;
                // if (accessChanges == null) {
                //   accessChanges =
                //       projectItems.map((pi) => '${pi.access}').toList();
                //   loading = List.generate(projectItems.length, (i) => false);
                // }

                return ListView.builder(
                  itemCount: projectItems.length,
                  itemBuilder: (context, index) {
                    var item = projectItems[index];

                    var accessItems = item.projectItemAccesses;

                    var isDifferent = accessItems.any((ai) =>
                        projectItemAccesses.firstWhere(
                            (pia) =>
                                pia.projectItemID == ai.projectItemID &&
                                pia.projectRoleID == ai.projectRoleID &&
                                pia.read == ai.read &&
                                pia.write == ai.write,
                            orElse: () => null) ==
                        null);

                    var isLoading = loading['${item.id}'];

                    var changedItemAccesses = projectItemAccesses
                        .where((pia) => pia.projectItemID == item.id)
                        .toList();

                    //var accessChange = accessChanges[index];

                    //var permissions = accessChange.split('');
                    return Card(
                      child: ExpansionTile(
                        leading: Icon(
                          item.projectItemType == ProjectItemType.client
                              ? Icons.business
                              : item.projectItemType == ProjectItemType.comment
                                  ? Icons.comment
                                  : item.projectItemType ==
                                          ProjectItemType.documentFolders
                                      ? Icons.folder
                                      : item.projectItemType ==
                                              ProjectItemType
                                                  .firestoreConversations
                                          ? Icons.chat
                                          : item.projectItemType ==
                                                  ProjectItemType.location
                                              ? Icons.location_on
                                              : item.projectItemType ==
                                                      ProjectItemType.logs
                                                  ? Icons.assignment
                                                  : item.projectItemType ==
                                                          ProjectItemType
                                                              .profileImage
                                                      ? Icons.image
                                                      : item.projectItemType ==
                                                              ProjectItemType
                                                                  .qualityReports
                                                          ? Icons
                                                              .assignment_late
                                                          : item.projectItemType ==
                                                                  ProjectItemType
                                                                      .tasks
                                                              ? Icons.extension
                                                              : item.projectItemType ==
                                                                      ProjectItemType
                                                                          .work
                                                                  ? Icons.work
                                                                  : item.projectItemType == ProjectItemType.address
                                                                      ? Icons.location_on
                                                                      : Icons.next_week,
                        ),
                        trailing: isLoading
                            ? CircularProgressIndicator()
                            : isDifferent
                                ? IconButton(
                                    icon: Icon(Icons.cloud_upload),
                                    onPressed: () async {
                                      //print()
                                      loading['${item.id}'] = true;
                                      setState(() {});
                                      // item.access = accessChange;
                                      _bloc.dispatch(
                                          ProjectItemListUpdateAccessOfItem(
                                              projectItemId: item.id,
                                              access: changedItemAccesses));
                                      //await Future.delayed(Duration(seconds: 1));

                                      // setState(() {});
                                    },
                                  )
                                : null,
                        title:
                            Text(item.name ?? item.projectItemType.toString()),
                        children: [
                          ListTile(
                            title: Row(
                              children: <Widget>[
                                Expanded(child: Container()),
                                Text('Read'),
                                Space(
                                  width: 12,
                                ),
                                Text('Write'),
                              ],
                            ),
                          ),
                        ]..addAll(List.generate(
                            changedItemAccesses.length,
                            (i) {
                              var changedAccess = changedItemAccesses[i];
                              print('Check for projectRole');
                              var role = projectRoles.firstWhere(
                                  (pr) => pr.id == changedAccess.projectRoleID);

                              print(role.toMap());
                              print('That was the result');
                              return ListTile(
                                title: Row(
                                  children: <Widget>[
                                    Text(role?.name ??
                                        '${changedAccess.projectRoleID}'),
                                    Expanded(child: Container()),
                                    Checkbox(
                                      value: changedAccess.read,
                                      onChanged: (isOn) {
                                        changedAccess.read = isOn;
                                        if (!isOn) changedAccess.write = false;
                                        setState(() {});
                                      },
                                    ),
                                    Checkbox(
                                      value: changedAccess.write,
                                      onChanged: (isOn) {
                                        changedAccess.write = isOn;
                                        if (isOn) changedAccess.read = true;
                                        setState(() {});
                                      },
                                    ),
                                  ],
                                ),
                              );
                            },
                          )

                              // List.generate(
                              //   permissions.length ~/ 2,
                              //   (i) {
                              //     var realI = i * 2;
                              //     var read = permissions[realI];
                              //     var write = permissions[realI + 1];

                              //     var roleChar = i == 0
                              //         ? 'A'
                              //         : i == 1
                              //             ? 'M'
                              //             : i == 2 ? 'U' : i == 3 ? 'C' : 'E';

                              //     var title = i == 0
                              //         ? 'Admin'
                              //         : i == 1
                              //             ? 'Manager'
                              //             : i == 2
                              //                 ? 'User'
                              //                 : i == 3
                              //                     ? 'ClientAdmin'
                              //                     : 'ClientManager';

                              //     return ListTile(
                              //       title: Row(
                              //         children: <Widget>[
                              //           Text(title),
                              //           Expanded(child: Container()),
                              //           Checkbox(
                              //             value: read == 'R',
                              //             onChanged: (isOn) {
                              //               updateAccess2(
                              //                   isOn ? 'R' : '-', index, realI);
                              //               if (!isOn)
                              //                 updateAccess2(isOn ? 'W' : '-',
                              //                     index, realI + 1);
                              //             },
                              //           ),
                              //           Checkbox(
                              //             value: write == 'W',
                              //             onChanged: (isOn) {
                              //               updateAccess2(isOn ? 'W' : '-',
                              //                   index, realI + 1);
                              //               if (isOn)
                              //                 updateAccess2(isOn ? 'R' : '-',
                              //                     index, realI);
                              //             },
                              //           ),
                              //         ],
                              //       ),
                              //     );
                              //   },
                              // ),
                              ),
                      ),
                    );
                  },
                );
              }
            },
          ),
        ),
      ),
    );
  }

  // void updateAccess2(String permission, int index, int user) {
  //   var update = accessChanges[index].replaceRange(user, user + 1, permission);
  //   print(update);
  //   accessChanges[index] = update;
  //   setState(() {});
  // }
}
