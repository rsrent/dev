import 'package:flutter/material.dart';
import 'package:dart_packages/tuple.dart';

Future<Tuple2<String, String>> postCreateDialog(BuildContext context) async {
  var _titleFieldController = TextEditingController();
  var _bodyFieldController = TextEditingController();
  var result = await showDialog<Tuple2<String, String>>(
    context: context,
    builder: (context) {
      return AlertDialog(
        title: Text('Ny post'),
        content: Column(
          children: <Widget>[
            TextField(
              controller: _titleFieldController,
              decoration: InputDecoration(hintText: "Postens titel..."),
              maxLines: 2,
            ),
            TextField(
              controller: _bodyFieldController,
              decoration: InputDecoration(hintText: "Postens indhold..."),
              maxLines: 5,
            ),
          ],
        ),
        actions: <Widget>[
          FlatButton(
            child: Text('CANCEL'),
            onPressed: () {
              Navigator.of(context).pop(null);
            },
          ),
          FlatButton(
            child: Text('OPRET'),
            onPressed: () {
              Navigator.of(context).pop(Tuple2(
                  _titleFieldController.text, _bodyFieldController.text));
            },
          ),
        ],
      );
    },
  );
  if (result is Tuple2<String, String>)
    return result;
  else
    return null;
}
