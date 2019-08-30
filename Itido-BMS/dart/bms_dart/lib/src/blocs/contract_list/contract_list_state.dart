/*

import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class ContractListState extends Equatable {
  ContractListState([List props = const []]) : super(props);
}

class ContractListLoading extends ContractListState {
  @override
  String toString() => 'ContractListLoading';
}

class ContractListLoaded extends ContractListState {
  final List<Contract> contracts;

  ContractListLoaded({@required this.contracts}) : super([contracts]);

  @override
  String toString() => 'ContractListLoaded { contracts: ${contracts?.length} }';
}

class ContractListFailure extends ContractListState {
  @override
  String toString() => 'ContractListFailure';
}

*/
