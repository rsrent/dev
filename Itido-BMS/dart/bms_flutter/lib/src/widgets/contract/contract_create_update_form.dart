import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/date_time_picker.dart';
import 'package:bms_flutter/src/components/decorated_drop_down_button.dart';
import 'package:bms_flutter/translations.dart';

import 'package:bms_dart/models.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/contract_create_update_bloc.dart';

class ContractCreateUpdateForm extends StatefulWidget {
  final bool isCreate;

  const ContractCreateUpdateForm({Key key, @required this.isCreate})
      : super(key: key);
  @override
  _ContractCreateUpdateFormState createState() =>
      _ContractCreateUpdateFormState();
}

class _ContractCreateUpdateFormState extends State<ContractCreateUpdateForm> {
  TextEditingController _weeklyHoursController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<ContractCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, ContractCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _weeklyHoursController = (_weeklyHoursController ??
              TextEditingController())
            ..text = (state.contract.weeklyHours ?? '').toString();
        }
      },
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: BlocBuilder(
            bloc: bloc,
            builder: (context, ContractCreateUpdateState state) {
              print(state.selectedAgreement?.name);

              return Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  !widget.isCreate
                      ? InputDecorator(
                          decoration: InputDecoration(
                            labelText: Translations.of(context).labelAgreement,
                            hintText:
                                Translations.of(context).hintSelectAgreement,
                            contentPadding: EdgeInsets.zero,
                          ),
                          child: Padding(
                            padding: const EdgeInsets.only(bottom: 8.0),
                            child: Text(state.selectedAgreement?.name ?? ''),
                          ),
                        )
                      : DecoratedDropDownButton<Agreement>(
                          onChanged: (agreement) => bloc
                              .dispatch(AgreementChanged(agreement: agreement)),
                          labelText: Translations.of(context).labelAgreement,
                          hintText:
                              Translations.of(context).hintSelectAgreement,
                          allValues: state.allAgreements,
                          selectedValue: state.selectedAgreement,
                          valueToString: (v) => v.name,
                        ),
                  // : DropdownButton<Agreement>(
                  //     value: state.selectedAgreement,
                  //     onChanged: (agreement) => bloc
                  //         .dispatch(AgreementChanged(agreement: agreement)),
                  //     items: state.allAgreements
                  //         .map<DropdownMenuItem<Agreement>>(
                  //           (a) => DropdownMenuItem<Agreement>(
                  //             value: a,
                  //             child: Text('${a.name}'),
                  //           ),
                  //         )
                  //         .toList(),
                  //     // labelText: Translations.of(context).labelAgreement,
                  //     // hintText:
                  //     //     Translations.of(context).hintSelectAgreement,
                  //     // allValuesStreamer: state.allAgreements,
                  //     // selectedValueStreamer: bloc.selectedAgreement,
                  //     // valueToString: (v) => v.name,
                  //   ),
                  DateTimePicker(
                    labelText: Translations.of(context).labelFrom,
                    selectedDate: state.contract.from,
                    selectDate: (date) =>
                        bloc.dispatch(FromChanged(dateTime: date)),
                    lastDate: state.contract.to,
                  ),
                  DateTimePicker(
                    labelText: Translations.of(context).labelTo,
                    selectedDate: state.contract.to,
                    selectDate: (date) =>
                        bloc.dispatch(ToChanged(dateTime: date)),
                    firstDate: state.contract.from,
                  ),
                  TextField(
                    controller: _weeklyHoursController,
                    decoration: InputDecoration(
                        labelText: Translations.of(context).labelHoursWeekly),
                    onChanged: (text) =>
                        bloc.dispatch(WeeklyHoursChanged(text: text)),
                    keyboardType: TextInputType.number,
                  ),
                  Space(height: 40),
                  Center(
                    child: RaisedButton(
                      child: Text(
                        widget.isCreate
                            ? Translations.of(context).buttonCreate
                            : Translations.of(context).buttonUpdate,
                      ),
                      onPressed: state.isValid
                          ? () {
                              bloc.dispatch(Commit());
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
