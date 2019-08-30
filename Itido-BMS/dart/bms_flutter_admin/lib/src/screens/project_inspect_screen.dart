import 'dart:async';

import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/folder_list_bloc.dart';
import 'package:bms_dart/project_list_bloc.dart';
import 'package:bms_dart/quality_report_item_list_bloc.dart';
import 'package:bms_dart/quality_report_list_bloc.dart';
import 'package:bms_dart/task_completed_list_bloc.dart';
import 'package:bms_dart/task_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_contract_list_bloc.dart';
import 'package:bms_dart/work_list_bloc.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:bms_flutter_admin/src/screens/user_inspect_screen.dart';
import 'package:bms_flutter_admin/src/widgets/document_of_folder_screen.dart';
import 'package:bms_flutter_admin/src/widgets/post_create_dialog.dart';
import 'package:bms_flutter_admin/src/widgets/project_items.dart';
import 'package:bms_flutter_admin/src/widgets/project_users_edit.dart';
import 'package:bms_flutter_admin/src/widgets/work_contract_drawer.dart';
import 'package:bms_flutter_admin/src/widgets/work_drawer.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/translations.dart';

import 'package:bms_flutter/src/widgets/folder/widgets.dart';
import 'package:bms_flutter/src/widgets/address/widgets.dart';
import 'package:bms_flutter/src/widgets/comment/widgets.dart';
import 'package:bms_flutter/src/widgets/project/widgets.dart';
import 'package:bms_flutter/src/widgets/task/widgets.dart';
import 'package:bms_flutter/src/widgets/task_completed/widgets.dart';
import 'package:bms_flutter/src/widgets/user/widgets.dart';
import 'package:bms_flutter/src/widgets/log/widgets.dart';
import 'package:bms_flutter/src/widgets/work/widgets.dart';
import 'package:bms_flutter/src/widgets/work_contract/widgets.dart';
import 'package:bms_flutter/src/widgets/quality_report/widgets.dart';
import 'package:bms_flutter/src/widgets/quality_report_item/widgets.dart';

class ProjectInspectScreen extends StatefulWidget {
  static void show(BuildContext context, int projectId, {int startIndex}) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (context) {
          return ProjectInspectScreen(
              projectId: projectId, startIndex: startIndex);
        },
      ),
    );
  }

  final int projectId;
  final int startIndex;

  const ProjectInspectScreen({
    Key key,
    @required this.projectId,
    this.startIndex,
  }) : super(key: key);

  @override
  _ProjectInspectScreenState createState() => _ProjectInspectScreenState();
}

class _ProjectInspectScreenState extends State<ProjectInspectScreen> {
  Completer loading;

  var queryResultBloc = QueryResultBloc();

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      builder: (context) => ProjectInspectBloc(
        widget.projectId,
        queryResultBloc: queryResultBloc,
        sprog: () => Translations.of(context),
      )..dispatch(ProjectInspectEventFetch()),
      child: Builder(
        builder: (context) {
          var bloc = BlocProvider.of<ProjectInspectBloc>(context);
          return AnimatedBlocBuilder(
            bloc: bloc,
            builder: (context, state) {
              if (state is LoadedProjectInspectState && state.project != null) {
                return _build(context, state);
              } else if (state is ErrorProjectInspectState) {
                return ErrorLoadingScreen(
                  info: Translations.of(context).infoErrorLoading,
                  onRetre: () => bloc.dispatch(ProjectInspectEventFetch()),
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

  Widget _build(BuildContext context, LoadedProjectInspectState state) {
    var profileImageItem = state.projectItems.firstWhere(
        (pi) => pi.projectItemType == ProjectItemType.profileImage,
        orElse: () => null);

    var addressItem = state.projectItems.firstWhere(
        (pi) => pi.projectItemType == ProjectItemType.address,
        orElse: () => null);

    var commentItems = state.projectItems
        .where((pi) => pi.projectItemType == ProjectItemType.comment)
        .toList();

    var taskItem = state.projectItems.firstWhere(
        (pi) => pi.projectItemType == ProjectItemType.tasks,
        orElse: () => null);

    var qualityReportItem = state.projectItems.firstWhere(
        (pi) => pi.projectItemType == ProjectItemType.qualityReports,
        orElse: () => null);

    var logsItem = state.projectItems.firstWhere(
        (pi) => pi.projectItemType == ProjectItemType.logs,
        orElse: () => null);

    var workItem = state.projectItems.firstWhere(
        (pi) => pi.projectItemType == ProjectItemType.work,
        orElse: () => null);

    var documentItems = state.projectItems
        .where((pi) => pi.projectItemType == ProjectItemType.documentFolders)
        .toList();

    var postItems = state.projectItems
        .where((pi) => pi.projectItemType == ProjectItemType.post)
        .toList();

    var inspectBloc = BlocProvider.of<ProjectInspectBloc>(context);

    return BlocListener(
      bloc: inspectBloc,
      listener: (context, dynamic state) {
        if (state is LoadedProjectInspectState) {
          if (!(loading?.isCompleted ?? true)) loading?.complete();
        }
      },
      child: Scaffold(
        body: QueryResultScreen(
          blocs: [
            queryResultBloc,
          ],
          child: InspectScreen(
            backgroundColor: Colors.white,
            startIndex: widget.startIndex,
            title: state.project.name,
            child: RefreshIndicator(
              onRefresh: () async {
                loading = Completer();
                inspectBloc.dispatch(ProjectInspectEventFetch());
                await loading.future;
              },
              child: Stack(
                children: <Widget>[
                  Positioned.fill(
                    child: ListView(
                      children: <Widget>[
                        if (profileImageItem != null)
                          CircleAvatar(
                            radius: 60,
                          ),
                        if (addressItem != null)
                          Column(
                            children: <Widget>[
                              ListTile(
                                leading: Icon(Icons.location_on),
                                title: Text(addressItem.address.addressName ??
                                    'Address'),
                                subtitle: Placeholder(
                                  fallbackHeight: 200,
                                  color: Colors.yellow,
                                ),
                                onTap: () {
                                  AddressCreateUpdateScreen.show(context,
                                      address: addressItem.address);
                                },
                              ),
                              Divider(),
                            ],
                          ),
                        for (int c = 0; c < commentItems.length; c++)
                          Column(
                            children: <Widget>[
                              ListTile(
                                leading: Icon(Icons.comment),
                                title: Text(
                                    commentItems[c].comment.title ?? 'Title'),
                                subtitle: Text(
                                    commentItems[c].comment.body ?? 'Comment'),
                                onTap: () {
                                  CommentCreateUpdateScreen.show(context,
                                      comment: commentItems[c].comment);
                                },
                              ),
                              Divider(),
                            ],
                          ),
                        Space(height: 200),
                      ],
                    ),
                  ),
                  Positioned(
                    bottom: 100,
                    right: 20,
                    child: FloatingActionButton(
                      heroTag: null,
                      child: Icon(Icons.add),
                      onPressed: () => addToProject(context, state),
                    ),
                  ),
                  Positioned(
                    bottom: 100,
                    right: 80,
                    child: FloatingActionButton(
                      heroTag: null,
                      child: Icon(Icons.edit),
                      onPressed: () =>
                          showProjectItems(context, widget.projectId),
                    ),
                  ),
                ],
              ),
            ),
            items: [
              // List of child projects
              if (state.project.childrenCount > 0)
                InspectScreenItem(
                  icon: Icon(Icons.group_work),
                  child: ProjectListScreen(
                    blocBuilder: (context) => ProjectListBloc(
                      () => ProjectListFetchOfProject(
                          projectId: widget.projectId),
                    ),
                    onSelect: (project) =>
                        ProjectInspectScreen.show(context, project.id),
                    floatingActionButton: Builder(
                      builder: (context) {
                        return FloatingActionButton(
                          heroTag: null,
                          child: Icon(Icons.add),
                          onPressed: () async {
                            var bloc =
                                BlocProvider.of<ProjectListBloc>(context);
                            var text = await showTextFieldDialog(
                              context,
                              title: 'Navn på projekt',
                              hint: 'Projektets navn...',
                            );
                            if (text != null)
                              bloc.dispatch(ProjectListAddNew(
                                  name: text, projectId: state.project.id));
                          },
                        );
                      },
                    ),
                  ),
                ),
              // List of project users
              InspectScreenItem(
                icon: Icon(Icons.group),
                child: UserListScreen(
                  blocBuilder: (context) => UserListBloc(
                    () => FetchOfProject(projectId: widget.projectId),
                  ),
                  onSelect: (user) => UserInspectScreen.show(context, user.id),
                  floatingActionButton:
                      projectUsersEditFloatingActionButton(widget.projectId),
                ),
              ),
              // List of tasks
              if (taskItem != null)
                _buildTaskItem(taskItem),
              if (qualityReportItem != null)
                _buildQualityReportItem(qualityReportItem),
              if (workItem != null)
                _buildWorkItem(workItem),
              if (workItem != null)
                _buildWorkContractItem(workItem),
              if (logsItem != null)
                _buildLogsItem(logsItem),
              if (documentItems.length > 0)
                _buildDocumentFoldersItem(documentItems),
              if (postItems.length > 0)
                _buildPostItem(postItems),
            ],
          ),
        ),
      ),
    );
  }

  void onTaskSelected(BuildContext context, Task task) {
    showModalBottomSheet(
        context: context,
        builder: (context) {
          return Container(
            height: 400,
            child: ListView(
              children: <Widget>[
                Space(height: 8),
                TaskTile(task: task),
                ListTile(
                  title: Text('Rediger'),
                  onTap: () {
                    TaskCreateUpdateScreen.show(context, task: task);
                  },
                ),
                ListTile(
                  title: Text('Se udførte'),
                  onTap: () {
                    TaskCompletedListScreen.show(
                      context,
                      blocBuilder: (context) => TaskCompletedListBloc(
                        () => TaskCompletedListFetchOfTask(taskId: task.id),
                      ),
                      onSelect: (taskCompleted) {
                        TaskCompletedCreateUpdateScreen.show(
                          context,
                          taskCompleted: taskCompleted,
                        );
                      },
                      floatingActionButton: FloatingActionButton(
                        heroTag: null,
                        child: Icon(Icons.add),
                        onPressed: () {
                          TaskCompletedCreateUpdateScreen.show(
                            context,
                            taskId: task.id,
                          );
                        },
                      ),
                    );
                  },
                ),
              ],
            ),
          );
        });
  }

  InspectScreenItem _buildTaskItem(ProjectItem item) {
    return InspectScreenItem(
      icon: Icon(Icons.extension),
      child: TaskListScreen(
        blocBuilder: (context) => TaskListBloc(
          () => TaskListFetchOfProjectItem(projectItemId: item.id),
        ),
        onSelect: (task) => onTaskSelected(context, task),
        floatingActionButton: FloatingActionButton(
          heroTag: null,
          child: Icon(Icons.add),
          onPressed: () {
            TaskCreateUpdateScreen.show(context, projectId: item.id);
          },
        ),
      ),
    );
  }

  InspectScreenItem _buildQualityReportItem(ProjectItem renameItem) {
    var _qrBloc = QualityReportListBloc(
      () => QualityReportListFetchOfProjectItem(projectItemId: renameItem.id),
    );

    return InspectScreenItem(
      icon: Icon(Icons.assignment_late),
      child: QualityReportListScreen(
        blocBuilder: (context) => _qrBloc,
        onSelect: (qr) {
          var qrItemBloc = QualityReportItemListBloc(
            () => FetchOfQualityReport(qualityReportId: qr.id),
          );

          print('qr.completedTime: ${qr.completedTime}');
          QualityReportItemListScreen.show(context,
              actions: [
                if (qr.completedTime == null)
                  Builder(
                    builder: (context) {
                      return IconButton(
                        icon: Icon(Icons.check),
                        onPressed: () {
                          _qrBloc.dispatch(QualityReportListComplete(
                              qualityReportId: qr.id));
                        },
                      );
                    },
                  ),
              ],
              blocBuilder: (context) => qrItemBloc,
              onRatingSelect: (item, rating) {
                qrItemBloc.dispatch(UpdateQualityReportItem(
                  qualityReportItem: item..rating = rating,
                ));
              },
              onCommentSelect: (item) async {
                var comment = await showTextFieldDialog(
                  context,
                  title: 'Kommentar',
                  hint: 'Skriv kommentar til opgaven',
                  startText: item.comment,
                );
                if (comment != null) {
                  qrItemBloc.dispatch(UpdateQualityReportItem(
                      qualityReportItem: item..comment = comment));
                }
              });
        },
        /*onSelect: (task) => onQualityReportSelected(context, task),*/
        floatingActionButton: Builder(
          builder: (context) {
            return FloatingActionButton(
              heroTag: null,
              child: Icon(Icons.add),
              onPressed: () {
                print('Hallo');

                var _bloc = BlocProvider.of<QualityReportListBloc>(context);
                _bloc.dispatch(
                    QualityReportListCreateNew(projectItemId: renameItem.id));
                //QualityReportCreateUpdateScreen.show(context, projectId: item.id);
              },
            );
          },
        ),
      ),
    );
  }

  InspectScreenItem _buildLogsItem(ProjectItem item) {
    return InspectScreenItem(
      icon: Icon(Icons.assignment),
      child: LogListScreen(
        blocBuilder: (context) => LogListBloc(
          () => LogListFetchOfProjectItem(projectItemId: item.id),
        ),
        floatingActionButton: Builder(
          builder: (context) {
            return FloatingActionButton(
              heroTag: null,
              child: Icon(Icons.add),
              onPressed: () {
                BlocProvider.of<LogListBloc>(context)
                    .dispatch(LogListAddNew(projectItemId: item.id));
              },
            );
          },
        ),
        onSelect: (log) => LogCreateUpdateScreen.show(context, log: log),
      ),
    );
  }

  InspectScreenItem _buildWorkItem(ProjectItem item) {
    return InspectScreenItem(
      icon: Icon(Icons.work),
      child: WorkListScreen(
        blocBuilder: (context) => WorkListBloc(
          () => WorkListFetchOfProjectItem(projectItemId: item.id),
          queryResultBloc: queryResultBloc,
          sprog: () => Translations.of(context),
        ),
        showProject: false,
        floatingActionButton: FloatingActionButton(
          heroTag: null,
          child: Icon(Icons.add),
          onPressed: () {
            Navigator.of(context).push(
              MaterialPageRoute(
                builder: (context) => WorkCreateUpdateScreen(
                  projectItemId: item.id,
                ),
              ),
            );
          },
        ),
        onSelect: (work, bloc) => WorkDrawer.show(context, work, bloc),
        onSelectTime: (work, bloc) => registerWork(context, work, bloc),
        onInviteAcceptSelect: (work, bloc) async {
          var answer = await showConfirmationDialog(
            context,
            title: 'Accepter vagten',
            body:
                'Du er inviteret til at tage denne vagt, tryk for at tage vagten',
            okButtonText: 'Tag vagten',
            cancelButtonText: 'Tilbage',
          );
          if (answer) {
            bloc.dispatch(WorkListReplyToInvite(answer: true, work: work));
          }
        },
        onInviteDeclineSelect: (work, bloc) async {
          print('Hallo');
          var answer = await showConfirmationDialog(
            context,
            title: 'Afslå vagten',
            body:
                'Du er inviteret til at tage denne vagt, tryk for at takke nej til vagten',
            okButtonText: 'Afslå invitation',
            cancelButtonText: 'Tilbage',
          );
          if (answer) {
            bloc.dispatch(WorkListReplyToInvite(answer: false, work: work));
          }
        },
      ),
    );
  }

  InspectScreenItem _buildWorkContractItem(ProjectItem item) {
    return InspectScreenItem(
      icon: Icon(Icons.next_week),
      child: WorkContractListScreen(
        blocBuilder: (context) => WorkContractListBloc(
          () => WorkContractListFetchOfProjectItem(proejctItemId: item.id),
          queryResultBloc: queryResultBloc,
          sprog: () => Translations.of(context),
        ),
        showProject: false,
        floatingActionButton: FloatingActionButton(
          heroTag: null,
          child: Icon(Icons.add),
          onPressed: () {
            Navigator.of(context).push(
              MaterialPageRoute(
                builder: (context) => WorkContractCreateUpdateScreen(
                  projectItemId: item.id,
                ),
              ),
            );
          },
        ),
        onSelect: (workContract, bloc) =>
            WorkContractDrawer.show(context, workContract, bloc),
      ),
    );
  }

  InspectScreenItem _buildDocumentFoldersItem(List<ProjectItem> items) {
    return InspectScreenItem(
      icon: Icon(Icons.folder),
      child: Stack(
        children: <Widget>[
          Positioned.fill(
            child: ListView.builder(
              itemCount: items.length,
              itemBuilder: (context, index) {
                var item = items[index];
                return ListTile(
                  leading: Icon(Icons.folder),
                  title: Text('${item.documentFolder['title']}'),
                  onTap: () {
                    documentOfFolderScreen(context, item.documentFolder['id']);
                  },
                );
              },
            ),
          ),
          Positioned(
            bottom: 100,
            right: 20,
            child: Builder(
              builder: (context) {
                return FloatingActionButton(
                  child: Icon(Icons.add),
                  onPressed: () async {
                    var bloc = BlocProvider.of<ProjectInspectBloc>(context);
                    var text = await showTextFieldDialog(
                      context,
                      title: 'Navn på folder',
                      hint: 'Folders navn...',
                    );
                    if (text != null)
                      bloc.dispatch(ProjectInspectEventAddFolder(
                          title: text, projectId: widget.projectId));
                  },
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  InspectScreenItem _buildPostItem(List<ProjectItem> items) {
    return InspectScreenItem(
      icon: Icon(Icons.markunread_mailbox),
      child: Stack(
        children: <Widget>[
          Positioned.fill(
            child: ListView.builder(
              itemCount: items.length,
              itemBuilder: (context, index) {
                var item = items[index];
                return ListTile(
                  leading: Icon(Icons.mail),
                  title: Text('${item.post.title}'),
                );
              },
            ),
          ),
          Positioned(
            bottom: 100,
            right: 20,
            child: Builder(
              builder: (context) {
                return FloatingActionButton(
                  child: Icon(Icons.add),
                  onPressed: () async {
                    var bloc = BlocProvider.of<ProjectInspectBloc>(context);
                    var tuple = await postCreateDialog(context);
                    if (tuple != null)
                      bloc.dispatch(
                        ProjectInspectEventAddPost(
                          title: tuple.first,
                          body: tuple.second,
                          projectId: widget.projectId,
                        ),
                      );
                  },
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  void addToProject(BuildContext context, LoadedProjectInspectState state) {
    var bloc = BlocProvider.of<ProjectInspectBloc>(context);
    showModalBottomSheet(
        context: context,
        builder: (context) {
          return Container(
            height: 400,
            child: ListView(
              children: <Widget>[
                ListTile(
                  title: Text(
                    'Tilføj til projekt',
                    style: TextStyle(fontWeight: FontWeight.bold),
                  ),
                ),
                ListTile(
                  title: Text('Nyt projekt'),
                  leading: Icon(Icons.add),
                  onTap: () async {
                    var text = await showTextFieldDialog(
                      context,
                      title: 'Navn på projekt',
                      hint: 'Projektets navn...',
                    );
                    if (text != null)
                      bloc.dispatch(ProjectInspectEventAddProject(
                          name: text, projectId: state.project.id));
                  },
                ),
                if (!state.projectItems
                    .any((pi) => pi.projectItemType == ProjectItemType.address))
                  ListTile(
                    leading: Icon(Icons.place),
                    title: Text('Address'),
                    onTap: () => bloc.dispatch(ProjectInspectEventAddAddress(
                        projectId: state.project.id)),
                  ),
                if (!state.projectItems
                    .any((pi) => pi.projectItemType == ProjectItemType.logs))
                  ListTile(
                    leading: Icon(Icons.assignment),
                    title: Text('Logs'),
                    onTap: () => bloc.dispatch(ProjectInspectEventAddLogs(
                        projectId: state.project.id)),
                  ),
                if (!state.projectItems
                    .any((pi) => pi.projectItemType == ProjectItemType.work))
                  ListTile(
                    leading: Icon(Icons.work),
                    title: Text('Arbejde'),
                    onTap: () => bloc.dispatch(ProjectInspectEventAddWork(
                        projectId: state.project.id)),
                  ),
                if (!state.projectItems
                    .any((pi) => pi.projectItemType == ProjectItemType.tasks))
                  ListTile(
                    leading: Icon(Icons.extension),
                    title: Text('Opgaver'),
                    onTap: () => bloc.dispatch(ProjectInspectEventAddTasks(
                        projectId: state.project.id)),
                  ),
                if (!state.projectItems.any((pi) =>
                    pi.projectItemType == ProjectItemType.qualityReports))
                  ListTile(
                    title: Text('Kvalitetsrapporter'),
                    leading: Icon(Icons.assignment_late),
                    onTap: () => bloc.dispatch(
                        ProjectInspectEventAddQualityReports(
                            projectId: state.project.id)),
                  ),
                ListTile(
                  title: Text('Kommentar'),
                  leading: Icon(Icons.comment),
                  onTap: () => bloc.dispatch(ProjectInspectEventAddComment(
                      projectId: state.project.id)),
                ),
                if (!state.projectItems.any(
                    (pi) => pi.projectItemType == ProjectItemType.profileImage))
                  ListTile(
                    title: Text('Profilbillede'),
                    leading: Icon(Icons.image),
                    onTap: () => bloc.dispatch(
                        ProjectInspectEventAddProfileImage(
                            projectId: state.project.id)),
                  ),
              ],
            ),
          );
        });
  }
}
