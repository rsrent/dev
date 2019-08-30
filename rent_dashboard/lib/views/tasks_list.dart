import '../dummy.dart';
import '../models/task.dart';
import '../models/location.dart';
import '../models/customer.dart';
import 'package:flutter/material.dart';

import '../models/task.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'dart:io';
import '../network.dart';
import '../filter.dart';


class TasksList extends StatefulWidget {
  Customer customer;
  Location location;
  TasksList({Key key, this.customer, this.location}) : super(key: key);
  @override
  _TasksListState createState() =>
      _TasksListState(customer: this.customer, location: this.location);
}

class _TasksListState extends State<TasksList> {
  Customer customer;
  Location location;
  List<Task> tasks = List();
  FilterController filterController;
  _TasksListState({this.customer, this.location}){
    filterController = FilterController(setState: setState, filterWorkStatus: true);
  }

  @override
  void initState() {
    super.initState();
    load();
  }

  @override
  Widget build(BuildContext context) {
    var ts = tasks.where((t) => t.included(filterController.getFilter())).toList();
    ts.sort((a, b) => a.nextTime == null ? 1 : b.nextTime == null ? -1 : a.nextTime.compareTo(b.nextTime));
    return Scaffold(
      appBar: location != null ? AppBar( title: Text(location.name)) : null,
      floatingActionButton: filterController.floatingButton(),
      body: filterController.bodyController(Container(
          child: ListView.builder(
            itemCount: ts.length,
            itemBuilder: (BuildContext context, int index) {
              var task = ts[index];
                return TaskCell(task, filterController.getFilter());
            },
          ),
        ),
      ),
    );
  }

  load() {
    if (!mounted) return;
    var api = Network.root + '/api/Dashboard/Tasks/0/0/0';

    if (location != null) {
      api += '/Location/${location.id}';
    } 
    
    http.get(api, 
      headers: Network.getHeaders())
      .then((result) {
      var rest = json.decode(result.body);

      var ts = List<Task>.from(rest.map((j) {
        var t = Task(json: j);
        return t;
      }));
      
      
    if (!mounted) return;
      setState(() {
        tasks = ts;
      });
    });
  }
}