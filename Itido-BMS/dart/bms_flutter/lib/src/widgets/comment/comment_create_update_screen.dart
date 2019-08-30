import 'package:bms_dart/comment_create_update_bloc.dart';
import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/widgets/comment/comment_create_update_form.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class CommentCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Comment comment,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => CommentCreateUpdateScreen(
        commentToUpdate: comment,
      ),
    ));
  }

  final Comment commentToUpdate;
  final bool isCreate;

  const CommentCreateUpdateScreen({Key key, this.commentToUpdate})
      : isCreate = commentToUpdate == null,
        super(key: key);

  @override
  _CommentCreateUpdateScreenState createState() =>
      _CommentCreateUpdateScreenState();
}

class _CommentCreateUpdateScreenState extends State<CommentCreateUpdateScreen> {
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
                ? Translations.of(context).titleCreateComment
                : Translations.of(context).titleUpdateComment,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            return CommentCreateUpdateBloc(this.widget.commentToUpdate.id)
              ..dispatch(PrepareUpdate(comment: this.widget.commentToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc = BlocProvider.of<CommentCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, CommentCreateUpdateState state) {
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
                child: CommentCreateUpdateForm(),
              );
            },
          ),
        ),
      ),
    );
  }
}
