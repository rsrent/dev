import 'package:bms_dart/noti_list_bloc.dart';
import 'package:bms_flutter_admin/src/screens/user_inspect_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_flutter/src/widgets/noti/widgets.dart';

class NotiListAllScreenAdmin extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return NotiListScreen(
      blocBuilder: (context) =>
          NotiListBloc(() => NotiListFetchLatest(count: 10)),
      onSelect: (noti) {
        if (noti.notiType.startsWith('Absence_')) {
          UserInspectScreen.show(context, noti.subjectId, startIndex: 1);
        }
        if (noti.notiType.startsWith('AccidentReport_')) {
          UserInspectScreen.show(context, noti.subjectId, startIndex: 3);
        }
      },
    );
  }
}
