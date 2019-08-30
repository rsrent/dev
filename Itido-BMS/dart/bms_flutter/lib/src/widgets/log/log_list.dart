import 'package:bms_dart/blocs.dart';
import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class LogList extends StatelessWidget {
  final Function(Log) onSelect;
  final Function(Log) onDelete;

  const LogList({Key key, this.onSelect, this.onDelete}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final logListBloc = BlocProvider.of<LogListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: logListBloc,
      builder: (context, ListState<Log> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          var logs = (state as Loaded).items;
          if (logs.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoLogs);
          }
          return ListView.builder(
            padding: EdgeInsets.all(0),
            itemBuilder: (BuildContext context, int index) {
              // if (index == logs.length) {
              //   return InkWell(
              //     child: Container(
              //       height: 100,
              //       child: Center(
              //         child: Row(
              //           mainAxisSize: MainAxisSize.min,
              //           children: <Widget>[
              //             Icon(
              //               Icons.refresh,
              //               color: Theme.of(context).primaryColor,
              //             ),
              //             Padding(
              //               padding: const EdgeInsets.only(left: 16),
              //               child: Text(
              //                 'Load more',
              //                 style: TextStyle(
              //                   color: Theme.of(context).primaryColor,
              //                 ),
              //               ),
              //             ),
              //           ],
              //         ),
              //       ),
              //     ),
              //     onTap: () {
              //       logListBloc.dispatch(LogListFetch(more: true));
              //     },
              //   );
              // }
              return LogTile(
                log: logs[index],
                onSelect: onSelect,
              );
            },
            itemCount: logs.length,
          );
        }
      },
    );
  }
}

class LogTile extends StatelessWidget {
  final Log log;
  final Function(Log) onSelect;

  const LogTile({
    Key key,
    @required this.log,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    var dateTimeString =
        Translations.of(context).dateTimeString(log.dateCreated);
    return Container(
      margin: const EdgeInsets.all(8),
      padding: const EdgeInsets.only(top: 8, bottom: 8),
      color: Color(0xFFF1F9FF),
      child: ListTile(
        isThreeLine: true,
        title: Text(
          log.title ?? '',
          style: TextStyle(fontSize: 20),
        ),
        subtitle: Padding(
          padding: const EdgeInsets.only(top: 8),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              Text(
                (log.log ?? '').trim(),
                // style: TextStyle(fontSize: 16),
              ),
              Divider(
                height: 16,
                color: Colors.transparent,
              ),
              Text(dateTimeString)
            ],
          ),
        ),
        onTap: onSelect != null ? () => onSelect(log) : null,
      ),
    );
  }
}
