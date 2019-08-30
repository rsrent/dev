import 'package:bms_dart/address_create_update_bloc.dart';
import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_flutter/components.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AddressCreateUpdateForm extends StatefulWidget {
  @override
  _AddressCreateUpdateFormState createState() =>
      _AddressCreateUpdateFormState();
}

class _AddressCreateUpdateFormState extends State<AddressCreateUpdateForm> {
  TextEditingController _addressNameController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<AddressCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, AddressCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _addressNameController = (_addressNameController ??
              TextEditingController())
            ..text = state.address.addressName;
        }
      },
      child: SingleChildScrollView(
        child: Padding(
            padding: const EdgeInsets.all(24.0),
            child: BlocBuilder(
              bloc: bloc,
              builder: (context, AddressCreateUpdateState state) {
                return Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    TextField(
                      decoration:
                          InputDecoration(labelText: 'Address', filled: true),
                      controller: _addressNameController,
                      onChanged: (text) => bloc.dispatch(
                        AddressNameChanged(text: text),
                      ),
                    ),
                    Space(height: 40),
                    Center(
                      child: RaisedButton(
                        child: Text('SUBMIT'),
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
            )),
      ),
    );
  }
}
