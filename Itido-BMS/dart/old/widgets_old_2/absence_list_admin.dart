import 'package:bms_dart/absence_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:bms_flutter_admin/src/screens/absence_update_screen.dart';
import 'package:flutter/material.dart';

class AbsenceListAdmin {
  static Widget getAbsenceListOfUser(BuildContext context, User user) {
    return BlocListHalfScreen<AbsenceListBloc, AbsenceListEvent,
        ListState<Absence>, Absence>(
      child: AbsenceList(
        onFolderSelect: (absence) {
          Navigator.of(context).push(MaterialPageRoute(
            builder: (context) => AbsenceUpdateScreen(
              user: user,
              absenceToUpdate: absence,
            ),
          ));
        },
      ),
      floatingActionButton: FloatingActionButton(
        //heroTag: null,
        child: Icon(Icons.add),
        onPressed: () {
          Navigator.of(context).push(MaterialPageRoute(builder: (context) {
            return AbsenceCreateScreen(user: user);
          }));
        },
      ),
      onSelectMany: (bloc, _) {},
    );
  }
}
