import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/src/widgets/work_contract_create_update_form.dart';
import 'package:bms_dart/work_contract_create_update_bloc.dart';

class WorkContractCreateScreen extends StatelessWidget {
  final int locationId;

  const WorkContractCreateScreen({Key key, @required this.locationId})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(Translations.of(context).titleCreateWorkContract),
      ),
      body: BlocProvider(
        builder: (context) {
          return WorkContractCreateUpdateBloc(
            projectItemId: this.locationId,
          )..dispatch(PrepareCreate());
        },
        child: Builder(
          builder: (context) {
            var _bloc = BlocProvider.of<WorkContractCreateUpdateBloc>(context);
            return BlocListener(
              bloc: _bloc,
              listener: (context, WorkContractCreateUpdateState state) {
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
              child: WorkContractCreateUpdateForm(),
            );
          },
        ),
      ),
    );
  }
}
