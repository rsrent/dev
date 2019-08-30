import 'package:bms_dart/query_result.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class QueryResultEvent extends Equatable {
  QueryResultEvent([List props = const <dynamic>[]]) : super(props);
}

class QuerySuccessfulEvent extends QueryResultEvent {
  final QueryResultTranslations translations;
  QuerySuccessfulEvent(this.translations) : super([translations]);
}

class QueryUnauthorizedEvent extends QueryResultEvent {
  final QueryResultTranslations translations;
  QueryUnauthorizedEvent(this.translations) : super([translations]);
}

class QueryErrorEvent extends QueryResultEvent {
  final QueryResultTranslations translations;
  QueryErrorEvent(this.translations) : super([translations]);
}
