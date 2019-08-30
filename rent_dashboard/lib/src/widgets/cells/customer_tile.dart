import 'package:flutter/material.dart';
import '../../models/customer.dart';
import '../../blocs/customer_provider.dart';
import '../../models/sortable_by.dart';

import 'data_tile.dart';

class CustomerTile extends StatelessWidget {
  final Customer customer;
  CustomerTile(this.customer);
  @override
  Widget build(BuildContext context) {
    var bloc = CustomerProvider.of(context);

    return DataTile(
      getHeader: (s) {
        
      },
      header: customer.name,
      subHeader: customer.name,
      simpleData: customer.smallData,
      sortBy: bloc.beingSortedBy,
      onPressed: () {
        bloc.inspected = customer.id;
        Navigator.pushNamed(
            context, 'data/${customer.id}/0/0/${customer.name}');
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
        bloc.inspected = customer.id;
        //Navigator.pushNamed(context, 'locations/${customer.id}/0');
        Navigator.pushNamed(
            context, 'data/${customer.id}/0/0/${customer.name}');
      },
    ); */
  }
/*
  _buildTileHeader(SortBy sortedBy) {
    switch (sortedBy) {
      case SortBy.Name:
        return Container();
      case SortBy.CleaningTaskStatus:
        return WorkStatusHeader(customer.smallData.tasks);
      case SortBy.QualityReportStatus:
        return WorkStatusHeader(customer.smallData.reports);
      case SortBy.DG:
        return DgHeader(
          dg: customer.smallData.dg,
        );
      case SortBy.ServiceLeader:
        return Text('Not supported');
      case SortBy.UnfinishedTasks:
        return UnfinishedTasks(
          morework: customer.smallData.incompleteMorework,
          qualityreports: customer.smallData.incompleteReports,
        );
      case SortBy.UnfinishedSetup:
        return UnfinishedSetup(
          serviceLeader: customer.smallData.locationsWithoutServiceLeader,
          personale: customer.smallData.locationsWithoutStaff,
          plan: customer.smallData.locationsWithoutTasks,
        );
    }
    return Text('');
  }

  _buildTileBody() {
    return Expanded(
      child: Container(
        margin: EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              '${customer.name}',
              style: TextStyle(fontSize: 20.0),
            ),
            Divider(
              color: Colors.transparent,
              height: 8.0,
            ),
            Text(
              '${customer.name}',
              style: TextStyle(fontSize: 16.0, color: Colors.grey),
            ),
          ],
        ),
      ),
    );
  } */
}
