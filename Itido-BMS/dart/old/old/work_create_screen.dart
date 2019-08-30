import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_flutter/src/components/primary_button.dart';
import 'package:bms_flutter/src/components/primary_button.dart';
import 'package:bms_flutter/src/widgets/loading_overlay.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/translations.dart';
import 'package:dart_packages/streamer.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/src/widgets/work_create_update_form.dart';
import 'package:bms_dart/work_create_update_bloc.dart';
import 'package:rxdart/rxdart.dart';

class WorkCreateScreen extends StatelessWidget {
  final int locationId;

  const WorkCreateScreen({Key key, @required this.locationId})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(Translations.of(context).titleCreateWork),
      ),
      body: BlocProvider(
        builder: (context) {
          return WorkCreateUpdateBloc(
            projectItemId: this.locationId,
          )..dispatch(PrepareCreate());
        },
        child: Builder(
          builder: (context) {
            var _bloc = BlocProvider.of<WorkCreateUpdateBloc>(context);
            return BlocListener(
              bloc: _bloc,
              listener: (context, WorkCreateUpdateState state) {
                if (state.createUpdateStatePhase ==
                    CreateUpdateStatePhase.Failed) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content:
                          Text(Translations.of(context).infoCreationFailed),
                    ));
                }
                if (state.createUpdateStatePhase ==
                    CreateUpdateStatePhase.Successful) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content:
                          Text(Translations.of(context).infoCreationSuccessful),
                    ));
                  _bloc.dispatch(PrepareCreate());
                }
              },
              child: WorkCreateUpdateForm(),
            );
          },
        ),
      ),
    );
  }
}
