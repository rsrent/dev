import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import './tasks_list.dart';
import '../models/user.dart';
import '../models/location.dart';
import '../models/customer.dart';
import '../models/data.dart';
import '../dummy.dart';
import '../models/task.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import '../network.dart';
import '../filter.dart';
import 'log_list.dart';

import './dataviews/full_over_view.dart';

class LocationTasksList extends StatefulWidget {
  ServiceLeader serviceLeader;
  Customer customer;
  LocationTasksList({Key key, this.customer, this.serviceLeader})
      : super(key: key);
  @override
  _LocationTasksListState createState() =>
      _LocationTasksListState(customer: customer, serviceLeader: serviceLeader);
}

class _LocationTasksListState extends State<LocationTasksList> {
  bool sortByTasks = true;
  ServiceLeader serviceLeader;
  Customer customer;
  var loadedLocations = List();
  var filteredLocations = List();
  FilterController filterController;

  _LocationTasksListState({this.customer, this.serviceLeader}) {
    filterController = FilterController(setState: setState);
  }

  @override
  void initState() {
    super.initState();
    load();
  }

  @override
  Widget build(BuildContext context) {
    //locations = sortedDataList(locations, filterController.getFilter());
    filteredLocations = filterController.sortLocations(loadedLocations);
    print('updated');
    return Scaffold(
      appBar: customer != null
          ? AppBar(title: Text(customer.name))
          : serviceLeader != null
              ? AppBar(title: Text(serviceLeader.name))
              : null,
      floatingActionButton: filterController.floatingButton(),
      body: filterController.bodyController(Container(
        child: ListView.builder(
          padding:
              EdgeInsets.only(bottom: 100.0, left: 8.0, right: 8.0, top: 8.0),
          itemCount: filteredLocations.length +
              (customer != null || serviceLeader != null ? 1 : 0),
          itemBuilder: (BuildContext context, int index) {
            Location loc;

            if ((customer != null || serviceLeader != null)) {
              if (index == 0) {
                return Card(
                    child: Column(
                  children: <Widget>[
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: <Widget>[
                        RaisedButton(
                          child: Text('Logs'),
                          onPressed: () {
                            Navigator.push(
                                context,
                                MaterialPageRoute(
                                    builder: (context) => LogsList(
                                          customerId: customer?.id,
                                          userId: serviceLeader?.id,
                                        )));
                          },
                        ),
                        RaisedButton(
                          child: Text('Tasks'),
                          onPressed: () {},
                        )
                      ],
                    ),
                    FullOverview(customer: customer, user: serviceLeader),
                  ],
                ));
              } else {
                loc = filteredLocations[index - 1];
              }
            } else {
              loc = filteredLocations[index];
            }

            return Card(
              child: FlatButton(
                padding: const EdgeInsets.all(8.0),
                child: LocationTaskCell(loc, filterController.getFilter()),
                onPressed: () {
                  viewLocationTasks(loc);
                },
              ),
            );
          },
        ),
      )),
    );
  }

  load() async {
    if (!mounted) return;
    var api = Network.root +
        '/api/Dashboard/Locations/${customer != null ?customer.id : 0}/${serviceLeader != null ? serviceLeader.id : 0}';
/*
    if (customer != null) {
      api += '/Customer/${customer.id}';
    } else if (serviceLeader != null) {
      api += '/ServiceLeader/${serviceLeader.id}';
    }*/

    http.get(api, headers: Network.getHeaders()).then((result) {
      var rest = json.decode(result.body);

      var cs = List<Location>.from(rest.map((j) {
        //print(j);
        var t = Location(j);

        return t;
      }));

      if (!mounted) return;
      setState(() {
        loadedLocations = cs;
      });
    });
  }

  viewLocationTasks(Location l) {
    Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => TasksList(
                  location: l,
                  customer: customer,
                )));
  }
}
