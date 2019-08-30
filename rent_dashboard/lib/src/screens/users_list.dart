import 'package:flutter/material.dart';
import '../blocs/user_provider.dart';
import '../models/user.dart';
import '../widgets/cells/user_tile.dart';
import '../widgets/filter_widget.dart';
import '../widgets/searchable_appbar.dart';
import '../widgets/list_grid.dart';

class UsersList extends StatelessWidget {
  final String title;
  UsersList({Key key, this.title}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final bloc = UserProvider.of(context);
    bloc.fetchServiceLeaders();

    return Scaffold(
      resizeToAvoidBottomPadding: false,
      appBar: searchableAppBar(
        bloc: bloc,
        title: '$title, Søg efter serviceleder..',
      ),
      body: FliterWidget(
        sortBy: UserBloc.sortableBy,
        updateSortBy: (s) {
          bloc.sortBy(s);
        },
        currentSort: () => bloc.beingSortedBy,
        updateQualityReportStatusOptions: bloc.updateQualityReportStatusOptions,
        updateCleaningTaskStatusOptions: bloc.updateCleaningTaskStatusOptions,
        child: StreamBuilder(
          stream: bloc.serviceLeaders,
          builder: (context, AsyncSnapshot<List<User>> snapshot) {
            if (!snapshot.hasData)
              return Center(child: CircularProgressIndicator());
            return ListGrid(
              hasData: snapshot.hasData,
              length: snapshot.hasData ? snapshot.data.length : 0,
              getTile: (index) {
                return UserTile(snapshot.data[index]);
              },
            );
          },
        ),
      ),
    );
  }
}
