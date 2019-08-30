import 'package:flutter/material.dart';
import '../blocs/task_provider.dart';
import '../models/task.dart';
import '../widgets/cells/task_tile.dart';
import '../widgets/list_grid.dart';

import '../widgets/filter_widget.dart';
import '../widgets/searchable_appbar.dart';

class TasksList extends StatelessWidget {
  final String title;
  final Function(TaskBloc) prepareBloc;
  TasksList({Key key, this.title, this.prepareBloc}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final bloc = TaskProvider.of(context);
    if (bloc != null && prepareBloc != null) {
      prepareBloc(bloc);
    }
    bloc.fetchTasks();

    return Scaffold(
      resizeToAvoidBottomPadding: false,
      appBar: searchableAppBar(
        bloc: bloc,
        title: '$title, Søg efter opgaver..',
      ),
      body: FliterWidget(
        sortBy: TaskBloc.sortableBy,
        updateSortBy: (s) {
          bloc.sortBy(s);
        },
        updateFilterBy: bloc.filterBy,
        currentSort: () => bloc.beingSortedBy,
        updateCleaningTaskStatusOptions: bloc.updateCleaningTaskStatusOptions,
        updateCleaningTaskPlanOptions: bloc.updateCleaningTaskPlanOptions,
        child: StreamBuilder(
          stream: bloc.tasks,
          builder: (context, AsyncSnapshot<List<Task>> snapshot) {
            if (!snapshot.hasData)
              return Center(child: CircularProgressIndicator());
            return ListGrid(
              hasData: snapshot.hasData,
              length: snapshot.hasData ? snapshot.data.length : 0,
              getTile: (index) {
                return TaskTile(snapshot.data[index]);
              },
            );
          },
        ),
      ),
    );
  }
}
