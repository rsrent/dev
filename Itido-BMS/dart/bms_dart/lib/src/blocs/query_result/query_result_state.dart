import 'package:bms_dart/query_result.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class QueryResultState extends Equatable {
  QueryResultState([List props = const <dynamic>[]]) : super(props);
}

class InitialQueryResultState extends QueryResultState {
  InitialQueryResultState() : super([DateTime.now()]);
}

class SuccessfulQueryState extends QueryResultState {
  final QueryResultTranslations translations;
  SuccessfulQueryState(this.translations) : super([DateTime.now()]);
}

class UnauthorizedQueryState extends QueryResultState {
  final QueryResultTranslations translations;
  UnauthorizedQueryState(this.translations) : super([DateTime.now()]);
}

class ErrorQueryState extends QueryResultState {
  final QueryResultTranslations translations;
  ErrorQueryState(this.translations) : super([DateTime.now()]);
}
