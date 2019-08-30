import 'package:flutter/material.dart';
import 'package:dart_packages/streamer.dart';

class StreamerTextField<T> extends StatefulWidget {
  final String labelText;
  final String hintText;
  final int maxLines;
  final TextInputType keyboardType;
  final Streamer<T> streamer;
  final T Function(String) valueFromString;
  final String Function(T) stringFromValue;
  StreamerTextField({
    this.labelText,
    this.hintText,
    this.maxLines: 1,
    this.streamer,
    this.keyboardType: TextInputType.text,
    this.valueFromString,
    this.stringFromValue,
  });
  _StreamerTextFieldState createState() => _StreamerTextFieldState<T>();
}

class _StreamerTextFieldState<T> extends State<StreamerTextField<T>> {
  TextEditingController controller;

  bool initialized = false;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    Future.delayed(Duration.zero, () {
      String startValue;

      if (widget.stringFromValue != null) {
        startValue = widget.stringFromValue(widget.streamer.value);
      } else {
        startValue = widget.streamer.value as String;
      }

      controller =
          TextEditingController.fromValue(TextEditingValue(text: startValue));

      setState(() {
        initialized = true;
      });
    });
  }

  @override
  Widget build(BuildContext context) {
    if (!initialized) return Container();
    return StreamBuilder(
      stream: widget.streamer.stream,
      builder: (BuildContext context, AsyncSnapshot<T> snapshot) {
        if (snapshot.data == '') {
          controller.clear();
        }
        return TextField(
          onChanged: (s) {
            if (widget.valueFromString != null) {
              var modified = widget.valueFromString(s);
              widget.streamer.update(modified);
            } else {
              widget.streamer.update(s as T);
            }
          },
          controller: controller,
          decoration: InputDecoration(
              labelText: widget.labelText,
              hintText: widget.hintText,
              errorText: snapshot.hasError ? snapshot.error.toString() : null,
              suffix: IconButton(
                icon: Icon(
                  Icons.clear,
                ),
                onPressed: () {
                  controller.clear();
                  //widget.streamer.update();
                },
              )),
          maxLines: widget.maxLines,
          keyboardType: widget.keyboardType,
        );
      },
    );
  }
}
