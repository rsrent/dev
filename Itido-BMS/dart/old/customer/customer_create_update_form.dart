import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/customer_create_update_bloc.dart';
import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/check_box_row.dart';
import 'package:bms_flutter/src/language/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class CustomerCreateUpdateForm extends StatefulWidget {
  final bool isCreate;

  const CustomerCreateUpdateForm({Key key, @required this.isCreate})
      : super(key: key);
  @override
  _CustomerCreateUpdateFormState createState() =>
      _CustomerCreateUpdateFormState();
}

class _CustomerCreateUpdateFormState extends State<CustomerCreateUpdateForm> {
  TextEditingController _nameController;
  TextEditingController _commentController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<CustomerCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, CustomerCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _nameController = (_nameController ?? TextEditingController())
            ..text = state.customer.name;
          _commentController = (_commentController ?? TextEditingController())
            ..text = state.customer.comment;
        }
      },
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: BlocBuilder(
            bloc: bloc,
            builder: (context, CustomerCreateUpdateState state) {
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
                  CheckBoxRow(
                    title: 'Has Standard folder',
                    value: state.customer.hasStandardFolders,
                    onChanged: (isTrue) => bloc.dispatch(
                      HasStandardFoldersChanged(isTrue: isTrue),
                    ),
                  ),
                  Space(),
                  CheckBoxRow(
                    title: 'Can read logs',
                    value: state.customer.canReadLogs,
                    onChanged: (isTrue) => bloc.dispatch(
                      CanReadLogsChanged(isTrue: isTrue),
                    ),
                  ),
                  TextField(
                    decoration:
                        InputDecoration(labelText: 'Comment', filled: true),
                    controller: _commentController,
                    onChanged: (text) =>
                        bloc.dispatch(CommentChanged(text: text)),
                  ),
                  Space(height: 40),
                  Center(
                    child: RaisedButton(
                      child: Text(
                        widget.isCreate
                            ? Translations.of(context).buttonCreate
                            : Translations.of(context).buttonUpdate,
                      ),
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
