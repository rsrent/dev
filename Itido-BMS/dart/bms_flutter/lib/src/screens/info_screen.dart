import 'package:flutter/material.dart';

import '../../translations.dart';

class ErrorLoadingScreen extends StatelessWidget {
  final String info;
  final Function onRetre;

  const ErrorLoadingScreen({Key key, @required this.info, this.onRetre})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        iconTheme: IconThemeData(
          color: Colors.black,
        ),
        elevation: 0,
        backgroundColor: Colors.transparent,
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Padding(
              padding: const EdgeInsets.only(bottom: 24),
              child: Text(
                info,
                style: TextStyle(fontSize: 20),
              ),
            ),
            RaisedButton(
              child: Text(Translations.of(context).optionTryAgain),
              onPressed: onRetre,
            ),
          ],
        ),
      ),
    );
  }
}
