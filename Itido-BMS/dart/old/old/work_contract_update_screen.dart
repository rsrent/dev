import 'package:bms_dart/models.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/src/widgets/work_contract_create_update_form.dart';
import 'package:bms_dart/work_contract_create_update_bloc.dart';

class WorkContractUpdateScreen extends StatelessWidget {
  final WorkContract workContract;

  const WorkContractUpdateScreen({Key key, this.workContract})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(Translations.of(context).titleUpdateWorkContract),
      ),
      body: BlocProvider(
        builder: (context) {
          return WorkContractCreateUpdateBloc(
            projectItemId: null,
          )..dispatch(PrepareUpdate(workContract: workContract));
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
                      content: Text(Translations.of(context).infoUpdateFailed),
                    ));
                }
                if (state.createUpdateStatePhase ==
                    CreateUpdateStatePhase.Successful) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content:
                          Text(Translations.of(context).infoUpdateSuccessful),
                    ));
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
