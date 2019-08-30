import 'package:flutter/material.dart';
import '../network.dart';
import '../api.dart';
import '../models/log.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class LogsList extends StatefulWidget {
  //final Api api;
  //LogsList({Key key, this.api}) : super(key: key);
  List<Log> logs = List();
  int locationId = 0;
  int customerId = 0;
  int userId = 0;
  LogsList({Key key, this.locationId, this.customerId, this.userId}) : super(key: key) 
  {
    locationId = locationId ?? 0;
    customerId = customerId ?? 0;
    userId = userId ?? 0;
  }

  @override
  _LogsListState createState() => _LogsListState();
}

class _LogsListState extends State<LogsList> {

  
  @override
  void initState() {
    super.initState();
    /*
    widget.api.setLogsListSetState(() {
      if (mounted) {
        setState(() {});
      }
    });*/
    load();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(),
      body: ListView.builder(
        padding: EdgeInsets.only(bottom: 100.0, left: 8.0, right: 8.0, top: 8.0),
          itemCount: widget.logs.length,
          itemBuilder: (BuildContext context, int index) {
            var task = widget.logs[index];
            return Card(child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: LogCell(task),
            ));
          }),
    );
  }

  load() {
    var api = Network.root + '/api/Dashboard/Logs/${widget.locationId}/${widget.customerId}/${widget.userId}';
    print(api);
    http.get(api, headers: Network.getHeaders()).then((result) {
      var rest = json.decode(result.body);

      var ts = List<Log>.from(rest.map((j) {
        var t = Log(j);
        return t;
      }));
      print(ts.length);
      setState(() {
        widget.logs = ts;
      });
    });
  }
  
}
