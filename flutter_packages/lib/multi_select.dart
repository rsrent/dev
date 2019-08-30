import 'package:flutter/material.dart';
import 'custom_check_box.dart';
import 'package:dart_packages/streamer.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_packages/multi_stream_builder.dart';

class MultiSelectController<T> {
  Streamer<bool> active;
  Streamer<List<T>> selected;

  MultiSelectController() {
    active = Streamer(seedValue: false);
    selected = Streamer(seedValue: List<T>());
  }

  void activate() {
    selected.update([]);
    active.update(true);
  }

  void deactivate() {
    selected.update([]);
    active.update(false);
  }
}

typedef bool IsEqual<T>(T value, T selected);

class MultiSelectTile<T> extends StatelessWidget {
  final WidgetBuilder builder;
  final MultiSelectController<T> controller;
  final value;
  final IsEqual<T> isEqual;

  MultiSelectTile({
    @required this.builder,
    @required this.isEqual,
    @required this.controller,
    @required this.value,
  });

  @override
  Widget build(BuildContext context) {
    return Row(
      children: <Widget>[
        StreamBuilderOf2(
          stream1: controller.active.stream,
          stream2: controller.selected.stream,
          builder: (
            BuildContext context,
            AsyncSnapshot<bool> snapshot1,
            AsyncSnapshot<List<T>> snapshot2,
          ) {
            if (!snapshot1.hasData || !snapshot1.data || !snapshot2.hasData) {
              return Container();
            }
            return Container(
              padding: EdgeInsets.all(0.5),
              margin: EdgeInsets.fromLTRB(6, 0, 2, 0),
              child: CustomCheckBox(
                active: snapshot2.data.any((s) => isEqual(s, value)),
                update: (a) {
                  var values = controller.selected.value;
                  if (a)
                    values.add(value);
                  else
                    values.removeWhere((s) => isEqual(s, value));
                  controller.selected.update(values);
                },
              ),
            );
          },
        ),
        Expanded(child: builder(context)),
      ],
    );
  }
}
