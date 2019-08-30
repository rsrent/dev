import 'package:flutter/material.dart';

void showSnackText(BuildContext context, String text) => Scaffold.of(context)
  ..removeCurrentSnackBar()
  ..showSnackBar(SnackBar(
    content: Text(text),
  ));
