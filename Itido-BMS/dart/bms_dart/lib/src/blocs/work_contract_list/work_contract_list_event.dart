import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class WorkContractListEvent extends Equatable {
  WorkContractListEvent([List props = const []]) : super(props);
}

class WorkContractListFetchOfUser extends WorkContractListEvent {
  final int userId;
  WorkContractListFetchOfUser({@required this.userId}) : super([userId]);
  @override
  String toString() => 'WorkContractListFetchOfUser';
}

class WorkContractListFetchOfProjectItem extends WorkContractListEvent {
  final int proejctItemId;
  WorkContractListFetchOfProjectItem({@required this.proejctItemId})
      : super([proejctItemId]);
  @override
  String toString() => 'WorkContractListFetchOfProjectItem';
}

class WorkContractListFetched extends WorkContractListEvent {
  final List<WorkContract> items;

  WorkContractListFetched({@required this.items}) : super([items]);
  @override
  String toString() => 'WorkContractListFetched { items: ${items.length} }';
}

class WorkContractListAddUserContract extends WorkContractListEvent {
  final User user;
  final Contract contract;
  final WorkContract workContract;
  WorkContractListAddUserContract(
      {@required this.user,
      @required this.contract,
      @required this.workContract})
      : super([
          user,
          contract,
          workContract,
        ]);
  @override
  String toString() =>
      'WorkContractListAddUserContract { user: $user, contract: $contract, workContract: $workContract }';
}

class WorkContractListRemoveUser extends WorkContractListEvent {
  final int workContractId;
  WorkContractListRemoveUser({@required this.workContractId})
      : super([workContractId]);
  @override
  String toString() =>
      'WorkContractListRemoveUser { workContractId: $workContractId }';
}
