import 'package:bms_dart/absence_list_bloc.dart';
import 'package:bms_dart/accident_report_list_bloc.dart';
import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/conversation_bloc.dart';
import 'package:bms_dart/conversation_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/noti_list_bloc.dart';
// import 'package:bms_dart/post_list_bloc.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/work_list_bloc.dart';
import 'package:bms_flutter/translations.dart';
// import 'package:bms_flutter/src/widgets/test.dart';

// import 'package:bms_flutter/src/screens/noti_list_all_screen.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:bms_flutter_user/screens/conversation_screen.dart';
import 'package:bms_flutter_user/widgets/home_menu.dart';
// import 'package:bms_flutter_user/widgets/user_work_list.dart';
import 'package:dart_packages/streamer.dart';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'conversation_list_screen.dart';
import 'package:bms_flutter/src/components/animated_stream_builder.dart';
import 'package:bms_flutter/src/widgets/noti/widgets.dart';
import 'package:bms_flutter/src/widgets/absence/widgets.dart';
import 'package:bms_flutter/src/widgets/accident_report/widgets.dart';
import 'package:bms_flutter/src/widgets/work/widgets.dart';

class HomeScreen extends StatefulWidget {
  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> with TickerProviderStateMixin {
  TabController _tabController;

  Streamer<int> currentTab = Streamer(seedValue: 0);
  QueryResultBloc queryResultBloc = QueryResultBloc();

  var authenticationRepository = repositoryProvider.authenticationRepository();

  @override
  void initState() {
    super.initState();
    _tabController = TabController(initialIndex: 0, length: 5, vsync: this);

    _tabController.addListener(() {
      currentTab.update(_tabController.index);
      setState(() {});
    });
  }

  Future<bool> _onWillPopScope() async {
    return false;
  }

  @override
  Widget build(BuildContext context) {
    return BlocProviderTree(
      blocProviders: [
        // BlocProvider<WorkListBloc>(
        //     builder: (context) => WorkListBloc(
        //           () => WorkListFetchOfSignedInUser(),
        //           queryResultBloc: queryResultBloc,
        //           sprog: () => Translations.of(context),
        //         )),
        BlocProvider<ConversationListBloc>(
          builder: (context) =>
              ConversationListBloc()..dispatch(ConversationListFetchAll()),
        ),
        // BlocProvider<AbsenceListBloc>(
        //   builder: (context) =>
        //       AbsenceListBloc(() => AbsenceListFetchOfSignedInUser()),
        // ),
        // BlocProvider<PostListBloc>(
        //   builder: (context) =>
        //       PostListBloc()..dispatch(PostListFetch(more: false)),
        // ),
        BlocProvider<AccidentReportListBloc>(
          builder: (context) => AccidentReportListBloc(
              () => AccidentReportListFetchOfSignedInUser()),
        ),
      ],
      child: WillPopScope(
        onWillPop: _onWillPopScope,
        child: QueryResultScreen(
          blocs: [queryResultBloc],
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
                    );
                    // Navigator.of(context).push(
                    //   MaterialPageRoute(
                    //     builder: (context) {
                    //       return NotiListAllScreen();
                    //     },
                    //   ),
                    // );
                  },
                )
              ],
            ),
            body: TabBarView(
              controller: _tabController,
              children: [
                WorkListScreen(
                  blocBuilder: (context) => WorkListBloc(
                    () => WorkListFetchOfSignedInUser(),
                    queryResultBloc: queryResultBloc,
                    sprog: () => Translations.of(context),
                  ),
                  showUser: false,
                ),

                // UserWorkList(
                //   showUser: false,
                //   onRefresh: (bloc) =>
                //       bloc.dispatch(WorkListFetchOfSignedInUser()),
                // ),
                ConversationList(
                  onSelect: (conversation) =>
                      onConversationSelect(context, conversation),
                ),
                AbsenceListScreen(
                  blocBuilder: (context) => AbsenceListBloc(
                    () => AbsenceListFetchOfSignedInUser(),
                    queryResultBloc: queryResultBloc,
                    sprog: () => Translations.of(context),
                  ),
                ),
                AccidentReportListScreen(
                  blocBuilder: (context) => AccidentReportListBloc(
                    () => AccidentReportListFetchOfSignedInUser(),
                    queryResultBloc: queryResultBloc,
                    sprog: () => Translations.of(context),
                  ),
                ),
                // BlocList<AbsenceListBloc>(
                //   child: AbsenceList(
                //     onSelect: (absence) {},
                //   ),
                //   onRefresh: (bloc) =>
                //       bloc.dispatch(AbsenceListFetchOfSignedInUser()),
                // ),
                // BlocList<PostListBloc>(
                //   child: PostList(),
                //   onRefresh: (bloc) =>
                //       bloc.dispatch(PostListFetch(more: false)),
                // ),
                BlocList<AccidentReportListBloc>(
                  child: AccidentReportList(),
                  onRefresh: (bloc) =>
                      bloc.dispatch(AccidentReportListFetchOfSignedInUser()),
                )
              ],
            ),
            bottomNavigationBar: BottomNavigationBar(
              fixedColor: Theme.of(context).primaryColor,
              unselectedItemColor: Colors.grey,
              currentIndex: _tabController.index,
              items: [
                BottomNavigationBarItem(
                  icon: Icon(Icons.work),
                  title: Text(Translations.of(context).buttonWork),
                ),
                BottomNavigationBarItem(
                  icon: Icon(Icons.message),
                  title: Text(Translations.of(context).buttonConversations),
                ),
                BottomNavigationBarItem(
                  icon: Icon(Icons.person_outline),
                  title: Text(Translations.of(context).buttonAbsense),
                ),
                BottomNavigationBarItem(
                  icon: Icon(Icons.local_post_office),
                  title: Text(Translations.of(context).buttonPosts),
                ),
                BottomNavigationBarItem(
                  icon: Icon(Icons.warning),
                  title: Text('APV'),
                ),
              ],
              onTap: (index) {
                _tabController.animateTo(index);
              },
            ),
            // floatingActionButton: _tabController.index == 2
            //     ? FloatingActionButton(
            //         child: Icon(Icons.add),
            //         onPressed: () {
            //           Navigator.of(context)
            //               .push(MaterialPageRoute(builder: (context) {
            //             return AbsenceCreateScreen(
            //                 user: authenticationRepository.getUser());
            //           }));
            //         },
            //       )
            //     : null,
            floatingActionButton: SizedBox(
              height: 60,
              width: 60,
              child: AnimatedStreamBuilder(
                stream: currentTab.stream,
                builder: (context, index) {
                  if (index == 2)
                    return FloatingActionButton(
                      heroTag: null,
                      child: Icon(Icons.add),
                      onPressed: () {
                        AbsenceCreateUpdateScreen.show(context,
                            userId: authenticationRepository.getUserId());
                      },
                    );
                  if (index == 4)
                    return FloatingActionButton(
                      heroTag: null,
                      child: Icon(Icons.add),
                      onPressed: () {
                        AccidentReportCreateUpdateScreen.show(context,
                            userId: authenticationRepository.getUserId());
                      },
                    );
                },
              ),
            ),
            drawer: Drawer(
              child: HomeMenu(),
            ),
          ),
        ),
      ),
    );
  }

  void onConversationSelect(BuildContext context, Conversation conversation) {
    Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => ConversationScreen(
        conversationId: conversation.id,
      ),
    ));
  }
}
