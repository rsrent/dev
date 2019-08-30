import 'package:flutter/foundation.dart';

abstract class QueryResult<T> {
  final bool successful;
  final bool hasContent;
  T get value;
  QueryResult(this.successful, this.hasContent);
}

class Ok<T> extends QueryResult<T> {
  T get value => _value;
  final T _value;
  Ok(this._value) : super(true, true);
}

class NoContent<T> extends QueryResult<T> {
  T get value => null;
  NoContent() : super(true, false);
}

class Unauthorized<T> extends QueryResult<T> {
  T get value => null;
  Unauthorized() : super(false, false);
}

class Error<T> extends QueryResult<T> {
  T get value => null;
  Error() : super(false, false);
}

class QueryResultTranslations {
  final String successMessage;
  final String errorMessage;
  final String successTitle;
  final String errorTitle;

  QueryResultTranslations({
    this.successMessage,
    this.errorMessage,
    @required this.successTitle,
    this.errorTitle,
  });
}
