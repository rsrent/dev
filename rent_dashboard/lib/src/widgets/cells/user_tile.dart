import 'package:flutter/material.dart';
import '../../models/user.dart';
import '../../blocs/user_provider.dart';

import 'data_tile.dart';

class UserTile extends StatelessWidget {
  final ServiceLeader serviceLeader;
  UserTile(this.serviceLeader);

  @override
  Widget build(BuildContext context) {
    var bloc = UserProvider.of(context);

    return DataTile(
      getHeader: (s) {
        
      },
      header: serviceLeader.name,
      subHeader: serviceLeader.name,
      simpleData: serviceLeader.smallData,
      sortBy: bloc.beingSortedBy,
      onPressed: () {
        bloc.inspected = serviceLeader.id;
        Navigator.pushNamed(
            context, 'data/0/${serviceLeader.id}/0/${serviceLeader.name}');
      }
    );

/*
    return FlatButton(
      padding: EdgeInsets.all(0.0),
      child: Card(
        child: Column(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            _buildTileHeader(bloc.beingSortedBy),
            _buildTileBody(),
          ],
        ),
      ),
      onPressed: () {
        bloc.inspected = serviceLeader.id;
        //Navigator.pushNamed(context, 'locations/0/${serviceLeader.id}');
        Navigator.pushNamed(
            context, 'data/0/${serviceLeader.id}/0/${serviceLeader.name}');
      },
    ); */
  }
/*
  _buildTileHeader(SortBy sortedBy) {
    switch (sortedBy) {
      case SortBy.Name:
        return Container();
      case SortBy.CleaningTaskStatus:
        return WorkStatusHeader(serviceLeader.smallData.tasks);
      case SortBy.QualityReportStatus:
        return WorkStatusHeader(serviceLeader.smallData.reports);
      case SortBy.DG:
        return DgHeader(
          dg: serviceLeader.smallData.dg,
        );
      case SortBy.ServiceLeader:
        return Text('Not supported');
      case SortBy.UnfinishedTasks:
        return UnfinishedTasks(
          morework: serviceLeader.smallData.incompleteMorework,
          qualityreports: serviceLeader.smallData.incompleteReports,
        );
      case SortBy.UnfinishedSetup:
        return UnfinishedSetup(
          serviceLeader: serviceLeader.smallData.locationsWithoutServiceLeader,
          personale: serviceLeader.smallData.locationsWithoutStaff,
          plan: serviceLeader.smallData.locationsWithoutTasks,
        );
    }
    return Text('');
  }

  _buildTileBody() {
    return Container(
      margin: EdgeInsets.all(16.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          Text(
            '${serviceLeader.name}',
            style: TextStyle(fontSize: 20.0),
          ),
          Divider(
            color: Colors.transparent,
            height: 8.0,
          ),
          Text(
            '${serviceLeader.name}',
            style: TextStyle(fontSize: 16.0, color: Colors.grey),
          ),
        ],
      ),
    );
  } */
}
