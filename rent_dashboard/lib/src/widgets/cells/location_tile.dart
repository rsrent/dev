import 'package:flutter/material.dart';
import '../../models/location.dart';
import '../../blocs/location_provider.dart';
import '../../models/sortable_by.dart';

import '../cell_headers/serviceleader_header.dart';
import '../cell_headers/next_quality_report_header.dart';

import 'data_tile.dart';

class LocationTile extends StatelessWidget {
  final Location location;
  LocationTile(this.location);

  @override
  Widget build(BuildContext context) {
    final bloc = LocationProvider.of(context);

    return DataTile(
      getHeader: (s) {
        if (s == SortBy.ServiceLeader)
          return ServiceLeaderHeader(
            serviceLeader: location.serviceLeaderName,
          );
        if (s == SortBy.QualityReportStatus)
          return NextQualityReport(location.nextQualityReport, location.agreedNextQualityReport);
      },
      header: location.name,
      subHeader: location.customerName,
      simpleData: location.smallData,
      sortBy: bloc.beingSortedBy,
      onPressed: () {
        //bloc.inspected = customer.id;
        Navigator.pushNamed(
            context, 'data/0/0/${location.id}/${location.name}');
      }
    );
/*
    return Card(
      child: Column(
        mainAxisSize: MainAxisSize.max,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _buildTileHeader(locationsBloc.beingSortedBy),
          _buildTileBody(),
        ],
      ),
    ); */
  }
/*
  _buildTileHeader(SortBy sortedBy) {
    switch (sortedBy) {
      case SortBy.Name:
        return Container();
      case SortBy.CleaningTaskStatus:
        return WorkStatusHeader(location.smallData.tasks);
      case SortBy.QualityReportStatus:
        return NextQualityReport(location.nextQualityReport);
      case SortBy.DG:
        return DgHeader(
          dg: location.smallData.dg,
        );
      case SortBy.ServiceLeader:
        return ServiceLeaderHeader(
          serviceLeader: location.serviceLeaderName,
        );
      case SortBy.UnfinishedTasks:
        return UnfinishedTasks(
          morework: location.smallData.incompleteMorework,
          qualityreports: location.smallData.incompleteReports,
        );
      case SortBy.UnfinishedSetup:
        return UnfinishedSetup(
          serviceLeader: location.smallData.locationsWithoutServiceLeader,
          personale: location.smallData.locationsWithoutStaff,
          plan: location.smallData.locationsWithoutTasks,
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
            '${location.name}',
            style: TextStyle(fontSize: 20.0),
          ),
          Divider(
            color: Colors.transparent,
            height: 8.0,
          ),
          Text(
            '${location.customerName}',
            style: TextStyle(fontSize: 16.0, color: Colors.grey),
          ),
        ],
      ),
    );
  } */
}
