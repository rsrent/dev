import 'package:flutter/material.dart';
import '../../models/log.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;

class LogTile extends StatelessWidget {
  final Log log;
  LogTile(this.log);
  @override
  Widget build(BuildContext context) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Text(
              '${log.title != '' ? log.title : "Titel mangler"}',
              style: TextStyle(fontSize: 20.0, color: Colors.blue),
            ),
            Text('${log.locationName}, ${log.customerName}'),
            Padding(
              padding: const EdgeInsets.only(top: 8.0, bottom: 8.0),
              child: Text(
                '${log.log}',
                maxLines: 10,
              ),
            ),

            Row(
              children: <Widget>[
                Expanded(child: Text('${log.userName}')),
                Text(
                  '${dtOps.toDDMM(log.time)}/${log.time.year} ${dtOps.toHHmm(log.time)}',
                  style: TextStyle(color: Colors.grey),
                ),
              ],
            ),
            //Divider(color: Colors.blue[700],)
          ],
        ),
      ),
    );
  }
}
