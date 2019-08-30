import 'package:flutter/material.dart';
//import 'package:map_view/map_view.dart';
/*
import './views/front_page.dart';
import './views/log_list.dart';
import './views/location_tasks_list.dart';
import './views/customer_tasks_list.dart';
import './views/tasks_list.dart';
import './views/service_leader_tasks_list.dart';
import './views/login.dart';

import './network.dart';
import './filter.dart';
import 'api.dart';*/

import 'src/app.dart';

void main() {
  //MapView.setApiKey("AIzaSyDnsQZayjsOg0vs_uR383PicymfGyjygqI");
  runApp(new App());
}
/*
class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return new MaterialApp(
      title: 'Flutter Demo',
      theme: new ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: new MainView(),
    );
  }
}

class MainView extends StatefulWidget {
  @override
  MainViewState createState() => MainViewState();
}

class MainViewState extends State<MainView> {
  final List<PageStorageKey> keys = [
    PageStorageKey('frontPage'),
    PageStorageKey('customersPage'),
    PageStorageKey('serviceLeadersPage'),
    PageStorageKey('locationsPage'),
    PageStorageKey('tasksPage'),
    PageStorageKey('logsPage')
  ];

  int currentTab = 0;

  Api api;
  Filters filter;
  List<Widget> pages;
  Widget currentPage;

  //List<Data> dataList;
  final PageStorageBucket bucket = PageStorageBucket();

  //TabController tabController;
  bool sorting = false;

  @override
  void initState() {
    super.initState();
    api = Api();
    filter = Filters();
    pages = [
      FrontPage(key: keys[0], api: api),
      CustomerTasksList(key: keys[1], api: api),
      ServiceLeaderTasksList(key: keys[2], api: api),
      LocationTasksList(key: keys[3]),
      TasksList(key: keys[4]),
      LogsList(key: keys[5])
    ];
    currentPage = pages[0];
    //tabController = new TabController(length: 6, vsync: this);
  }

  @override
  Widget build(BuildContext context) {
    return Network.token == null
        ? LoginView(loggedIn)
        : sorting ? FilterView(filter) : decideFormat();
  }

  decideFormat() {
    return MediaQuery.of(context).size.width > 800.0
        ? ipadFormat()
        : iphoneFormat();
  }

  iphoneFormat() {
    return Scaffold(
      appBar: AppBar(
        title: Text('Rent'),
        actions: <Widget>[
          FlatButton(
            child: Icon(Icons.cached),
            onPressed: () {
              api.loadAll();
            },
          )
        ],
      ),
      body: PageStorage(
        child: currentPage,
        bucket: bucket,
      ),
      
      bottomNavigationBar: BottomNavigationBar(
        fixedColor: Colors.blue,
        currentIndex: currentTab,
        onTap: (int index) {
          setState(() {
            currentTab = index;
            currentPage = pages[index];
          });
        },
        items: <BottomNavigationBarItem>[
          barItem('home', Icons.dashboard),
          barItem('kunder', Icons.business),
          barItem('sl', Icons.account_circle),
          barItem('lokaliteter', Icons.location_on),
          barItem('opgaver', Icons.filter_tilt_shift),
          barItem('logs', Icons.comment),
        ],
      ),
    );
  }

  ipadFormat() {
    return Scaffold(
      body: ListView(
        scrollDirection: Axis.horizontal,
        children: <Widget>[]..addAll(pages.map((p) => SizedBox(width: 400.0, child: p,) )),
      ),
    );
  }

  BottomNavigationBarItem barItem(text, icon) {
    return BottomNavigationBarItem(
      icon: Icon(
        icon,
        color: Colors.blue,
      ),
      title: Text(
        text,
        style: TextStyle(color: Colors.blue),
      ),
    );
  }

  doneEditing() {
    setState(() {
      sorting = false;
    });
  }

  loggedIn(token) {
    Network.token = token;
    setState(() {});
    api.loadAll();
  }

  @override
  void dispose() {
    super.dispose();
    //tabController.dispose();
  }
}
 */
