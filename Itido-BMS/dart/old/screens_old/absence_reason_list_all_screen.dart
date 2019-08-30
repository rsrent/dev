import 'package:bms_dart/absence_reason_list_bloc.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_flutter/widgets.dart';
import 'absence_reason_create_screen.dart';
import 'absence_reason_update_screen.dart';

class AbsenceReasonListAllScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return BlocListScreen(
      blocBuilder: (context) =>
          AbsenceReasonListBloc(() => AbsenceReasonListFetch()),
      body: AbsenceReasonList(
        onFolderSelect: (agreement) {
          Navigator.of(context).push(MaterialPageRoute(
            builder: (context) => AbsenceReasonUpdateScreen(
              absenceReasonToUpdate: agreement,
            ),
          ));
        },
      ),
      appBar: AppBar(
        title: Text(Translations.of(context).buttonAbsenseReasons),
      ),
      floatingActionButton: FloatingActionButton(
        child: Icon(Icons.add),
        onPressed: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (context) => AbsenceReasonCreateScreen(),
            ),
          );
        },
      ),
    );
  }
}
