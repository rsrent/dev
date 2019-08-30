import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class WorkListEvent extends Equatable {
  WorkListEvent([List props = const []]) : super(props);
}

class WorkListFetchAll extends WorkListEvent {
  @override
  String toString() => 'WorkListFetchAll';
}

class WorkListFetchOfSignedInUser extends WorkListEvent {
  @override
  String toString() => 'WorkListFetchOfSignedInUser';
}

class WorkListFetchOfUser extends WorkListEvent {
  final int userId;

  WorkListFetchOfUser({
    @required this.userId,
  }) : super([userId]);
  @override
  String toString() => 'WorkListFetchOfUser';
}

class WorkListFetchOfProjectItem extends WorkListEvent {
  final int projectItemId;

  WorkListFetchOfProjectItem({
    @required this.projectItemId,
  }) : super([projectItemId]);
  @override
  String toString() => 'WorkListFetchOfProjectItem';
}

class WorkListFetched extends WorkListEvent {
  final List<Work> items;

  WorkListFetched({@required this.items}) : super([items]);
  @override
  String toString() => 'WorkListFetched { items: ${items?.length} }';
}

class WorkListAddUserContract extends WorkListEvent {
  final User user;
  final Contract contract;
  final Work work;
  WorkListAddUserContract(
      {@required this.user, @required this.contract, @required this.work})
      : super([
          user,
          contract,
          work,
        ]);
  @override
  String toString() =>
      'WorkListAddUserContract { user: $user, contract: $contract, work: $work }';
}

class WorkListRegister extends WorkListEvent {
  final int workId;
  final int startTimeMins;
  final int endTimeMins;
  WorkListRegister({
    @required this.workId,
    @required this.startTimeMins,
    @required this.endTimeMins,
  }) : super([
          workId,
          startTimeMins,
          endTimeMins,
        ]);
  @override
  String toString() =>
      'WorkListRegister { workId: $workId, startTimeMins: $startTimeMins, endTimeMins: $endTimeMins }';
}

class WorkListReplaceUserContract extends WorkListEvent {
  final User user;
  final Contract contract;
  final Work work;
  WorkListReplaceUserContract(
      {@required this.user, @required this.contract, @required this.work})
      : super([
          user,
          contract,
          work,
        ]);
  @override
  String toString() =>
      'WorkListReplace { user: $user, contract: $contract, work: $work }';
}

class WorkListRemoveUser extends WorkListEvent {
  final int workId;
  WorkListRemoveUser({@required this.workId}) : super([workId]);
  @override
  String toString() => 'WorkListRemoveUser { workId: $workId }';
}

class WorkListRemoveReplacer extends WorkListEvent {
  final int workId;
  WorkListRemoveReplacer({@required this.workId}) : super([workId]);
  @override
  String toString() => 'WorkListRemoveReplacer { workId: $workId }';
}

class WorkListInviteUserContract extends WorkListEvent {
  final User user;
  final Contract contract;
  final Work work;
  WorkListInviteUserContract(
      {@required this.user, @required this.contract, @required this.work})
      : super([user, contract, work]);
  @override
  String toString() =>
      'WorkListInviteUserContract { user: $user, contract: $contract, work: $work }';
}

class WorkListInviteUserContractToReplace extends WorkListEvent {
  final User user;
  final Contract contract;
  final Work work;
  WorkListInviteUserContractToReplace(
      {@required this.user, @required this.contract, @required this.work})
      : super([user, contract, work]);
  @override
  String toString() =>
      'WorkListInviteUserContractToReplace { user: $user, contract: $contract, work: $work }';
}

class WorkListReplyToInvite extends WorkListEvent {
  final Work work;
  final bool answer;
  WorkListReplyToInvite({@required this.work, @required this.answer})
      : super([work, answer]);
  @override
  String toString() => 'WorkListReplyToInvite { work: $work, answer: $answer }';
}
