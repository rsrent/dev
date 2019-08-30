import 'package:flutter/material.dart';

Future<void> snackbarController(
  BuildContext context,
  Future<bool> success,
  String successMessage, {
  String errorMessage: 'Fejl',
  Function successFunction,
  Function errorFunction,
}) async {
  if (await success) {
    Scaffold.of(context).showSnackBar(new SnackBar(
      content: new Text(successMessage),
    ));
    await Future.delayed(Duration(milliseconds: 500));
    if (successFunction != null) successFunction();
  } else {
    Scaffold.of(context).showSnackBar(new SnackBar(
      content: new Text(errorMessage),
    ));
    await Future.delayed(Duration(milliseconds: 500));
    if (errorFunction != null) errorFunction();
  }
}

Future<void> showSnackbar<T>({
  BuildContext context,
  Future<T> future,
  T value,
  String Function(T) onCompleteShowMessage,
}) async {
  T res;
  if (future != null)
    res = await future;
  else
    res = value;
  var message = onCompleteShowMessage(res);
  Scaffold.of(context).showSnackBar(new SnackBar(
    content: new Text(message),
  ));
}
