import 'package:bms_dart/models.dart';
import 'package:bms_dart/project_list_bloc.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:bms_flutter_admin/src/widgets/document_of_folder_screen.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../widgets/home_menu.dart';
import 'package:flutter/material.dart';

import 'package:bms_flutter_admin/src/screens/user_inspect_screen.dart';

import 'package:bms_flutter/src/widgets/project/widgets.dart';
import 'package:bms_flutter/src/widgets/user/widgets.dart';
import 'package:bms_flutter/src/widgets/noti/widgets.dart';
import 'package:bms_dart/noti_list_bloc.dart';
import 'package:bms_dart/blocs.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'project_inspect_screen.dart';

class HomeScreen extends StatefulWidget {
  final int projectId = 4184;

  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen>
    with SingleTickerProviderStateMixin {
  TabController _tabController;
  QueryResultBloc _queryResultBloc = QueryResultBloc();

  @override
  void initState() {
    _tabController = TabController(length: 3, vsync: this);
    super.initState();
  }

  Future<bool> _onWillPopScope() async {
    return false;
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: _onWillPopScope,
      child: Scaffold(
        appBar: AppBar(
          leading: Builder(
            builder: (context) {
              return IconButton(
                icon: Icon(Icons.menu),
                onPressed: () {
                  Scaffold.of(context).openDrawer();
                },
              );
            },
          ),
          actions: <Widget>[
            IconButton(
              icon: Icon(Icons.notifications),
              onPressed: () {
                NotiListScreen.show(
                  context,
                  blocBuilder: (context) =>
                      NotiListBloc(() => NotiListFetchLatest(count: 10)),
                  onSelect: (noti) {
                    if (noti.notiType.startsWith('Absence_')) {
                      UserInspectScreen.show(context, noti.userId,
                          startIndex: 1);
                    }
                    if (noti.notiType.startsWith('AccidentReport_')) {
                      UserInspectScreen.show(context, noti.userId,
                          startIndex: 3);
                    }
                  },
                );
                // Navigator.of(context).push(
                //   MaterialPageRoute(
                //     builder: (context) {
                //       return NotiListAllScreenAdmin();
                //     },
                //   ),
                // );
              },
            )
          ],
        ),
        //2822
        // body: ProjectInspectScreen(
        //   projectId: 2822,
        // ),

        body: QueryResultScreen(
          blocs: [_queryResultBloc],
          child: BlocProvider(
            builder: (context) => ProjectInspectBloc(
              widget.projectId,
              queryResultBloc: _queryResultBloc,
              sprog: () => Translations.of(context),
            )..dispatch(ProjectInspectEventFetch()),
            child: Builder(
              builder: (context) {
                var bloc = BlocProvider.of<ProjectInspectBloc>(context);
                return AnimatedBlocBuilder(
                  bloc: bloc,
                  builder: (context, state) {
                    if (state is LoadedProjectInspectState &&
                        state.project != null) {
                      return _build(context, state);
                    } else if (state is ErrorProjectInspectState) {
                      return ErrorLoadingScreen(
                        info: Translations.of(context).infoErrorLoading,
                        onRetre: () =>
                            bloc.dispatch(ProjectInspectEventFetch()),
                      );
                    } else {
                      return LoadingScreen();
                    }
                  },
                );
              },
            ),
          ),
        ),

        // body: ProjectListScreen(
        //   blocBuilder: (context) => ProjectListBloc(
        //     () => ProjectListFetchOfProject(projectId: 2822),
        //   ),
        //   onSelect: (project) => ProjectInspectScreen.show(context, project.id),
        //   floatingActionButton: Builder(
        //     builder: (context) {
        //       return FloatingActionButton(
        //         heroTag: null,
        //         child: Icon(Icons.add),
        //         onPressed: () async {
        //           var bloc = BlocProvider.of<ProjectListBloc>(context);
        //           var text = await projectCreateDialog(context);
        //           if (text != null)
        //             bloc.dispatch(
        //                 ProjectListAddNew(name: text, projectId: 2822));
        //         },
        //       );
        //     },
        //   ),
        // ),
        /*
        body: ListView(
          children: <Widget>[
            ListTile(
              title: Text('Forsinkede vagter'),
            ),
            ListTile(
              title: Text('Anmodninger om fravær'),
            ),
            ListTile(
              title: Text('Fraværende brugere'),
            ),
            ListTile(
              title: Text('Vagter uden aftager'),
            ),
            ListTile(
              title: Text('Vagtplaner uden aftager'),
            ),
          ],
        ), */
        drawer: Drawer(
          child: HomeMenu(),
        ),
      ),
    );
  }

  Widget _build(BuildContext context, LoadedProjectInspectState state) {
    var documentItems = state.projectItems
        .where((pi) => pi.projectItemType == ProjectItemType.documentFolders)
        .toList();

    return Scaffold(
      body: TabBarView(
        controller: _tabController,
        children: [
          ProjectListScreen(
            blocBuilder: (context) => ProjectListBloc(
              () => ProjectListFetchOfProject(projectId: widget.projectId),
            ),
            onSelect: (project) =>
                ProjectInspectScreen.show(context, project.id),
            floatingActionButton: Builder(
              builder: (context) {
                return FloatingActionButton(
                  heroTag: null,
                  child: Icon(Icons.add),
                  onPressed: () async {
                    var bloc = BlocProvider.of<ProjectListBloc>(context);
                    var text = await showTextFieldDialog(
                      context,
                      title: 'Navn på projekt',
                      hint: 'Projektets navn...',
                    );
                    if (text != null)
                      bloc.dispatch(ProjectListAddNew(
                          name: text, projectId: widget.projectId));
                  },
                );
              },
            ),
          ),
          UserListScreen(
            blocBuilder: (context) => UserListBloc(
              () => FetchAll(),
            ),
            onSelect: (user) => UserInspectScreen.show(context, user.id),
            floatingActionButton: FloatingActionButton(
              child: Icon(Icons.add),
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) => UserCreateUpdateScreen(),
                  ),
                );
              },
            ),
          ),
          ListView.builder(
            itemCount: documentItems.length,
            itemBuilder: (context, index) {
              var item = documentItems[index];
              return ListTile(
                leading: Icon(Icons.folder),
                title: Text('${item.documentFolder['title']}'),
                //subtitle: Text(item.access),
                onTap: () {
                  documentOfFolderScreen(context, item.documentFolder['id']);
                },
              );
            },
          )
        ],
      ),
      bottomNavigationBar: TabBar(
        labelColor: Colors.black,
        controller: _tabController,
        tabs: <Widget>[
          Tab(
            text: 'Projects',
          ),
          Tab(
            text: 'Users',
          ),
          Tab(
            text: 'Documents',
          ),
        ],
      ),
    );
  }
}
