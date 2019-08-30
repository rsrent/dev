import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/customer_create_update_bloc.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'customer_create_update_form.dart';

class CustomerCreateUpdateScreen extends StatefulWidget {
  static Future show(BuildContext context, Customer customer) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => CustomerCreateUpdateScreen(
        customerToUpdate: customer,
      ),
    ));
  }

  final Customer customerToUpdate;
  final bool isCreate;

  const CustomerCreateUpdateScreen({Key key, this.customerToUpdate})
      : isCreate = customerToUpdate == null,
        super(key: key);

  @override
  _CustomerCreateUpdateScreenState createState() =>
      _CustomerCreateUpdateScreenState();
}

class _CustomerCreateUpdateScreenState
    extends State<CustomerCreateUpdateScreen> {
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
                ? Translations.of(context).titleCreateCustomer
                : Translations.of(context).titleUpdateCustomer,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            if (widget.isCreate)
              return CustomerCreateUpdateBloc()..dispatch(PrepareCreate());
            else
              return CustomerCreateUpdateBloc()
                ..dispatch(
                    PrepareUpdate(customer: this.widget.customerToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc = BlocProvider.of<CustomerCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, CustomerCreateUpdateState state) {
                  print('${state.createUpdateStatePhase}');
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
                child: CustomerCreateUpdateForm(isCreate: widget.isCreate),
              );
            },
          ),
        ),
      ),
    );
  }
}
