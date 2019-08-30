import 'package:bms_dart/accident_report_list_bloc.dart';
import 'package:bms_dart/contract_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';

class AccidentReportListAdmin {
  static Widget getAccidentReportListOfUser(BuildContext context, User user) {
    return BlocListHalfScreen<AccidentReportListBloc, AccidentReportListEvent,
        ListState<AccidentReport>, AccidentReport>(
      child: AccidentReportList(),
      floatingActionButton: FloatingActionButton(
        heroTag: null,
        child: Icon(Icons.add),
        onPressed: () {
          AccidentReportCreateScreen.showAccidenReportCreateModule(
              context, user.id);
        },
      ),
    );
  }
}
