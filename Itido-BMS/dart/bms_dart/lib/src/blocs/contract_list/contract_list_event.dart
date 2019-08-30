import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class ContractListEvent extends Equatable {
  ContractListEvent([List props = const []]) : super(props);
}

class ContractListFetchOfUser extends ContractListEvent {
  final int userId;

  ContractListFetchOfUser({@required this.userId}) : super([userId]);
  @override
  String toString() => 'ContractList';
}
