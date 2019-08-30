import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/streamer_check_box_list_tile.dart';
import 'package:bms_flutter/src/components/streamer_text_field.dart';
import 'package:bms_flutter/translations.dart';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/absence_reason_create_update_bloc.dart';

class AbsenceReasonCreateUpdateForm extends StatefulWidget {
  @override
  _AbsenceReasonCreateUpdateFormState createState() =>
      _AbsenceReasonCreateUpdateFormState();
}

class _AbsenceReasonCreateUpdateFormState
    extends State<AbsenceReasonCreateUpdateForm> {
  TextEditingController _descriptionController;

  bool isCreate = false;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<AbsenceReasonCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, state) {
        if (state is PreparingUpdate) {
          _descriptionController = (_descriptionController ??
              TextEditingController())
            ..text = state.absenceReason.description;
          isCreate = false;
        }
        if (state is PreparingCreate) {
          _descriptionController =
              (_descriptionController ?? TextEditingController())..clear();
          isCreate = true;
        }
        setState(() {});
      },
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: <Widget>[
          Padding(
            padding: const EdgeInsets.all(24.0),
            child: StreamerTextField(
              controller: _descriptionController,
              labelText: Translations.of(context).labelDescription,
              streamer: bloc.description,
            ),
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(12, 0, 12, 12),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                StreamerCheckBoxListTile(
                  streamer: bloc.canManagerCreate,
                  labelText: Translations.of(context).labelCanManagerCreate,
                ),
                StreamerCheckBoxListTile(
                  streamer: bloc.canManagerRequest,
                  labelText: Translations.of(context).labelCanManagerRequest,
                ),
                StreamerCheckBoxListTile(
                  streamer: bloc.canUserCreate,
                  labelText: Translations.of(context).labelCanUserCreate,
                ),
                StreamerCheckBoxListTile(
                  streamer: bloc.canUserRequest,
                  labelText: Translations.of(context).labelCanUserRequest,
                ),
                Space(height: 40),
                Center(
                  child: RaisedButton(
                    child: Text(
                      isCreate
                          ? Translations.of(context).buttonCreate
                          : Translations.of(context).buttonUpdate,
                    ),
                    onPressed: () {
                      if (isCreate) {
                        bloc.dispatch(CreateRequested());
                      } else {
                        bloc.dispatch(UpdateRequested());
                      }
                    },
                  ),
                ),
                Space(height: 40),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
