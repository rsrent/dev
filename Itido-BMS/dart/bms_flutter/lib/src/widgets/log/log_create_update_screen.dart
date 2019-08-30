import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/widgets/log/log_create_update_form.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class LogCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Log log,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => LogCreateUpdateScreen(
        logToUpdate: log,
      ),
    ));
  }

  final Log logToUpdate;
  final bool isCreate;

  const LogCreateUpdateScreen({Key key, this.logToUpdate})
      : isCreate = logToUpdate == null,
        super(key: key);

  @override
  _LogCreateUpdateScreenState createState() => _LogCreateUpdateScreenState();
}

class _LogCreateUpdateScreenState extends State<LogCreateUpdateScreen> {
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
                ? Translations.of(context).titleCreateLog
                : Translations.of(context).titleUpdateLog,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            return LogCreateUpdateBloc(this.widget.logToUpdate.id)
              ..dispatch(PrepareUpdate(log: this.widget.logToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc = BlocProvider.of<LogCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, LogCreateUpdateState state) {
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
                child: LogCreateUpdateForm(),
              );
            },
          ),
        ),
      ),
    );
  }
}
