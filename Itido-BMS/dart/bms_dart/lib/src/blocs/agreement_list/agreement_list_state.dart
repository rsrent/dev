/*
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import 'package:bms_dart/models.dart';

@immutable
abstract class AgreementListState extends Equatable {
  AgreementListState([List props = const []]) : super(props);
}

class AgreementListLoading extends AgreementListState {
  @override
  String toString() => 'Loading';
}

class AgreementListLoaded extends AgreementListState {
  final List<Agreement> agreements;

  AgreementListLoaded({@required this.agreements}) : super([agreements]);

  @override
  String toString() => 'Loaded { agreements: ${agreements?.length} }';
}

class AgreementListFailure extends AgreementListState {
  @override
  String toString() => 'Failure';
}
*/
