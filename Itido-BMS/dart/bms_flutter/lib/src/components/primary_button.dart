import 'package:flutter/material.dart';

class PrimaryButton extends StatelessWidget {
  final Function onPressed;
  final String text;
  final bool disabled;

  const PrimaryButton({
    Key key,
    @required this.onPressed,
    @required this.text,
    this.disabled = false,
  }) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Center(
      child: RaisedButton(
        onPressed: !disabled ? onPressed : null,
        child: Text(text),
      ),
    );
  }
}
