import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/location_create_update_bloc.dart';
import 'package:bms_flutter/components.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class LocationCreateUpdateForm extends StatefulWidget {
  @override
  _LocationCreateUpdateFormState createState() =>
      _LocationCreateUpdateFormState();
}

class _LocationCreateUpdateFormState extends State<LocationCreateUpdateForm> {
  TextEditingController _nameController;
  TextEditingController _addressController;
  TextEditingController _phoneController;
  TextEditingController _commentController;
  TextEditingController _projectNumberController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<LocationCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, LocationCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _nameController = (_nameController ?? TextEditingController())
            ..text = state.location.name;
          _addressController = (_addressController ?? TextEditingController())
            ..text = state.location.address;
          _phoneController = (_phoneController ?? TextEditingController())
            ..text = state.location.phone;
          _commentController = (_commentController ?? TextEditingController())
            ..text = state.location.comment;
          _projectNumberController = (_projectNumberController ??
              TextEditingController())
            ..text = (state.location.projectNumber ?? '').toString();
        }
      },
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: BlocBuilder(
            bloc: bloc,
            builder: (context, LocationCreateUpdateState state) {
              return Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Name', filled: true),
                    controller: _nameController,
                    onChanged: (text) => bloc.dispatch(NameChanged(text: text)),
                  ),
                  Space(),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Address', filled: true),
                    controller: _addressController,
                    onChanged: (text) =>
                        bloc.dispatch(AddressChanged(text: text)),
                  ),
                  Space(),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Phone', filled: true),
                    controller: _phoneController,
                    onChanged: (text) =>
                        bloc.dispatch(PhoneChanged(text: text)),
                    keyboardType: TextInputType.phone,
                  ),
                  Space(),
                  TextField(
                    decoration: InputDecoration(
                        labelText: 'ProjectNumber', filled: true),
                    controller: _projectNumberController,
                    onChanged: (text) =>
                        bloc.dispatch(ProjectNumberChanged(text: text)),
                  ),
                  Space(),
                  TextField(
                    maxLines: 10,
                    decoration:
                        InputDecoration(labelText: 'Comment', filled: true),
                    controller: _commentController,
                    onChanged: (text) =>
                        bloc.dispatch(CommentChanged(text: text)),
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
          ),
        ),
      ),
    );
  }
}
