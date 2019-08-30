import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/agreement_create_update_bloc.dart';
import 'package:bms_flutter/src/components/show_snack_text.dart';
import 'package:bms_flutter/src/widgets/agreement/agreement_create_update_form.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AgreementCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Agreement agreement,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => AgreementCreateUpdateScreen(
        agreementToUpdate: agreement,
      ),
    ));
  }

  final Agreement agreementToUpdate;
  final bool isCreate;

  const AgreementCreateUpdateScreen({Key key, this.agreementToUpdate})
      : isCreate = agreementToUpdate == null,
        super(key: key);

  @override
  _AgreementCreateUpdateScreenState createState() =>
      _AgreementCreateUpdateScreenState();
}

class _AgreementCreateUpdateScreenState
    extends State<AgreementCreateUpdateScreen> {
  bool updated = false;

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
                ? Translations.of(context).titleCreateAgreement
                : Translations.of(context).titleUpdateAgreement,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            if (widget.isCreate)
              return AgreementCreateUpdateBloc()..dispatch(PrepareCreate());
            else
              return AgreementCreateUpdateBloc()
                ..dispatch(
                    PrepareUpdate(agreement: this.widget.agreementToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc = BlocProvider.of<AgreementCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, AgreementCreateUpdateState state) {
                  if (state is CreateFailure || state is UpdateFailure) {
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationFailed
                            : Translations.of(context).infoUpdateFailed);
                  }
                  if (state is CreateSuccessful || state is UpdateSuccessful) {
                    updated = true;
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationSuccessful
                            : Translations.of(context).infoUpdateSuccessful);
                  }
                },
                child: AgreementCreateUpdateForm(isCreate: widget.isCreate),
              );
            },
          ),
        ),
      ),
    );
  }
}
