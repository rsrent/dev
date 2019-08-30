import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/absence_create_update_bloc.dart';
import 'package:bms_flutter/src/widgets/absence/absence_create_update_form.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AbsenceCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Absence absence,
    int userId,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => AbsenceCreateUpdateScreen(
        absenceToUpdate: absence,
        userId: userId,
      ),
    ));
  }

  final Absence absenceToUpdate;
  final int userId;
  final bool isCreate;

  const AbsenceCreateUpdateScreen({Key key, this.absenceToUpdate, this.userId})
      : isCreate = absenceToUpdate == null,
        super(key: key);

  @override
  _AbsenceCreateUpdateScreenState createState() =>
      _AbsenceCreateUpdateScreenState();
}

class _AbsenceCreateUpdateScreenState extends State<AbsenceCreateUpdateScreen> {
  bool updated = false;
  QueryResultBloc _queryResultBloc = QueryResultBloc();

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
                ? Translations.of(context).titleCreateAbsence
                : Translations.of(context).titleUpdateAbsence,
          ),
        ),
        body: QueryResultScreen(
          blocs: [_queryResultBloc],
          child: BlocProvider(
            builder: (context) {
              if (widget.isCreate)
                return AbsenceCreateUpdateBloc(
                  userId: widget.userId,
                  sprog: () => Translations.of(context),
                  queryResultBloc: _queryResultBloc,
                )..dispatch(PrepareCreate());
              else
                return AbsenceCreateUpdateBloc(
                  sprog: () => Translations.of(context),
                  queryResultBloc: _queryResultBloc,
                )..dispatch(
                    PrepareUpdate(absence: this.widget.absenceToUpdate));
            },
            child: Builder(
              builder: (context) {
                var _bloc = BlocProvider.of<AbsenceCreateUpdateBloc>(context);

                return BlocListener(
                  bloc: _bloc,
                  listener: (context, AbsenceCreateUpdateState state) {
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
                  child: AbsenceCreateUpdateForm(isCreate: widget.isCreate),
                );
              },
            ),
          ),
        ),
      ),
    );
  }
}
