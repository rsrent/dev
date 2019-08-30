import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_contract_create_update_bloc.dart';
import 'package:bms_flutter/src/components/show_snack_text.dart';
import 'package:bms_flutter/src/screens/query_result_screen.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'work_contract_create_update_form.dart';

class WorkContractCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    WorkContract workContract,
    int projectItemId,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => WorkContractCreateUpdateScreen(
        workContractToUpdate: workContract,
      ),
    ));
  }

  final WorkContract workContractToUpdate;
  final int projectItemId;
  final bool isCreate;

  const WorkContractCreateUpdateScreen(
      {Key key, this.workContractToUpdate, this.projectItemId})
      : isCreate = workContractToUpdate == null,
        super(key: key);

  @override
  _WorkContractCreateUpdateScreenState createState() =>
      _WorkContractCreateUpdateScreenState();
}

class _WorkContractCreateUpdateScreenState
    extends State<WorkContractCreateUpdateScreen> {
  bool updated = false;
  var queryResultBloc = QueryResultBloc();

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
                ? Translations.of(context).titleCreateWorkContract
                : Translations.of(context).titleUpdateWorkContract,
          ),
        ),
        body: QueryResultScreen(
          blocs: [queryResultBloc],
          child: BlocProvider(
            builder: (context) {
              if (widget.isCreate)
                return WorkContractCreateUpdateBloc(
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                  projectItemId: widget.projectItemId,
                )..dispatch(PrepareCreate());
              else
                return WorkContractCreateUpdateBloc(
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                )..dispatch(PrepareUpdate(
                    workContract: this.widget.workContractToUpdate));
            },
            child: Builder(
              builder: (context) {
                var _bloc =
                    BlocProvider.of<WorkContractCreateUpdateBloc>(context);

                return BlocListener(
                  bloc: _bloc,
                  listener: (context, WorkContractCreateUpdateState state) {
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
                  child: WorkContractCreateUpdateForm(),
                );
              },
            ),
          ),
        ),
      ),
    );
  }
}
