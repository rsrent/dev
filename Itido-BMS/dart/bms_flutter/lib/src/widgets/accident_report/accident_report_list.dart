import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/src/widgets/request_widget.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/accident_report_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AccidentReportList extends StatelessWidget {
  final Function(AccidentReport) onSelect;

  const AccidentReportList({Key key, this.onSelect}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final accidentReportListBloc =
        BlocProvider.of<AccidentReportListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: accidentReportListBloc,
      builder: (context, ListState<AccidentReport> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<AccidentReport>) {
          if (state.items.isEmpty) {
            return InfoListView(
                info: Translations.of(context).infoNoAccidentReports);
          }
          return ListView.builder(
            padding: EdgeInsets.only(top: 8, bottom: 80),
            itemBuilder: (BuildContext context, int index) {
              return AccidentReportTile(
                accidentReport: state.items[index],
                onSelect: onSelect,
              );
            },
            itemCount: state.items.length,
          );
        }
      },
    );
  }
}

class AccidentReportTile extends StatelessWidget {
  final AccidentReport accidentReport;
  final Function(AccidentReport) onSelect;

  const AccidentReportTile({
    Key key,
    @required this.accidentReport,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final accidentReportListBloc =
        BlocProvider.of<AccidentReportListBloc>(context);

    return Card(
      child: ExpansionTile(
        title: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            Text(
              Translations.of(context)
                  .accidentReportTypeString(accidentReport.accidentReportType),
              style: TextStyle(fontSize: 20),
            ),
            RequestWidget.info(request: accidentReport.request),
          ],
        ),
        children: <Widget>[
          ListTile(
            title: Text(Translations.of(context).labelAccidentTime),
            subtitle: Text(Translations.of(context)
                .dateTimeString(accidentReport.dateTime)),
          ),
          ListTile(
            title: Text(Translations.of(context).labelAccidentPlace),
            subtitle: Text(accidentReport.place),
          ),
          ListTile(
            title: Text(Translations.of(context).labelAccidentDescription),
            subtitle: Text(accidentReport.description),
          ),
          ListTile(
            title: Text(
                accidentReport.accidentReportType == AccidentReportType.Accident
                    ? Translations.of(context).labelAccidentActionTaken
                    : Translations.of(context).labelAlmostAccidentActionTaken),
            subtitle: Text(accidentReport.actionTaken),
          ),
          if (accidentReport.accidentReportType == AccidentReportType.Accident)
            ListTile(
              title:
                  Text(Translations.of(context).labelAccidentAbsenceDuration),
              subtitle: Text('${accidentReport.absenceDurationDays}'),
            ),
          RequestWidget.horizontal(
            request: accidentReport.request,
            onRespond: (isApproved) {
              accidentReportListBloc.dispatch(AccidentReportListRespond(
                id: accidentReport.id,
                isApproved: isApproved,
              ));
            },
          ),
        ],
      ),
    );
  }
}
