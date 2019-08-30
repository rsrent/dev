import 'package:bms_flutter/src/components/primary_button.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/src/widgets/absence_create_update_form.dart';
import 'package:bms_dart/absence_create_update_bloc.dart';

class AbsenceUpdateScreen extends StatelessWidget {
  final Absence absenceToUpdate;
  final User user;
  AbsenceUpdateScreen({@required this.user, this.absenceToUpdate});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(Translations.of(context).titleUpdateAbsence),
      ),
      body: BlocProvider(
        builder: (context) {
          return AbsenceCreateUpdateBloc(user: user)
            ..dispatch(PrepareUpdate(absence: absenceToUpdate));
        },
        child: Builder(
          builder: (context) {
            var _bloc = BlocProvider.of<AbsenceCreateUpdateBloc>(context);
            return BlocListener(
              bloc: _bloc,
              listener: (context, AbsenceCreateUpdateState state) {
                if (state is UpdateFailure) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content: Text(Translations.of(context).infoUpdateFailed),
                    ));
                } else if (state is UpdateSuccessful) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content:
                          Text(Translations.of(context).infoUpdateSuccessful),
                    ));
                }
              },
              child: BlocBuilder(
                bloc: _bloc,
                builder: (context, AbsenceCreateUpdateState state) {
                  return bodyBuilder(context, state, _bloc);
                },
              ),
            );
          },
        ),
      ),
    );
  }

  Widget bodyBuilder(BuildContext context, AbsenceCreateUpdateState state,
      AbsenceCreateUpdateBloc bloc) {
    return SingleChildScrollView(
      child: Stack(
        children: <Widget>[
          Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: <Widget>[
              AbsenceCreateUpdateForm(),
              StreamBuilder<bool>(
                stream: bloc.formValid.stream,
                builder: (BuildContext context, AsyncSnapshot snapshot) {
                  return PrimaryButton(
                    onPressed: () {
                      bloc.dispatch(UpdateRequested());
                    },
                    text: Translations.of(context).buttonUpdate,
                    disabled: !(snapshot.data ?? false),
                  );
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
  }
}
