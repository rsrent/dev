import 'package:dart_packages/streamer.dart';
import 'package:flutter/material.dart';

class DecoratedDropDownButton<T> extends StatelessWidget {
  final List<T> allValues;
  final T selectedValue;
  final Function(T) onChanged;
  final String Function(T) valueToString;
  final String labelText;
  final String hintText;

  const DecoratedDropDownButton({
    Key key,
    @required this.allValues,
    @required this.selectedValue,
    @required this.onChanged,
    @required this.valueToString,
    this.labelText,
    this.hintText,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return DropdownButtonHideUnderline(
      child: InputDecorator(
        decoration: InputDecoration(
          labelText: labelText,
          contentPadding: EdgeInsets.zero,
        ),
        child: allValues != null
            ? DropdownButton<T>(
                isDense: false,
                isExpanded: true,
                value: selectedValue,
                onChanged: onChanged,
                items: [
                  DropdownMenuItem<T>(
                    value: null,
                    child: Text(
                      hintText,
                      style: TextStyle(color: Colors.grey),
                    ),
                  )
                ]..addAll(allValues.map<DropdownMenuItem<T>>((T value) {
                    return DropdownMenuItem<T>(
                      value: value,
                      child: Text(
                        valueToString(value),
                      ),
                    );
                  }).toList()),
              )
            : Center(child: CircularProgressIndicator()),
      ),
    );
  }
}
