import 'package:dart_packages/streamer.dart';
import 'package:flutter/material.dart';

class StreamerDropDownButton<T> extends StatelessWidget {
  final Streamer<List<T>> allValuesStreamer;
  final Streamer<T> selectedValueStreamer;
  final String Function(T) valueToString;
  final String labelText;
  final String hintText;

  const StreamerDropDownButton({
    Key key,
    @required this.allValuesStreamer,
    @required this.selectedValueStreamer,
    @required this.valueToString,
    this.labelText,
    this.hintText,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return DropdownButtonHideUnderline(
      child: StreamBuilder<List<T>>(
        stream: allValuesStreamer.stream,
        builder: (context, AsyncSnapshot<List<T>> allValuesSnapshot) {
          var allValues = allValuesSnapshot.data;

          return StreamBuilder<T>(
            stream: selectedValueStreamer.stream,
            builder: (context, AsyncSnapshot<T> selectedValueSnapshot) {
              var selectedValue = selectedValueSnapshot.data;
              return InputDecorator(
                decoration: InputDecoration(
                  labelText: labelText,
                  contentPadding: EdgeInsets.zero,
                ),
                child: allValues != null
                    ? DropdownButton<T>(
                        value: selectedValue,
                        onChanged: selectedValueStreamer.update,
                        items: [
                          DropdownMenuItem<T>(
                            value: null,
                            child: Text(hintText),
                          )
                        ]..addAll(allValues.map<DropdownMenuItem<T>>((T value) {
                            return DropdownMenuItem<T>(
                              value: value,
                              child: Text(valueToString(value)),
                            );
                          }).toList()),
                      )
                    : Center(child: CircularProgressIndicator()),
              );
            },
          );
        },
      ),
    );
  }
}
