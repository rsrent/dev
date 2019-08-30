import 'package:bms_flutter/translations.dart';

import '../components/date_time_picker.dart';
import '../components/streamer_drop_down_button.dart';
import '../components/streamer_text_field.dart';
import 'package:bms_dart/models.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/contract_create_update_bloc.dart';
import 'package:rxdart/rxdart.dart';

class ContractCreateUpdateForm extends StatefulWidget {
  final bool isEdit;

  const ContractCreateUpdateForm({Key key, @required this.isEdit})
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
        print('state $state');

        if (state is PreparingUpdate) {
          _weeklyHoursController = (_weeklyHoursController ??
              TextEditingController())
            ..text = '${state.contract.weeklyHours}';
        }
        if (state is PreparingCreate) {
          _weeklyHoursController =
              (_weeklyHoursController ?? TextEditingController());
          _weeklyHoursController.clear();
        }
      },
      child: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            widget.isEdit
                ? InputDecorator(
                    decoration: InputDecoration(
                      labelText: Translations.of(context).labelAgreement,
                      hintText: Translations.of(context).hintSelectAgreement,
                      contentPadding: EdgeInsets.zero,
                    ),
                    child: Padding(
                      padding: const EdgeInsets.only(bottom: 8.0),
                      child: Text(bloc.selectedAgreement.value?.name ?? ''),
                    ),
                  )
                : StreamerDropDownButton<Agreement>(
                    labelText: Translations.of(context).labelAgreement,
                    hintText: Translations.of(context).hintSelectAgreement,
                    allValuesStreamer: bloc.allAgreements,
                    selectedValueStreamer: bloc.selectedAgreement,
                    valueToString: (v) => v.name,
                  ),
            StreamBuilder(
              stream: Observable.combineLatest2(
                  bloc.from.stream, bloc.to.stream, (f, t) => [f, t]),
              builder: (BuildContext context, AsyncSnapshot snapshot) {
                return Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: <Widget>[
                      DateTimePicker(
                        labelText: Translations.of(context).labelFrom,
                        selectedDate: bloc.from.value,
                        selectDate: bloc.from.update,
                        lastDate: bloc.to.value,
                      ),
                      DateTimePicker(
                        labelText: Translations.of(context).labelTo,
                        selectedDate: bloc.to.value,
                        selectDate: bloc.to.update,
                        firstDate: bloc.from.value,
                      ),
                    ]);
              },
            ),
            StreamerTextField(
              labelText: Translations.of(context).labelHoursWeekly,
              controller: _weeklyHoursController,
              streamer: bloc.weeklyHours,
            )
          ],
        ),
      ),
    );
  }
}
