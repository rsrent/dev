import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:flutter/material.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class CommentCreateUpdateBloc
    extends Bloc<CommentCreateUpdateEvent, CommentCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final CommentRepository _commentRepository =
      repositoryProvider.commentRepository();

  final int commentId;

  CommentCreateUpdateBloc(this.commentId);

  @override
  CommentCreateUpdateState get initialState =>
      CommentCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<CommentCreateUpdateState> mapEventToState(
    CommentCreateUpdateEvent event,
  ) async* {
    if (event is PrepareUpdate) {
      var comment = await _commentRepository.fetch(commentId);
      yield CommentCreateUpdateState.createOrCopy(null,
          comment: comment,
          createUpdateStatePhase: CreateUpdateStatePhase.Initial);
      yield CommentCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is TitleChanged)
      yield CommentCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress,
          changes: (comment) => comment.title = event.text);
    if (event is BodyChanged)
      yield CommentCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress,
          changes: (comment) => comment.body = event.text);

    if (event is Commit) {
      var newState = CommentCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result =
          await _commentRepository.update(commentId, newState.comment);
      if (result) {
        yield CommentCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield CommentCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
