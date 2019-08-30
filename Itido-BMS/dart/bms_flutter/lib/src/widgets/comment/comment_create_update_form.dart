import 'package:bms_dart/comment_create_update_bloc.dart';
import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_flutter/components.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class CommentCreateUpdateForm extends StatefulWidget {
  @override
  _CommentCreateUpdateFormState createState() =>
      _CommentCreateUpdateFormState();
}

class _CommentCreateUpdateFormState extends State<CommentCreateUpdateForm> {
  TextEditingController _titleController;
  TextEditingController _commentController;

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<CommentCreateUpdateBloc>(context);

    return BlocListener(
      bloc: bloc,
      listener: (context, CommentCreateUpdateState state) {
        if (state.createUpdateStatePhase == CreateUpdateStatePhase.Initial) {
          _titleController = (_titleController ?? TextEditingController())
            ..text = state.comment.title;
          _commentController = (_commentController ?? TextEditingController())
            ..text = state.comment.body;
        }
      },
      child: SingleChildScrollView(
        child: Padding(
            padding: const EdgeInsets.all(24.0),
            child: BlocBuilder(
              bloc: bloc,
              builder: (context, CommentCreateUpdateState state) {
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
                          InputDecoration(labelText: 'Comment', filled: true),
                      maxLines: 12,
                      controller: _commentController,
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
