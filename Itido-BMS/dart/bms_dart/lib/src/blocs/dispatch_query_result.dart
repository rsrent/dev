import 'package:bms_dart/query_result_bloc.dart';

abstract class DispatchQueryResult {
  QueryResultBloc getQueryResultBloc();

  void dispatchQueryResult(QueryResult queryResult,
      [QueryResultTranslations translations]) {
    var bloc = getQueryResultBloc();
    if (bloc != null) {
      bloc.dispatchFromQueryResult(queryResult, translations);
    }
  }
}
