/*

import 'package:http/http.dart' as http;
import 'dart:convert';
import './network.dart';
import './models/data.dart';
import './models/customer.dart';
import './models/user.dart';
import './models/log.dart';
import './filter.dart';

class Api {
  var frontPageSetState;
  var customersListSetState;
  var serviceLeadersListSetState;
  var locationsListSetState;
  var tasksListSetState;
  var logsListSetState;

  CompleteData frontPageData;
  List<Customer> customers = List();
  List<ServiceLeader> serviceLeaders = List();
  List<Log> logs = List();

  setFrontPageSetState(up) => frontPageSetState = up;
  setCustomersListSetState(up) => customersListSetState = up;
  setServiceLeadersListSetState(up) => serviceLeadersListSetState = up;
  setLocationsListSetState(up) => locationsListSetState = up;
  setTasksListSetState(up) => tasksListSetState = up;
  setLogsListSetState(up) => logsListSetState = up;

  loadAll() async {
    await loadHolidays();
    loadFrontPage();
    loadCustomer();
    loadServiceLeaders();
    loadLogs();
  }

  loadHolidays() async {
    var response = await http.get(Network.api + '/api/Dashboard/Holidays',
        headers: Network.getHeaders());
    if (response.statusCode == 200) {
      var rest = json.decode(response.body);
      Filters.holidays = List.from(rest.map((d) {
        return DateTime.parse(d);
      }));
    }
  }

  loadFrontPage() {
    http
        .get(Network.api + '/api/Dashboard/Data/0/0', headers: Network.getHeaders())
        .then((result) {
      var rest = json.decode(result.body);
      frontPageData = CompleteData(rest);
      frontPageSetState();
      //update(frontPageSetState);
    });
  }

  loadCustomer() {
    http
        .get(Network.api + '/api/Dashboard/Customers',
            headers: Network.getHeaders())
        .then((result) {
      var rest = json.decode(result.body);

      var cs = List<Customer>.from(rest.map((j) {
        var t = Customer.fromJson(j);

        return t;
      }));

      customers = cs;
      customersListSetState();
      //update(customersListSetState);
    });
  }

  loadServiceLeaders() {
    http
        .get(Network.api + '/api/Dashboard/Users',
            headers: Network.getHeaders())
        .then((result) {
      var rest = json.decode(result.body);

      var cs = List<ServiceLeader>.from(rest.map((j) {
        var t = ServiceLeader(json: j);

        return t;
      }));
      serviceLeaders = cs;
      serviceLeadersListSetState();
    });
  }

  loadLogs() {
    var api = Network.api + '/api/Dashboard/Logs';

    http.get(api, headers: Network.getHeaders()).then((result) {
      var rest = json.decode(result.body);

      var ts = List<Log>.from(rest.map((j) {
        var t = Log(j);
        return t;
      }));
      logs = ts;
      logsListSetState();
    });
  }

  update(state) {
    state(() {});
  }
}


*/