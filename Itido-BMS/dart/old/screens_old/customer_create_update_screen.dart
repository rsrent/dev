import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/customer_create_update_bloc.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class CustomerCreateUpdateScreen extends StatelessWidget {
  final Customer customerToUpdate;
  final bool isCreate;

  const CustomerCreateUpdateScreen({Key key, this.customerToUpdate})
      : isCreate = customerToUpdate == null,
        super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          isCreate
              ? Translations.of(context).titleCreateCustomer
              : Translations.of(context).titleUpdateCustomer,
        ),
      ),
      body: BlocProvider(
        builder: (context) {
          if (isCreate)
            return CustomerCreateUpdateBloc()..dispatch(PrepareCreate());
          else
            return CustomerCreateUpdateBloc()
              ..dispatch(PrepareUpdate(customer: this.customerToUpdate));
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
                      isCreate
                          ? Translations.of(context).infoCreationFailed
                          : Translations.of(context).infoUpdateFailed);
                }
                if (state.createUpdateStatePhase ==
                    CreateUpdateStatePhase.Successful) {
                  showSnackText(
                      context,
                      isCreate
                          ? Translations.of(context).infoCreationSuccessful
                          : Translations.of(context).infoUpdateSuccessful);
                }
              },
              child: CustomerCreateUpdateForm(),
            );
          },
        ),
      ),
    );
  }
}
