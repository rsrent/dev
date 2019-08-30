import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_flutter/src/components/primary_button.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/src/widgets/log_create_update_form.dart';
import 'package:bms_dart/blocs.dart';

class LogUpdateScreen extends StatelessWidget {
  final Log logToUpdate;

  const LogUpdateScreen({
    Key key,
    @required this.logToUpdate,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          //'${Translations.of(context).titleUpdateLog}: ${logToUpdate.name}',
          '',
        ),
      ),
      body: BlocProvider(
        builder: (context) {
          return LogCreateUpdateBloc(logToUpdate.id)
            ..dispatch(PrepareUpdate(log: logToUpdate));
        },
        child: Builder(
          builder: (context) {
            var _bloc = BlocProvider.of<LogCreateUpdateBloc>(context);

            return BlocListener(
              bloc: _bloc,
              listener: (context, LogCreateUpdateState state) {
                if (state.createUpdateStatePhase ==
                    CreateUpdateStatePhase.Failed) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content: Text(Translations.of(context).infoUpdateFailed),
                    ));
                }
              },
              child: BlocBuilder(
                bloc: _bloc,
                builder: (context, LogCreateUpdateState state) {
                  return SingleChildScrollView(
                    child: Stack(
                      children: <Widget>[
                        Column(
                          crossAxisAlignment: CrossAxisAlignment.stretch,
                          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                          children: <Widget>[
                            LogCreateUpdateForm(),
                            PrimaryButton(
                              text: Translations.of(context).buttonUpdate,
                              onPressed: () {
                                _bloc.dispatch(Commit());
                              },
                            ),
                          ],
                        ),
                        if (state is Loading)
                          Center(
                            child: CircularProgressIndicator(),
                          )
                      ],
                    ),
                  );
                },
              ),
            );
          },
        ),
      ),
    );
  }
}
