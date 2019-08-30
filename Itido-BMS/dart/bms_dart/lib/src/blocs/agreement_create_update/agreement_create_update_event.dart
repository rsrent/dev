import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AgreementCreateUpdateEvent extends Equatable {
  AgreementCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends AgreementCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class PrepareUpdate extends AgreementCreateUpdateEvent {
  final Agreement agreement;
  PrepareUpdate({@required this.agreement}) : super([agreement]);
  @override
  String toString() => 'PrepareUpdate { agreement: $agreement }';
}

class CreateRequested extends AgreementCreateUpdateEvent {
  @override
  String toString() => 'CreateRequested';
}

class UpdateRequested extends AgreementCreateUpdateEvent {
  @override
  String toString() => 'UpdateRequested';
}
