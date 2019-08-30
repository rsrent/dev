import 'package:flutter/material.dart';
import '../blocs/log_provider.dart';
import '../blocs/data_provider.dart';
import '../models/log.dart';
import '../widgets/cells/log_tile.dart';
import '../widgets/list_grid.dart';

class LogsList extends StatelessWidget {
  final String title;
  LogsList({Key key, this.title}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final logsBloc = LogProvider.of(context);
    logsBloc.fetch();

    return Scaffold(
      appBar: AppBar(
        title: Text('$title, Logs'),
      ),
      body: StreamBuilder(
        stream: logsBloc.logs,
        builder: (context, AsyncSnapshot<List<Log>> snapshot) {
          if (!snapshot.hasData)
              return Center(child: CircularProgressIndicator());
          return ListGrid(
            hasData: snapshot.hasData,
            length: snapshot.hasData ? snapshot.data.length : 0,
            getTile: (index) {
              return LogTile(snapshot.data[index]);
            },
            ratio: 1.5,
          );
        },
      ),
    );
  }
}
