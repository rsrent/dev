import 'package:dart_packages/streamer.dart';
import 'package:flutter/material.dart';

class StreamerCheckBoxListTile extends StatelessWidget {
  final Streamer<bool> streamer;
  final String labelText;

  const StreamerCheckBoxListTile({
    Key key,
    @required this.streamer,
    this.labelText = '',
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: this.streamer.stream,
      builder: (BuildContext context, AsyncSnapshot snapshot) {
        return CheckboxListTile(
          title: Text(labelText),
          value: snapshot.data ?? false,
          onChanged: this.streamer.update,
        );
      },
    );
  }
}
