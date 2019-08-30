import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class PostCreateUpdateState extends Equatable {
  PostCreateUpdateState([List props = const []]) : super(props);
}

class Initial extends PostCreateUpdateState {
  @override
  String toString() => 'Initial';
}

class PreparingCreate extends PostCreateUpdateState {
  @override
  String toString() => 'PreparingCreate';
}

class Loading extends PostCreateUpdateState {
  @override
  String toString() => 'Loading';
}

class CreateFailure extends PostCreateUpdateState {
  @override
  String toString() => 'CreateFailure';
}

class CreateSuccessful extends PostCreateUpdateState {
  @override
  String toString() => 'CreateSuccessful';
}
