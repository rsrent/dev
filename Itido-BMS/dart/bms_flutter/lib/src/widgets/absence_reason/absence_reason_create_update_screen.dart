import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/absence_reason_create_update_bloc.dart';
import 'package:bms_flutter/src/widgets/absence_reason/absence_reason_create_update_form.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AbsenceReasonCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    AbsenceReason absenceReason,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => AbsenceReasonCreateUpdateScreen(
        absenceReasonToUpdate: absenceReason,
      ),
    ));
  }

  final AbsenceReason absenceReasonToUpdate;
  final bool isCreate;

  const AbsenceReasonCreateUpdateScreen({Key key, this.absenceReasonToUpdate})
      : isCreate = absenceReasonToUpdate == null,
        super(key: key);

  @override
  _AbsenceReasonCreateUpdateScreenState createState() =>
      _AbsenceReasonCreateUpdateScreenState();
}

class _AbsenceReasonCreateUpdateScreenState
    extends State<AbsenceReasonCreateUpdateScreen> {
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
                ? Translations.of(context).titleCreateAbsenceReason
                : Translations.of(context).titleUpdateAbsenceReason,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            if (widget.isCreate)
              return AbsenceReasonCreateUpdateBloc()..dispatch(PrepareCreate());
            else
              return AbsenceReasonCreateUpdateBloc()
                ..dispatch(PrepareUpdate(
                    absenceReason: this.widget.absenceReasonToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc =
                  BlocProvider.of<AbsenceReasonCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, AbsenceReasonCreateUpdateState state) {
                  if (state is CreateFailure || state is UpdateFailure) {
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationFailed
                            : Translations.of(context).infoUpdateFailed);
                  }
                  if (state is CreateSuccessful || state is UpdateSuccessful) {
                    updated = true;
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationSuccessful
                            : Translations.of(context).infoUpdateSuccessful);
                  }
                },
                child: AbsenceReasonCreateUpdateForm(),
              );
            },
          ),
        ),
      ),
    );
  }
}
