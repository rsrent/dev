import './models/task.dart';
import 'package:flutter/material.dart';
import './main.dart';
import './enums.dart';
import './generic/date_time.dart';
import './models/location.dart';
import './models/data.dart';
//Filters filters = Filters();

class Filters {
  static List<DateTime> holidays = List();

  Map<WorkStatus, bool> statusSet;

  WorkStatus statusToSortBy = WorkStatus.Overdue;
  bool sortByTasks = true;

  Set<PlanType> planTypes;

  DateTime maxDate;

  Filters() {
    DateTime now = DateTime.now();
    DateTime endOfWeek = now.add(Duration(days: 7 - now.weekday));
    maxDate =
        DateTime(endOfWeek.year, endOfWeek.month, endOfWeek.day, 23, 59, 59);

    statusSet = Map()
      ..putIfAbsent(WorkStatus.Critical, () => true)
      ..putIfAbsent(WorkStatus.Okay, () => true)
      ..putIfAbsent(WorkStatus.Overdue, () => true)
      ..putIfAbsent(WorkStatus.Unstarted, () => true)
      ..putIfAbsent(WorkStatus.Ignored, () => false);

    planTypes = Set()
      ..add(PlanType.Vinduer)
      ..add(PlanType.FanCoil)
      ..add(PlanType.Periodic);
  }
}

class FilterView extends StatefulWidget {
  final Filters filter;
  final bool filterWorkStatus;
  FilterView(this.filter, [this.filterWorkStatus = false]);
  @override
  _FilterViewState createState() => _FilterViewState(this.filter);
}

class _FilterViewState extends State<FilterView> {
  Filters filter;
  _FilterViewState(this.filter);

  @override
  Widget build(BuildContext context) {
    //return Center(child: Text('Test'));
    return Scaffold(
      body: Container(
        child: ListView(
          padding: const EdgeInsets.only(
              left: 16.0, right: 16.0, top: 60.0, bottom: 60.0),
          //mainAxisAlignment: MainAxisAlignment.center,
          //crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            title('Planer'),
            plansWidget(),

            widget.filterWorkStatus
                ? Column(
                    children: <Widget>[
                      Divider(color: Colors.black),
                      title('Opgave status'),
                      statusesWidget(),
                      Divider(color: Colors.black),
                    ],
                  )
                : Divider(color: Colors.black),

            title('Sorter efter'),
            sortByTaskOrReports(),
            sortByStatus(),
            //timeWidget(),
            Divider(
              color: Colors.black,
              height: 30.0,
            ),
            Center(
              child: RaisedButton(
                child: Text(toDateString(filter.maxDate)),
                onPressed: () async {
                  var date = await showDatePicker(
                      context: context,
                      initialDate: filter.maxDate,
                      firstDate: DateTime(1970),
                      lastDate: DateTime.now().add(
                          Duration(days: 365))); //, locale: Locale('da','DK')
                  setState(() {
                    filter.maxDate = date;
                  });
                },
              ),
            ),
          ],
        ),
        color: Colors.lightBlue[50],
      ),
    );
  }

  Widget title(text) {
    return Padding(
      padding: const EdgeInsets.only(left: 8.0, bottom: 12.0),
      child: Text(
        text,
        style: TextStyle(fontSize: 20.0, color: Colors.blue[700]),
      ),
    );
  }

  Widget sortByTaskOrReports() {
    Widget radio(bool v, icon) {
      return Column(
        children: <Widget>[
          Icon(
            icon,
            size: 40.0,
            color: Colors.blue,
          ),
          Radio(
            value: v,
            onChanged: (v) {
              filter.sortByTasks = v;
              setState(() {});
            },
            groupValue: filter.sortByTasks,
          ),
        ],
      );
    }

    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
      children: <Widget>[
        radio(false, Icons.assignment),
        radio(true, Icons.filter_tilt_shift),
      ],
    );
  }

  Widget sortByStatus() {
    Widget statusRadio(status) {
      return Column(
        children: <Widget>[
          getStatusIcon(status),
          Radio(
            value: status,
            onChanged: (v) {
              filter.statusToSortBy = v;
              setState(() {});
            },
            groupValue: filter.statusToSortBy,
          ),
        ],
      );
    }

    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
      children: <Widget>[
        statusRadio(WorkStatus.Overdue),
        statusRadio(WorkStatus.Critical),
        statusRadio(WorkStatus.Okay),
        statusRadio(WorkStatus.Unstarted),
      ],
    );
  }

  Widget plansWidget() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        planCheckbox('Vinduer', PlanType.Vinduer),
        planCheckbox('FanCoil', PlanType.FanCoil),
        planCheckbox('Periodisk', PlanType.Periodic),
      ],
    );
  }

  Widget planCheckbox(String text, PlanType plan) {
    return Expanded(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          Text(text),
          Checkbox(
            value: filter.planTypes.contains(plan),
            onChanged: (isOn) {
              if (isOn)
                filter.planTypes.add(plan);
              else
                filter.planTypes.remove(plan);
              setState(() {});
            },
          )
        ],
      ),
    );
  }

  Widget timeWidget() {
    return Column(
      children: <Widget>[
        MonthPicker(
          firstDate: DateTime(1970),
          lastDate: DateTime(DateTime.now().year + 1),
          onChanged: (date) {
            setState(() {
              filter.maxDate = date;
            });
          },
          selectedDate: filter.maxDate,
        )
      ],
    );
  }

  Widget statusesWidget() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        statusCheckbox('Overdue', WorkStatus.Overdue),
        statusCheckbox('Critical', WorkStatus.Critical),
        statusCheckbox('Okay', WorkStatus.Okay),
        statusCheckbox('Unstarted', WorkStatus.Unstarted),
      ],
    );
  }

  Widget statusCheckbox(String text, WorkStatus status) {
    return Expanded(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          Text(text),
          Checkbox(
            value: filter.statusSet[status],
            onChanged: (isOn) {
              if (isOn)
                filter.statusSet[status] = true;
              else
                filter.statusSet[status] = false;
              setState(() {});
            },
          )
        ],
      ),
    );
  }
}

class FilterController {
  bool _show = false;
  Filters _filter;
  var setState;
  bool filterWorkStatus;
  FilterController({@required this.setState, this.filterWorkStatus}) {
    _filter = Filters();
    filterWorkStatus = filterWorkStatus ?? false;
  }

  Filters getFilter() => _filter;
  bool showFilterView() => _show;
  Widget floatingButton() {
    return FloatingActionButton(
      child: Text('Sorter'),
      onPressed: () {
        _show = !_show;
        setState(() {});
      },
    );
  }

  Widget bodyController(body) {
    if (_show) {
      return FilterView(_filter, filterWorkStatus = filterWorkStatus);
    } else
      return body;
  }

  List<Location> sortLocations(List<Location> lss) {
    if (_filter.sortByTasks) {
      lss.sort((a, b) => compareData(a.smallData, b.smallData));
    } else {
      lss.sort((a, b) => a.nextQualityReport == null
          ? 1
          : b.nextQualityReport == null
              ? -1
              : a.nextQualityReport.compareTo(b.nextQualityReport));
    }
    return lss;
  }

  compareData(SimpleData d1, SimpleData d2) {
    if (_filter.sortByTasks) {
      return d2.tasks
          .getAmount(_filter.statusToSortBy)
          .compareTo(d1.tasks.getAmount(_filter.statusToSortBy));
    } else {
      return d2.reports
          .getAmount(_filter.statusToSortBy)
          .compareTo(d1.reports.getAmount(_filter.statusToSortBy));
    }

    /*
    if (_filter.sortByTasks) {
      return d2.getTasksCount(filter).compareTo(d1.getTasksCount(filter));
    } else {
      if (d1._oneReport && d2._oneReport) {
        if (d1._reports[0].status(filter) == filter.statusToSortBy)
          return -1;
        else if (d2._reports[0].status(filter) == filter.statusToSortBy)
          return 1;
        else {
          if (d1._reports[0] == null || d1._reports[0].time == null) return 1;
          if (d2._reports[0] == null || d2._reports[0].time == null)
            return -1;
          else
            return 0;
        }
      } else
        return d2.getReportsCount(filter).compareTo(d1.getReportsCount(filter));
    }*/
  }
}
