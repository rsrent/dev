import 'package:flutter/material.dart';

Future<String> showTextFieldDialog(
  BuildContext context, {
  @required String title,
  @required String hint,
  String startText,
  String cancelButtonText,
  String okButtonText,
}) async {
  var _textFieldController = TextEditingController(text: startText ?? '');
  var result = await showDialog<String>(
    context: context,
    builder: (context) {
      return AlertDialog(
        title: Text(title),
        content: TextField(
          controller: _textFieldController,
          decoration: InputDecoration(hintText: hint),
        ),
        actions: <Widget>[
          FlatButton(
            child: Text(cancelButtonText ?? 'TILBAGE'),
            onPressed: () {
              Navigator.of(context).pop(null);
            },
          ),
          FlatButton(
            child: Text(okButtonText ?? 'FÆRDIG'),
            onPressed: () {
              Navigator.of(context).pop(_textFieldController.text);
            },
          ),
        ],
      );
    },
  );
  if (result is String)
    return result;
  else
    return null;
}

Future<bool> showConfirmationDialog(
  BuildContext context, {
  @required String title,
  @required String body,
  String cancelButtonText,
  String okButtonText,
}) async {
  var result = await showDialog<bool>(
    context: context,
    builder: (context) {
      return AlertDialog(
        title: Text(title),
        content: Text(body),
        actions: <Widget>[
          FlatButton(
            child: Text(cancelButtonText ?? 'TILBAGE'),
            onPressed: () {
              Navigator.of(context).pop(false);
            },
          ),
          FlatButton(
            child: Text(okButtonText ?? 'FÆRDIG'),
            onPressed: () {
              Navigator.of(context).pop(true);
            },
          ),
        ],
      );
    },
  );
  if (result is bool)
    return result;
  else
    return false;
}
