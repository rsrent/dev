import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class PostCreateUpdateEvent extends Equatable {
  PostCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareCreate extends PostCreateUpdateEvent {
  @override
  String toString() => 'PrepareCreate';
}

class CreateRequested extends PostCreateUpdateEvent {
  @override
  String toString() => 'CreateRequested';
}

class AddCondition extends PostCreateUpdateEvent {
  final PostConditionType postConditionType;
  AddCondition({@required this.postConditionType});
  @override
  String toString() =>
      'AddCondition { postConditionType: ${this.postConditionType} }';
}

class RemoveCondition extends PostCreateUpdateEvent {
  final PostCondition postCondition;
  RemoveCondition({@required this.postCondition});
  @override
  String toString() =>
      'RemoveCondition { postCondition: ${this.postCondition} }';
}
