import 'package:flutter/material.dart';
import '../blocs/customer_provider.dart';
import '../models/customer.dart';
import '../widgets/cells/customer_tile.dart';
import '../widgets/filter_widget.dart';
import '../widgets/searchable_appbar.dart';
import '../widgets/list_grid.dart';

class CustomersList extends StatelessWidget {
  final String title;
  CustomersList({Key key, this.title}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final bloc = CustomerProvider.of(context);
    bloc.fetchCustomers();

    return Scaffold(
      resizeToAvoidBottomPadding: false,
      appBar: searchableAppBar(
        bloc: bloc,
        title: '$title, Søg efter kunde..',
      ),
      body: FliterWidget(
        sortBy: CustomerBloc.sortableBy,
        updateSortBy: (s) {
          bloc.sortBy(s);
        },
        updateFilterBy: bloc.filterBy,
        currentSort: () => bloc.beingSortedBy,
        updateQualityReportStatusOptions: bloc.updateQualityReportStatusOptions,
        updateCleaningTaskStatusOptions: bloc.updateCleaningTaskStatusOptions,
        //updateCleaningTaskPlanOptions: bloc.updateCleaningTaskPlanOptions,
        child: StreamBuilder(
          stream: bloc.customers,
          builder: (context, AsyncSnapshot<List<Customer>> snapshot) {
            if (!snapshot.hasData)
              return Center(child: CircularProgressIndicator());
            return ListGrid(
              hasData: snapshot.hasData,
              length: snapshot.hasData ? snapshot.data.length : 0,
              getTile: (index) {
                return CustomerTile(snapshot.data[index]);
              },
            );
          },
        ),
      ),
    );
  }
}
