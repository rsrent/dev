import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/work.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class WorkCreateUpdateState extends Equatable {
  final Work work;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  WorkCreateUpdateState({
    @required this.work,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          work.toMap(),
          isCreate,
          isValid,
          createUpdateStatePhase,
        ]);

  factory WorkCreateUpdateState.createOrCopy(
    dynamic old, {
    Work work,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Work) changes,
  }) {
    WorkCreateUpdateState previous;
    if (old is WorkCreateUpdateState) previous = old;

    var _work = work ?? previous?.work ?? Work();
    var _isCreate = isCreate ?? previous?.isCreate ?? true;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = createUpdateStatePhase ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;

    if (changes != null) changes(_work);

    return WorkCreateUpdateState(
      work: _work,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'WorkCreateUpdateState { work: ${work.toMap()}, isCreate: $isCreate, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
