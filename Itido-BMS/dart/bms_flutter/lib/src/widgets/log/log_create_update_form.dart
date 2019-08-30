import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_flutter/components.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class LogCreateUpdateForm extends StatefulWidget {
  @override
  _LogCreateUpdateFormState createState() => _LogCreateUpdateFormState();
}

class _LogCreateUpdateFormState extends State<LogCreateUpdateForm> {
  TextEditingController _titleController;
  TextEditingController _logController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<LogCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, LogCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _titleController = (_titleController ?? TextEditingController())
            ..text = state.log.title;
          _logController = (_logController ?? TextEditingController())
            ..text = state.log.log;
        }
      },
      child: SingleChildScrollView(
        child: Padding(
            padding: const EdgeInsets.all(24.0),
            child: BlocBuilder(
              bloc: bloc,
              builder: (context, LogCreateUpdateState state) {
                return Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    TextField(
                      decoration:
                          InputDecoration(labelText: 'Titel', filled: true),
                      controller: _titleController,
                      onChanged: (text) => bloc.dispatch(
                        TitleChanged(text: text),
                      ),
                    ),
                    Divider(color: Colors.transparent),
                    TextField(
                      decoration:
                          InputDecoration(labelText: 'Log', filled: true),
                      maxLines: 12,
                      controller: _logController,
                      onChanged: (text) => bloc.dispatch(
                        BodyChanged(text: text),
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
