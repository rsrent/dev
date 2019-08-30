import 'package:bms_dart/models.dart';
import 'package:dart_packages/tuple.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/work.dart';
import '../create_update_state_phase.dart';

@immutable
class WorkContractCreateUpdateState extends Equatable {
  final bool isCreate;
  final bool evenUnevenWeeks;
  final CreateUpdateStatePhase createUpdateStatePhase;
  final List<Tuple2<Holiday, bool>> holidays;
  final WorkContract workContract;

  WorkContractCreateUpdateState({
    @required this.isCreate,
    @required this.evenUnevenWeeks,
    @required this.createUpdateStatePhase,
    @required this.holidays,
    @required this.workContract,
  }) : super([
          isCreate,
          evenUnevenWeeks,
          createUpdateStatePhase,
          holidays.join(', '),
          workContract.toMap(),
        ]);

  factory WorkContractCreateUpdateState.createOrCopy(
    dynamic old, {
    WorkContract workContract,
    bool isCreate,
    bool evenUnevenWeeks,
    CreateUpdateStatePhase createUpdateStatePhase,
    List<Tuple2<Holiday, bool>> holidays,
    void Function(WorkContract, List<Tuple2<Holiday, bool>> holidays) changes,
  }) {
    WorkContractCreateUpdateState previous;
    if (old is WorkContractCreateUpdateState) previous = old;

    var _workContract =
        workContract ?? previous?.workContract ?? WorkContract();
    var _isCreate = isCreate ?? previous?.isCreate ?? true;
    var _evenUnevenWeeks = evenUnevenWeeks ?? previous?.evenUnevenWeeks ?? true;
    var _holidays = holidays ?? previous?.holidays ?? [];
    var _createUpdateStatePhase = createUpdateStatePhase ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;

    if (changes != null) changes(_workContract, _holidays);

    return WorkContractCreateUpdateState(
      workContract: _workContract,
      isCreate: _isCreate,
      evenUnevenWeeks: _evenUnevenWeeks,
      createUpdateStatePhase: _createUpdateStatePhase,
      holidays: _holidays,
    );
  }

  @override
  String toString() => 'WorkContractCreateUpdateState';
}
