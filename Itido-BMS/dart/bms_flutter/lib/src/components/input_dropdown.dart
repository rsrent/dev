import 'package:flutter/material.dart';

class ValueInputDropdown extends StatelessWidget {
  const ValueInputDropdown({
    Key key,
    this.labelText,
    this.valueText,
    this.valueStyle,
    this.onPressed,
  }) : super(key: key);

  final String labelText;
  final String valueText;
  final TextStyle valueStyle;
  final VoidCallback onPressed;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: InkWell(
        onTap: onPressed,
        child: InputDecorator(
          decoration: InputDecoration(
            labelText: labelText,
            contentPadding: EdgeInsets.zero,
          ),
          isEmpty: valueText == null,
          baseStyle: valueStyle,
          child: Padding(
            padding: const EdgeInsets.symmetric(vertical: 12),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              mainAxisSize: MainAxisSize.min,
              children: <Widget>[
                Expanded(child: Text(valueText ?? '', style: valueStyle)),
                Icon(
                  Icons.arrow_drop_down,
                  color: Theme.of(context).brightness == Brightness.light
                      ? Colors.grey.shade700
                      : Colors.white70,
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class ChildInputDropdown extends StatelessWidget {
  const ChildInputDropdown({
    Key key,
    this.labelText,
    this.child,
    this.isEmpty,
    this.onPressed,
  }) : super(key: key);

  final String labelText;
  final Widget child;
  final bool isEmpty;
  final VoidCallback onPressed;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: InkWell(
        onTap: onPressed,
        child: InputDecorator(
          decoration: InputDecoration(
            labelText: labelText,
            contentPadding: EdgeInsets.zero,
          ),
          isEmpty: isEmpty,
          child: child,
        ),
      ),
    );
  }
}
