import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/comment.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class CommentCreateUpdateState extends Equatable {
  final Comment comment;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  CommentCreateUpdateState({
    @required this.comment,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          comment.toMap(),
          isValid,
          createUpdateStatePhase,
        ]);

  factory CommentCreateUpdateState.createOrCopy(
    dynamic old, {
    Comment comment,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Comment) changes,
  }) {
    CommentCreateUpdateState previous;
    if (old is CommentCreateUpdateState) previous = old;

    var _comment = comment ?? previous?.comment ?? Comment();
    var _isValid = isValid ?? previous?.isValid ?? true;
    var _createUpdateStatePhase = createUpdateStatePhase ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;

    if (changes != null) changes(_comment);

    return CommentCreateUpdateState(
      comment: _comment,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'CommentCreateUpdateState { comment: ${comment.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
