import 'package:dart_packages/streamer.dart';
import 'package:flutter/material.dart';

class StreamerTextField extends StatelessWidget {
  final Streamer<String> streamer;
  final String labelText;
  final String hintText;
  final bool obscureText;
  final TextEditingController controller;
  final int maxLines;

  const StreamerTextField({
    Key key,
    @required this.streamer,
    this.labelText = '',
    this.hintText = '',
    this.obscureText = false,
    this.controller,
    this.maxLines,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: this.streamer.stream,
      builder: (BuildContext context, AsyncSnapshot snapshot) {
        return TextField(
          controller: controller,
          onChanged: this.streamer.update,
          decoration: InputDecoration(
            labelText: this.labelText,
            hintText: this.hintText,
            errorText: snapshot.error,
            filled: false,
          ),
          obscureText: this.obscureText,
          maxLines: this.maxLines,
        );
      },
    );
  }
}
