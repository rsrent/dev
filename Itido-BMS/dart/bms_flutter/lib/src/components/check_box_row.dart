import 'package:flutter/material.dart';

class CheckBoxRow extends StatelessWidget {
  final String title;
  final bool value;
  final Function(bool) onChanged;

  const CheckBoxRow({
    Key key,
    @required this.title,
    @required this.value,
    @required this.onChanged,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: <Widget>[
        Text(
          title,
          style: TextStyle(color: Colors.grey[600], fontSize: 16),
        ),
        Checkbox(
          value: value ?? false,
          onChanged: onChanged,
        ),
      ],
    );
  }
}
