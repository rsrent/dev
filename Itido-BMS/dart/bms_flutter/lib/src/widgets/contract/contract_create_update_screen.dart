import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/contract_create_update_bloc.dart';
import 'package:bms_flutter/src/components/show_snack_text.dart';
import 'package:bms_flutter/src/widgets/contract/contract_create_update_form.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class ContractCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Contract contract,
    int userId,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => ContractCreateUpdateScreen(
        contractToUpdate: contract,
        userId: userId,
      ),
    ));
  }

  final Contract contractToUpdate;
  final int userId;
  final bool isCreate;

  const ContractCreateUpdateScreen(
      {Key key, this.contractToUpdate, this.userId})
      : isCreate = contractToUpdate == null,
        super(key: key);

  @override
  _ContractCreateUpdateScreenState createState() =>
      _ContractCreateUpdateScreenState();
}

class _ContractCreateUpdateScreenState
    extends State<ContractCreateUpdateScreen> {
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
                ? Translations.of(context).titleCreateContract
                : Translations.of(context).titleUpdateContract,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            if (widget.isCreate)
              return ContractCreateUpdateBloc(userId: widget.userId)
                ..dispatch(PrepareCreate());
            else
              return ContractCreateUpdateBloc()
                ..dispatch(
                    PrepareUpdate(contract: this.widget.contractToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc = BlocProvider.of<ContractCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, ContractCreateUpdateState state) {
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
                child: ContractCreateUpdateForm(
                  isCreate: widget.isCreate,
                ),
              );
            },
          ),
        ),
      ),
    );
  }
}
