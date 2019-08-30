import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/date_time_picker.dart';
import 'package:bms_flutter/src/components/decorated_drop_down_button.dart';
import 'package:bms_flutter/translations.dart';

import 'package:bms_dart/models.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/absence_create_update_bloc.dart';

class AbsenceCreateUpdateForm extends StatefulWidget {
  final bool isCreate;

  const AbsenceCreateUpdateForm({Key key, @required this.isCreate})
      : super(key: key);
  @override
  _AbsenceCreateUpdateFormState createState() =>
      _AbsenceCreateUpdateFormState();
}

class _AbsenceCreateUpdateFormState extends State<AbsenceCreateUpdateForm> {
  TextEditingController _commentController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<AbsenceCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, AbsenceCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _commentController = (_commentController ?? TextEditingController())
            ..text = (state.absence.comment ?? '').toString();
        }
      },
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: BlocBuilder(
            bloc: bloc,
            builder: (context, AbsenceCreateUpdateState state) {
              return Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  !widget.isCreate
                      ? InputDecorator(
                          decoration: InputDecoration(
                            labelText: Translations.of(context).buttonAbsense,
                            hintText: Translations.of(context)
                                .hintSelectAbsenceReason,
                            contentPadding: EdgeInsets.zero,
                          ),
                          child: Padding(
                            padding: const EdgeInsets.only(bottom: 8.0),
                            child: Text(
                                state.selectedAbsenceReason?.description ?? ''),
                          ),
                        )
                      : DecoratedDropDownButton<AbsenceReason>(
                          onChanged: (absenceReason) => bloc.dispatch(
                              AbsenceReasonChanged(
                                  absenceReason: absenceReason)),
                          labelText: Translations.of(context).buttonAbsense,
                          hintText:
                              Translations.of(context).hintSelectAbsenceReason,
                          allValues: state.allAbsenceReasons,
                          selectedValue: state.selectedAbsenceReason,
                          valueToString: (v) => v.description,
                        ),
                  DateTimePicker(
                    labelText: Translations.of(context).labelFrom,
                    selectedDate: state.absence.from,
                    selectDate: (date) =>
                        bloc.dispatch(FromChanged(dateTime: date)),
                    lastDate: state.absence.to,
                  ),
                  DateTimePicker(
                    labelText: Translations.of(context).labelTo,
                    selectedDate: state.absence.to,
                    selectDate: (date) =>
                        bloc.dispatch(ToChanged(dateTime: date)),
                    firstDate: state.absence.from,
                  ),
                  TextField(
                    controller: _commentController,
                    decoration: InputDecoration(
                        labelText: Translations.of(context).labelComment),
                    onChanged: (text) =>
                        bloc.dispatch(CommentChanged(text: text)),
                  ),
                  Space(height: 40),
                  Center(
                    child: RaisedButton(
                      child: Text(
                        widget.isCreate
                            ? Translations.of(context).buttonCreate
                            : Translations.of(context).buttonUpdate,
                      ),
                      onPressed: state.isValid && state.errors.canCreate
                          ? () {
                              bloc.dispatch(Commit(asRequest: false));
                            }
                          : null,
                    ),
                  ),
                  Space(height: 20),
                  Center(
                    child: RaisedButton(
                      child: Text(Translations.of(context).buttonRequest),
                      onPressed: state.isValid && state.errors.canRequest
                          ? () {
                              bloc.dispatch(Commit(asRequest: false));
                            }
                          : null,
                    ),
                  ),
                  Space(height: 40),
                ],
              );
            },
          ),
        ),
      ),
    );
  }
}
