import 'package:flutter/material.dart';
import 'recent_data.dart';
import '../blocs/data_provider.dart';

import './complete_work_data.dart';
import './recent_data.dart';
import './static_data.dart';

import 'data_components/qualityreport_status_chart.dart';
import 'data_components/tasks_status_chart.dart';

class CompleteDataOverview extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final dataBloc = DataProvider.of(context);
    dataBloc?.fetchData();
    return StreamBuilder(
      stream: dataBloc.data,
      builder: (context, AsyncSnapshot dataSnapshot) {
        if (dataSnapshot.hasData)
          return Padding(
            padding: const EdgeInsets.all(20.0),
            child: Column(
              children: <Widget>[
                CompleteWorkView(dataSnapshot.data),
                Divider(height: 20.0),
                RecentData.fromData(dataSnapshot.data),
                Divider(height: 20.0),
                StaticData(dataSnapshot.data),
                Card(
                  child: Container(
                    height: 400.0,
                    padding: const EdgeInsets.all(8.0),
                    child: QualityReportStatusChart('Kvalitetsrapporter',
                        dataSnapshot.data.workStatusSet),
                  ),
                ),



                InkWell(
                  onTap: () {
                    Navigator.of(context).push(MaterialPageRoute<Null>(
                        builder: (BuildContext context) {
                      return Scaffold(
                        appBar: AppBar(
                          title: const Text('Flippers Page'),
                        ),
                        body: Hero(
                          tag: 'extremeBondage.com',
                          child: Container(
                            color: Colors.lightBlue,
                            padding: const EdgeInsets.all(8.0),
                            child: TasksStatusChart('', [
                              dataSnapshot.data.windowTasks,
                              dataSnapshot.data.fanCoilTasks,
                              dataSnapshot.data.periodicTasks
                            ]),
                          ),
                        ),
                      );
                    }));
                  },
                  child: Card(
                    child: Hero(
                      tag: 'extremeBondage.com',
                      child: Container(
                        color: Colors.lightBlue,
                        height: 400.0,
                        padding: const EdgeInsets.all(8.0),
                        child: TasksStatusChart('', [
                          dataSnapshot.data.windowTasks,
                          dataSnapshot.data.fanCoilTasks,
                          dataSnapshot.data.periodicTasks
                        ]),
                      ),
                    ),
                  ),
                ),
              ],
            ),
          );
        else
          return Container(
            height: 500.0,
            child: Center(child: CircularProgressIndicator()),
          );
      },
    );
  }
}
