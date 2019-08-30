import 'package:flutter/material.dart';
import '../../models/data.dart';
import '../../models/sortable_by.dart';

import '../cell_headers/workstatus_header.dart';
import '../cell_headers/unfinished_setup_header.dart';
import '../cell_headers/unfinished_tasks_header.dart';
import '../cell_headers/dg_header.dart';

typedef Widget GetHeader(SortBy sortby);

class DataTile extends StatelessWidget {
  final Function onPressed;
  final String header;
  final String subHeader;
  final SimpleData simpleData;
  final SortBy sortBy;
  final GetHeader getHeader;
  DataTile(
      {this.sortBy,
      this.simpleData,
      this.getHeader,
      this.header,
      this.onPressed,
      this.subHeader});

  @override
  Widget build(BuildContext context) {
    return FlatButton(
      child: Card(
        child: Column(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            _buildTileHeader(sortBy),
            Divider(
              color: Colors.transparent,
              height: 0.0,
            ),
            _buildTileBody(),
            Divider(
              color: Colors.transparent,
              height: 0.0,
            ),
          ],
        ),
      ),
      onPressed: onPressed,
    );
  }

  _buildTileHeader(SortBy sortedBy) {
    var specialHeader = getHeader != null ? getHeader(sortedBy) : null;

    if (specialHeader != null) return specialHeader;

    switch (sortedBy) {
      case SortBy.Name:
        return Container();
      case SortBy.CleaningTaskStatus:
        return WorkStatusHeader(simpleData.tasks);
      case SortBy.QualityReportStatus:
        return WorkStatusHeader(simpleData.reports);
      case SortBy.DG:
        return DgHeader(
          dg: simpleData.dg,
        );

      case SortBy.UnfinishedTasks:
        return UnfinishedTasks(
          morework: simpleData.incompleteMorework,
          qualityreports: simpleData.incompleteReports,
        );
      case SortBy.UnfinishedSetup:
        return UnfinishedSetup(
          serviceLeader: simpleData.locationsWithoutServiceLeader,
          personale: simpleData.locationsWithoutStaff,
          plan: simpleData.locationsWithoutTasks,
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
            '$header',
            style: TextStyle(fontSize: 18.0),
          ),
          Divider(
            color: Colors.transparent,
            height: 8.0,
          ),
          Text(
            '$subHeader',
            style: TextStyle(fontSize: 14.0, color: Colors.grey),
          ),
        ],
      ),
    );
  }
}
