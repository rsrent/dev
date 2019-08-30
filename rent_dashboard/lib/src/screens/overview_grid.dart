import 'package:flutter/material.dart';
import '../blocs/data_provider.dart';
import '../widgets/data_components/tasks_status_chart.dart';
import '../widgets/data_components/qualityreport_status_chart.dart';
import '../widgets/data_components/work_history_chart.dart';
import '../widgets/data_components/dg_widget.dart';
//import '../widgets/data_components/map_widget.dart';
import '../blocs/work_history_provider.dart';
import '../blocs/log_provider.dart';
import '../blocs/dg_provider.dart';

import '../widgets/cells/log_tile.dart';
import '../widgets/list_grid.dart';
import 'package:dart_packages/date_time_operations.dart' as dateTimeOps;

class OverviewGrid extends StatelessWidget {
  String title;
  OverviewGrid({Key key, this.title}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final dataBloc = DataProvider.of(context);
    dataBloc?.fetchData();
    dataBloc?.fetchNews();

    final logsBloc = LogProvider.of(context);
    logsBloc.fetch();

    return Scaffold(
      appBar: AppBar(
        title: Text('$title, Overblik'),
      ),
      body: Container(
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [Colors.teal[100], Colors.grey[200]],
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
          ),
        ),
        child: GridView.count(
          padding: EdgeInsets.all(8.0),
          crossAxisCount: MediaQuery.of(context).size.width > 1200.0
              ? 3
              : MediaQuery.of(context).size.width > 800.0 ? 2 : 1,
          crossAxisSpacing: 16.0,
          mainAxisSpacing: 16.0,
          children: [
            _buildNewsFeed(dataBloc),
            _buildLogs(logsBloc),
            _buildDgWidget(dataBloc),
            _buildWorkStatusChart(dataBloc),
            _buildQualityReportChart(dataBloc),
            _buildWorkHistoryChart(context),
            _buildMap(dataBloc),
          ],
        ),
      ),
    );
  }

  Widget _buildWorkStatusChart(DataBloc dataBloc) {
    return StreamBuilder(
      stream: dataBloc.data,
      builder: (context, AsyncSnapshot dataSnapshot) {
        if (!dataSnapshot.hasData)
          return Center(child: CircularProgressIndicator());

        return HeroCardInspect(
          child: TasksStatusChart('', [
            dataSnapshot.data.windowTasks,
            dataSnapshot.data.fanCoilTasks,
            dataSnapshot.data.periodicTasks
          ]),
          title: 'Opgave status',
        );
      },
    );
  }

  Widget _buildQualityReportChart(DataBloc dataBloc) {
    return StreamBuilder(
      stream: dataBloc.data,
      builder: (context, AsyncSnapshot dataSnapshot) {
        if (!dataSnapshot.hasData)
          return Center(child: CircularProgressIndicator());

        return HeroCardInspect(
          child: QualityReportStatusChart(
              'Kvalitetsrapporter', dataSnapshot.data.qualityReportStatus),
          title: 'Kvalitetsreport status',
        );
      },
    );
  }

  Widget _buildWorkHistoryChart(BuildContext context) {
    var bloc = WorkHistoryProvider.of(context);
    bloc.fetch();

    return StreamBuilder(
      stream: bloc.workHistory,
      builder: (context, AsyncSnapshot<Map<DateTime, List<int>>> dataSnapshot) {
        if (!dataSnapshot.hasData)
          return Center(child: CircularProgressIndicator());
        return HeroCardInspect(
          child: WorkHistoryChart(dataSnapshot.data),
          title: 'Historik',
        );
      },
    );
  }

  Widget _buildDgWidget(DataBloc dataBloc) {
    //var bloc = DgProvider.of(context);
    //bloc.fetch();

    return StreamBuilder(
      stream: dataBloc.data,
      builder: (context, AsyncSnapshot dataSnapshot) {
        if (!dataSnapshot.hasData)
          return Center(child: CircularProgressIndicator());

        return HeroCardInspect(
          child: DgWidget(dataSnapshot.data),
          title: 'Nøgletal',
        );
      },
    );
  }

  Widget _buildMap(DataBloc dataBloc) {
    return StreamBuilder(
      stream: dataBloc.data,
      builder: (context, AsyncSnapshot dataSnapshot) {
        return Center(child: CircularProgressIndicator());
        //if (!dataSnapshot.hasData)
        //return Card(child: MapWidget(dataSnapshot.data));
      },
    );
  }

  Widget _buildNewsFeed(DataBloc dataBloc) {
    return StreamBuilder(
      stream: dataBloc.news,
      builder: (context, AsyncSnapshot snapshot) {
        if (!snapshot.hasData)
          return Center(child: CircularProgressIndicator());
        return HeroCardInspect(
          child: ListView.builder(
            physics: ScrollPhysics(),
            itemCount: snapshot.data.length,
            itemBuilder: (context, index) {
              var news = snapshot.data[index];
              return Card(
                child: ListTile(
                  title: Text(news.title),
                  subtitle: Text(
                      '${news.body}\n${dateTimeOps.toDDMM(news.time)} ${dateTimeOps.toHHmm(news.time)}'),
                ),
              );
            },
          ),
          title: 'News',
        );
      },
    );
  }

  Widget _buildLogs(LogBloc bloc) {
    return StreamBuilder(
      stream: bloc.logs,
      builder: (context, AsyncSnapshot snapshot) {
        if (!snapshot.hasData)
          return Center(child: CircularProgressIndicator());
        return HeroCardInspect(
          child: ListGrid(
            hasData: snapshot.hasData,
            length: snapshot.hasData ? snapshot.data.length : 0,
            getTile: (index) {
              return LogTile(snapshot.data[index]);
            },
            ratio: 1.5,
          ),
          title: 'Logs',
        );
      },
    );
  }
}

class HeroCardInspect extends StatelessWidget {
  final Widget child;
  final String title;
  HeroCardInspect({this.child, this.title});

  @override
  Widget build(BuildContext context) {
    return Container(
      child: FlatButton(
        padding: EdgeInsets.all(0.0),
        onPressed: () {
          Navigator.of(context)
              .push(MaterialPageRoute<Null>(builder: (BuildContext context) {
            return Scaffold(
              appBar: AppBar(
                title: Text(title),
              ),
              body: Hero(
                tag: title,
                child: Material(
                  child: Container(
                    color: Colors.white,
                    padding: const EdgeInsets.all(8.0),
                    margin: const EdgeInsets.only(bottom: 20.0),
                    child: child,
                  ),
                ),
              ),
            );
          }));
        },
        child: IgnorePointer(
          child: Card(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                Container(
                  padding: const EdgeInsets.only(top: 8.0, bottom: 8.0),
                  child: Text(
                    title,
                    style: TextStyle(fontSize: 20.0, color: Colors.white),
                    textAlign: TextAlign.center,
                  ),
                  decoration: BoxDecoration(
                    gradient: LinearGradient(
                        colors: [Theme.of(context).primaryColor, Colors.grey]),
                  ),
                ),
                Expanded(
                  child: Hero(
                    tag: title,
                    child: Material(
                      child: Container(
                        color: Colors.white,
                        padding: const EdgeInsets.all(8.0),
                        child: child,
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
