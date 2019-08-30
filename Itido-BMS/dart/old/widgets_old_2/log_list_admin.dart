import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:bms_flutter_admin/src/screens/log_update_screen.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class LogListAdmin {
  static Widget getLogListOfLocation(BuildContext context, int locationId) {
    return Builder(
      builder: (context) {
        return BlocListHalfScreen<LogListBloc, LogListEvent, ListState<Log>,
            Log>(
          child: LogList(
            onFolderSelect: (log) {
              Navigator.of(context).push(MaterialPageRoute(builder: (context) {
                return LogUpdateScreen(
                  logToUpdate: log,
                );
              }));
            },
          ),
          floatingActionButton: FloatingActionButton(
            heroTag: null,
            child: Icon(Icons.add),
            onPressed: () {
              BlocProvider.of<LogListBloc>(context)
                  .dispatch(LogListAddNew(projectItemId: locationId));
            },
          ),
        );
      },
    );
  }
}
