import 'package:flutter/material.dart';
import '../blocs/location_provider.dart';
import '../models/location.dart';
import '../widgets/cells/location_tile.dart';
import '../widgets/filter_widget.dart';
import '../widgets/searchable_appbar.dart';
import '../widgets/list_grid.dart';

class LocationsList extends StatelessWidget {
  final String title;
  final Function(LocationBloc) prepareBloc;
  LocationsList({Key key, this.title, this.prepareBloc}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final bloc = LocationProvider.of(context);
    if (bloc != null && prepareBloc != null) {
      prepareBloc(bloc);
    }
    bloc.fetch();

    return Scaffold(
      resizeToAvoidBottomPadding: false,
      appBar: searchableAppBar(
        bloc: bloc,
        title: '$title, Søg efter lokation..',
      ),
      body: FliterWidget(
        sortBy: LocationBloc.sortableBy,
        updateSortBy: (s) {
          bloc.sortBy(s);
        },
        currentSort: () => bloc.beingSortedBy,
        updateQualityReportStatusOptions: bloc.updateQualityReportStatusOptions,
        updateCleaningTaskStatusOptions: bloc.updateCleaningTaskStatusOptions,
        child: StreamBuilder(
          stream: bloc.locations,
          builder: (context, AsyncSnapshot<List<Location>> snapshot) {
            print(snapshot.hasData);
            if (!snapshot.hasData)
              return Center(child: CircularProgressIndicator());
            return ListGrid(
              hasData: snapshot.hasData,
              length: snapshot.hasData ? snapshot.data.length : 0,
              getTile: (index) {
                return LocationTile(snapshot.data[index]);
              },
            );
          },
        ),
      ),
    );
  }
}
