import 'package:flutter/material.dart';
import '../models/customer.dart';
import '../models/task.dart';
import '../models/data.dart';
import '../api.dart';

import './location_tasks_list.dart';
import '../filter.dart';

import 'package:http/http.dart' as http;
import 'dart:convert';
import '../network.dart';

class CustomerTasksList extends StatefulWidget {
  final Api api;
  CustomerTasksList({Key key, this.api}) : super(key: key);
  @override
  _CustomerTasksListState createState() => _CustomerTasksListState();
}

class _CustomerTasksListState extends State<CustomerTasksList> {
  List<Customer> customers = List();
  FilterController filterController;

  _CustomerTasksListState() {
    filterController = FilterController(setState: setState);
  }

  @override
  void initState() {
    super.initState();
    load();
    /*
    widget.api.setCustomersListSetState(() {
      if (mounted) {
        setState(() {
          customers = widget.api.customers;
        });
      }
    }); */
  }

  @override
  Widget build(BuildContext context) {
    //customers =
    //    sortedDataList(widget.api.customers, filterController.getFilter());

    return Scaffold(
      floatingActionButton: filterController.floatingButton(),
      body: filterController.bodyController(Container(
        child: ListView.builder(
          padding:
              EdgeInsets.only(bottom: 100.0, left: 8.0, right: 8.0, top: 8.0),
          itemCount: customers.length,
          itemBuilder: (BuildContext context, int index) {
            var customer = customers[index];
            return Card(
              child: FlatButton(
                padding: const EdgeInsets.all(8.0),
                child: CustomerTaskCell(customer, filterController.getFilter()),
                onPressed: () {
                  viewCustomerLocations(customer);
                },
              ),
            );
          },
        ),
      )),
    );
  }

  load() {
    http
        .get(Network.root + '/api/Dashboard/Customers',
            headers: Network.getHeaders())
        .then((result) {
      var rest = json.decode(result.body);

      var cs = List<Customer>.from(rest.map((j) {
        var t = Customer(j);

        return t;
      })); 

      

      setState(() {
              customers = cs;
            });
      //customersListSetState();
      //update(customersListSetState);
    });
  }

  viewCustomerLocations(Customer c) {
    Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => LocationTasksList(
                  customer: c,
                )));
  }
}
