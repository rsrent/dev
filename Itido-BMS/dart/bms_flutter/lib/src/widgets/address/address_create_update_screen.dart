import 'package:bms_dart/address_create_update_bloc.dart';
import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/widgets/address/address_create_update_form.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AddressCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Address address,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => AddressCreateUpdateScreen(
        addressToUpdate: address,
      ),
    ));
  }

  final Address addressToUpdate;
  final bool isCreate;

  const AddressCreateUpdateScreen({Key key, this.addressToUpdate})
      : isCreate = addressToUpdate == null,
        super(key: key);

  @override
  _AddressCreateUpdateScreenState createState() =>
      _AddressCreateUpdateScreenState();
}

class _AddressCreateUpdateScreenState extends State<AddressCreateUpdateScreen> {
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
                ? Translations.of(context).titleCreateAddress
                : Translations.of(context).titleUpdateAddress,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            return AddressCreateUpdateBloc(this.widget.addressToUpdate.id)
              ..dispatch(PrepareUpdate(address: this.widget.addressToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc = BlocProvider.of<AddressCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, AddressCreateUpdateState state) {
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
                child: AddressCreateUpdateForm(),
              );
            },
          ),
        ),
      ),
    );
  }
}
