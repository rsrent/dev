import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class AgreementCreateUpdateState extends Equatable {
  AgreementCreateUpdateState([List props = const []]) : super(props);
}

class Initial extends AgreementCreateUpdateState {
  @override
  String toString() => 'Initial';
}

class PreparingCreate extends AgreementCreateUpdateState {
  @override
  String toString() => 'PreparingCreate';
}

class PreparingUpdate extends AgreementCreateUpdateState {
  final Agreement agreement;
  PreparingUpdate({@required this.agreement}) : super([agreement]);
  @override
  String toString() => 'PreparingUpdate { agreement: $agreement }';
}

class Loading extends AgreementCreateUpdateState {
  @override
  String toString() => 'Loading';
}

class CreateFailure extends AgreementCreateUpdateState {
  @override
  String toString() => 'CreateFailure';
}

class UpdateFailure extends AgreementCreateUpdateState {
  @override
  String toString() => 'UpdateFailure';
}

class CreateSuccessful extends AgreementCreateUpdateState {
  final int id;

  CreateSuccessful({@required this.id}) : super([id]);

  @override
  String toString() => 'CreateSuccessful { id: $id }';
}

class UpdateSuccessful extends AgreementCreateUpdateState {
  @override
  String toString() => 'UpdateSuccessful';
}
