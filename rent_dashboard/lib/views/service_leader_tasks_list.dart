import 'package:flutter/material.dart';

import '../models/user.dart';
import '../models/data.dart';
import '../models/task.dart';
import '../api.dart';
import './location_tasks_list.dart';

import '../filter.dart';

class ServiceLeaderTasksList extends StatefulWidget {
  final Api api;
  ServiceLeaderTasksList({Key key, this.api}) : super(key: key);
  @override
  _ServiceLeaderTasksListState createState() => _ServiceLeaderTasksListState();
}

class _ServiceLeaderTasksListState extends State<ServiceLeaderTasksList> {
  List<ServiceLeader> serviceLeaders = List();
  FilterController filterController;

  _ServiceLeaderTasksListState() {
    filterController = FilterController(setState: setState);
    
  }

  @override
  void initState() {
    super.initState();
    widget.api.setServiceLeadersListSetState(() {
      if (mounted) {
        setState(() {
          serviceLeaders = widget.api.serviceLeaders;
        });
      }
    });
    serviceLeaders = widget.api.serviceLeaders;
  }

  @override
  Widget build(BuildContext context) {
    //serviceLeaders =
    //    sortedDataList(widget.api.serviceLeaders, filterController.getFilter());
    return Scaffold(
      floatingActionButton: filterController.floatingButton(),
      body: filterController.bodyController(
        Container(
          child: ListView.builder(
            padding:
                EdgeInsets.only(bottom: 100.0, left: 8.0, right: 8.0, top: 8.0),
            itemCount: serviceLeaders.length,
            itemBuilder: (BuildContext context, int index) {
              var serviceLeader = serviceLeaders[index];
              return Card(
                child: FlatButton(
                  padding: const EdgeInsets.all(8.0),
                  child: ServiceLeaderTasksCell(
                      serviceLeader, filterController.getFilter()),
                  onPressed: () {
                    viewCustomerLocations(serviceLeader);
                  },
                ),
              );
            },
          ),
        ),
      ),
    );
  }

  viewCustomerLocations(ServiceLeader sl) {
    Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => LocationTasksList(
                  serviceLeader: sl,
                )));
  }
}
