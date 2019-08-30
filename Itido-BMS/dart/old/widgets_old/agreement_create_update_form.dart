import 'package:bms_flutter/translations.dart';

import '../components/streamer_text_field.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/agreement_create_update_bloc.dart';

class AgreementCreateUpdateForm extends StatefulWidget {
  @override
  _AgreementCreateUpdateFormState createState() =>
      _AgreementCreateUpdateFormState();
}

class _AgreementCreateUpdateFormState extends State<AgreementCreateUpdateForm> {
  TextEditingController _nameController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<AgreementCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, state) {
        if (state is PreparingUpdate) {
          _nameController = TextEditingController(text: state.agreement.name);
        }
      },
      child: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            StreamerTextField(
              controller: _nameController,
              labelText: Translations.of(context).labelName,
              streamer: bloc.name,
            ),
          ],
        ),
      ),
    );
  }
}
