import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/streamer_text_field.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/agreement_create_update_bloc.dart';

class AgreementCreateUpdateForm extends StatefulWidget {
  final bool isCreate;

  const AgreementCreateUpdateForm({Key key, @required this.isCreate})
      : super(key: key);

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
      child: BlocBuilder(
        bloc: bloc,
        builder: (context, AgreementCreateUpdateState state) {
          return Padding(
            padding: const EdgeInsets.all(24.0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                StreamerTextField(
                  controller: _nameController,
                  labelText: Translations.of(context).labelName,
                  streamer: bloc.name,
                ),
                Space(height: 40),
                Center(
                  child: RaisedButton(
                    child: Text(
                      widget.isCreate
                          ? Translations.of(context).buttonCreate
                          : Translations.of(context).buttonUpdate,
                    ),
                    onPressed: () {
                      if (widget.isCreate) {
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
          );
        },
      ),
    );
  }
}
