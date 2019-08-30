import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AgreementListEvent extends Equatable {
  AgreementListEvent([List props = const []]) : super(props);
}

class AgreementListFetch extends AgreementListEvent {
  @override
  String toString() => 'AgreementListFetch';
}
