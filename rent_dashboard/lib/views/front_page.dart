import '../models/data.dart';
import '../models/task.dart';
import '../models/report.dart';
import 'package:flutter/material.dart';

import 'package:http/http.dart' as http;
import 'dart:convert';
import '../network.dart';
import '../api.dart';
import '../filter.dart';
import '../generic/date_time.dart';
import './dataviews/recent_data.dart';
import './dataviews/full_over_view.dart';

class FrontPage extends StatefulWidget {
  final Api api;
  FrontPage({this.api, Key key}) : super(key: key);
  @override
  _FrontPageState createState() => _FrontPageState();
}

class _FrontPageState extends State<FrontPage> {
  Filters filter;

  _FrontPageState() {
    filter = Filters();
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    widget.api.setFrontPageSetState(() {
      if (mounted) {
        setState(() {});
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: widget.api.frontPageData != null
          ? ListView(
              padding:
                  const EdgeInsets.only(left: 20.0, right: 20.0, top: 20.0),
              children: <Widget>[
                Text(
                  'Rent',
                  style: TextStyle(fontSize: 30.0, color: Colors.blue),
                  textAlign: TextAlign.center,
                ),
                FullOverview(),
/*
                WorkStatusOverview(widget.api.frontPageData, filter),
                Divider(height: 20.0),
                RecentNumbers.fromData(widget.api.frontPageData),
                Divider(height: 20.0),
                LocationsOverview(widget.api.frontPageData, filter),*/
                Divider(height: 200.0, color: Colors.transparent),
              ],
            )
          : Text('loading'),
    );
  }
}
