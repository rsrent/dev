import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_create_update_bloc.dart';
import 'package:bms_flutter/src/components/show_snack_text.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'work_create_update_form.dart';
import 'package:bms_dart/query_result_bloc.dart';

class WorkCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Work work,
    int projectItemId,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => WorkCreateUpdateScreen(
        workToUpdate: work,
      ),
    ));
  }

  final Work workToUpdate;
  final int projectItemId;
  final bool isCreate;

  const WorkCreateUpdateScreen({Key key, this.workToUpdate, this.projectItemId})
      : isCreate = workToUpdate == null,
        super(key: key);

  @override
  _WorkCreateUpdateScreenState createState() => _WorkCreateUpdateScreenState();
}

class _WorkCreateUpdateScreenState extends State<WorkCreateUpdateScreen> {
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
                ? Translations.of(context).titleCreateWork
                : Translations.of(context).titleUpdateWork,
          ),
        ),
        body: QueryResultScreen(
          blocs: [queryResultBloc],
          child: BlocProvider(
            builder: (context) {
              if (widget.isCreate)
                return WorkCreateUpdateBloc(
                  projectItemId: widget.projectItemId,
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                )..dispatch(PrepareCreate());
              else
                return WorkCreateUpdateBloc(
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                )..dispatch(PrepareUpdate(work: this.widget.workToUpdate));
            },
            child: Builder(
              builder: (context) {
                var _bloc = BlocProvider.of<WorkCreateUpdateBloc>(context);

                return BlocListener(
                  bloc: _bloc,
                  listener: (context, WorkCreateUpdateState state) {
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
                  child: WorkCreateUpdateForm(),
                );
              },
            ),
          ),
        ),
      ),
    );
  }
}
