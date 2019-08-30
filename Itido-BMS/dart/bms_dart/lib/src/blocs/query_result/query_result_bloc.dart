import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/query_result.dart';
import './bloc.dart';
export 'package:bms_dart/query_result.dart';

class QueryResultBloc extends Bloc<QueryResultEvent, QueryResultState> {
  @override
  QueryResultState get initialState => InitialQueryResultState();

  @override
  Stream<QueryResultState> mapEventToState(
    QueryResultEvent event,
  ) async* {
    print('QueryResultBloc !!!! ');
    print(event);

    if (event is QuerySuccessfulEvent)
      yield SuccessfulQueryState(event.translations);
    if (event is QueryUnauthorizedEvent)
      yield UnauthorizedQueryState(event.translations);
    if (event is QueryErrorEvent) yield ErrorQueryState(event.translations);
  }

  void dispatchFromQueryResult(
    QueryResult result,
    QueryResultTranslations translations,
  ) {
    if (result is Ok) dispatch(QuerySuccessfulEvent(translations));
    if (result is NoContent) dispatch(QuerySuccessfulEvent(translations));
    if (result is Unauthorized) dispatch(QueryUnauthorizedEvent(translations));
    if (result is Error) dispatch(QueryErrorEvent(translations));
  }
}
