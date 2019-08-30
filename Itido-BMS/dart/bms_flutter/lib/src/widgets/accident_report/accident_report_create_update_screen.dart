import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/accident_report_create_update_bloc.dart';
import 'package:bms_flutter/src/widgets/accident_report/accident_report_create_update_form.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AccidentReportCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    AccidentReport accidentReport,
    int userId,
    AccidentReportType accidentReportType,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => AccidentReportCreateUpdateScreen(
        accidentReportToUpdate: accidentReport,
        userId: userId,
        accidentReportType: accidentReportType,
      ),
    ));
  }

  final AccidentReport accidentReportToUpdate;
  final int userId;
  final AccidentReportType accidentReportType;
  final bool isCreate;

  const AccidentReportCreateUpdateScreen({
    Key key,
    this.accidentReportToUpdate,
    this.userId,
    this.accidentReportType,
  })  : isCreate = accidentReportToUpdate == null,
        super(key: key);

  @override
  _AccidentReportCreateUpdateScreenState createState() =>
      _AccidentReportCreateUpdateScreenState();
}

class _AccidentReportCreateUpdateScreenState
    extends State<AccidentReportCreateUpdateScreen> {
  bool updated = false;

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async => false,
      child: Scaffold(
        appBar: AppBar(
          leading: IconButton(
            icon: Icon(Icons.arrow_back),
            onPressed: () {
              Navigator.of(context).pop(updated);
            },
          ),
          title: Text(
            widget.isCreate
                ? widget.accidentReportType == AccidentReportType.Accident
                    ? Translations.of(context).titleCreateAccidentReport
                    : Translations.of(context).titleCreateAlmostAccidentReport
                : widget.accidentReportToUpdate.accidentReportType ==
                        AccidentReportType.Accident
                    ? Translations.of(context).titleUpdateAccidentReport
                    : Translations.of(context).titleUpdateAlmostAccidentReport,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            if (widget.isCreate)
              return AccidentReportCreateUpdateBloc(
                  userId: widget.userId,
                  accidentReportType: widget.accidentReportType)
                ..dispatch(PrepareCreate());
            else
              return AccidentReportCreateUpdateBloc()
                ..dispatch(PrepareUpdate(
                    accidentReport: this.widget.accidentReportToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc =
                  BlocProvider.of<AccidentReportCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, AccidentReportCreateUpdateState state) {
                  if (state.createUpdateStatePhase ==
                      CreateUpdateStatePhase.Failed) {
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationFailed
                            : Translations.of(context).infoUpdateFailed);
                  }
                  if (state.createUpdateStatePhase ==
                      CreateUpdateStatePhase.Successful) {
                    updated = true;
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationSuccessful
                            : Translations.of(context).infoUpdateSuccessful);
                  }
                },
                child: AccidentReportCreateUpdateForm(),
              );
            },
          ),
        ),
      ),
    );
  }
}
