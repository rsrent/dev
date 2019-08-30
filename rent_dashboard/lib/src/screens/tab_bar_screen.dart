import 'package:flutter/material.dart';
import 'customers_list.dart';
import 'users_list.dart';
import 'locations_list.dart';
import 'tasks_list.dart';
import 'logs_list.dart';
import 'overview_grid.dart';

class TabBarScreen extends StatefulWidget {
  final String title;
  bool includeCustomers;
  bool includeUsers;
  bool includeLocations;

  TabBarScreen(
      {this.includeCustomers: true,
      this.includeUsers: true,
      this.includeLocations: true,
      this.title});

  @override
  _TabBarScreenState createState() => _TabBarScreenState();
}

class _TabBarScreenState extends State<TabBarScreen> {
  final List<PageStorageKey> keys = [
    PageStorageKey('frontPage'),
    PageStorageKey('customersPage'),
    PageStorageKey('serviceLeadersPage'),
    PageStorageKey('locationsPage'),
    PageStorageKey('tasksPage'),
    PageStorageKey('logsPage')
  ];

  int currentTab = 0;

  List<Widget> pages;
  List<BottomNavigationBarItem> items() {
    var _items = [barItem('home', Icons.dashboard)];

    if (widget.includeCustomers) {
      _items.add(barItem('kunder', Icons.business));
    }
    if (widget.includeUsers) {
      _items.add(barItem('sl', Icons.account_circle));
    }
    if (widget.includeLocations) {
      _items.add(barItem('lokaliteter', Icons.location_on));
    }

    _items.add(barItem('opgaver', Icons.filter_tilt_shift));
    _items.add(barItem('logs', Icons.comment));
    return _items;
  }

  Widget currentPage;

  //List<Data> dataList;
  final PageStorageBucket bucket = PageStorageBucket();

  //TabController tabController;
  bool sorting = false;

  @override
  void initState() {
    super.initState();
    pages = [
      OverviewGrid(
        key: keys[0],
        title: widget.title,
      )
    ];

    if (widget.includeCustomers) {
      pages.add(CustomersList(
        key: keys[1],
        title: widget.title,
      ));
    }
    if (widget.includeUsers) {
      pages.add(UsersList(
        key: keys[2],
        title: widget.title,
      ));
    }
    if (widget.includeLocations) {
      pages.add(LocationsList(
        key: keys[3],
        title: widget.title,
      ));
    }

    pages.add(TasksList(
      key: keys[4],
      title: widget.title,
    ));
    pages.add(LogsList(
      key: keys[5],
      title: widget.title,
    ));
    currentPage = pages[0];
  }

  @override
  Widget build(BuildContext context) {
    return decideFormat();
  }

  decideFormat() {
    return iphoneFormat();
    return MediaQuery.of(context).size.width > 800.0
        ? ipadFormat()
        : iphoneFormat();
  }

  iphoneFormat() {
    return Scaffold(
      body: PageStorage(
        child: currentPage,
        bucket: bucket,
      ),
      /*
      floatingActionButton: FloatingActionButton(
        child: Text('Sorter'),
        onPressed: () {
          setState(() {
            sorting = !sorting;
          });
        },
      ), */
      bottomNavigationBar: BottomNavigationBar(
        fixedColor: Theme.of(context).primaryColor,
        currentIndex: currentTab,
        onTap: (int index) {
          setState(() {
            currentTab = index;
            currentPage = pages[index];
          });
        },
        items: items(),
      ),
    );
  }

  ipadFormat() {
    return Scaffold(
      body: ListView(
        scrollDirection: Axis.horizontal,
        children: <Widget>[]..addAll(pages.map((p) => SizedBox(
              width: 400.0,
              child: p,
            ))),
      ),
    );
  }

  BottomNavigationBarItem barItem(text, icon) {
    var themeData = Theme.of(context);

    print(themeData);

    return BottomNavigationBarItem(
      icon: Icon(
        icon,
        color: Theme.of(context).primaryColor,
      ),
      title: Text(
        text,
        style: TextStyle(color: Theme.of(context).primaryColor),
      ),
    );
  }

  doneEditing() {
    setState(() {
      sorting = false;
    });
  }

  @override
  void dispose() {
    super.dispose();
    //tabController.dispose();
  }
}
