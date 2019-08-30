import 'package:flutter/material.dart';
import 'loader.dart';
import 'multi_stream_builder.dart';

typedef BodyBuilder = Widget Function();
typedef BodyBuilderFromValue<T> = Widget Function(T value);

typedef BodyBuilderFromValueOf2<T1, T2> = Widget Function(T1 value1, T2 value2);
typedef BodyBuilderFromValueOf3<T1, T2, T3> = Widget Function(
    T1 value1, T2 value2, T3 value3);
typedef BodyBuilderFromValueOf4<T1, T2, T3, T4> = Widget Function(
    T1 value1, T2 value2, T3 value3, T4 value4);
typedef BodyBuilderFromValueOf5<T1, T2, T3, T4, T5> = Widget Function(
    T1 value1, T2 value2, T3 value3, T4 value4, T5 value5);
typedef ValueToString<T> = String Function(T value);

typedef ValueToStringOf2<T1, T2> = String Function(T1 value1, T2 value2);
typedef ValueToStringOf3<T1, T2, T3> = String Function(
    T1 value1, T2 value2, T3 value3);
typedef ValueToStringOf4<T1, T2, T3, T4> = String Function(
    T1 value1, T2 value2, T3 value3, T4 value4);
typedef ValueToStringOf5<T1, T2, T3, T4, T5> = String Function(
    T1 value1, T2 value2, T3 value3, T4 value4, T5 value5);

class DualHeaderWithHint extends StatelessWidget {
  const DualHeaderWithHint({this.name, this.value, this.hint, this.showHint});

  final String name;
  final String value;
  final String hint;
  final bool showHint;

  Widget _crossFade(Widget first, Widget second, bool isExpanded) {
    return AnimatedCrossFade(
      firstChild: first,
      secondChild: second,
      firstCurve: const Interval(0.0, 0.6, curve: Curves.fastOutSlowIn),
      secondCurve: const Interval(0.4, 1.0, curve: Curves.fastOutSlowIn),
      sizeCurve: Curves.fastOutSlowIn,
      crossFadeState:
          isExpanded ? CrossFadeState.showSecond : CrossFadeState.showFirst,
      duration: const Duration(milliseconds: 200),
    );
  }

  @override
  Widget build(BuildContext context) {
    final ThemeData theme = Theme.of(context);
    final TextTheme textTheme = theme.textTheme;

    return Row(children: <Widget>[
      Expanded(
        flex: 2,
        child: Container(
          margin: const EdgeInsets.only(left: 24.0),
          child: FittedBox(
            fit: BoxFit.scaleDown,
            alignment: Alignment.centerLeft,
            child: Text(
              name,
              style: textTheme.body1.copyWith(fontSize: 15.0),
            ),
          ),
        ),
      ),
      Expanded(
          flex: 3,
          child: Container(
              margin: const EdgeInsets.only(left: 24.0),
              child: _crossFade(
                  Text(value,
                      style: textTheme.caption.copyWith(fontSize: 15.0)),
                  Text(hint, style: textTheme.caption.copyWith(fontSize: 15.0)),
                  showHint)))
    ]);
  }
}

class ExpandableItem<T> with Expandable {
  ExpandableItem({this.name, this.builder});

  final BodyBuilder builder;
  String name;

  ExpansionPanelHeaderBuilder get headerBuilder {
    return (BuildContext context, bool isExpanded) {
      return Row(
        children: <Widget>[
          Expanded(
            flex: 2,
            child: Container(
              margin: const EdgeInsets.only(left: 24.0),
              child: FittedBox(
                fit: BoxFit.scaleDown,
                alignment: Alignment.centerLeft,
                child: Text(
                  name,
                  style: Theme.of(context)
                      .textTheme
                      .body1
                      .copyWith(fontSize: 15.0),
                ),
              ),
            ),
          ),
        ],
      );
    };
  }

  Widget build() => Padding(
        padding: EdgeInsets.all(4),
        child: builder(),
      );
}

class ExpandableListForm extends StatefulWidget {
  final List<Expandable> expandableItems;
  ExpandableListForm(this.expandableItems);

  @override
  _ExpandableListFormState createState() => _ExpandableListFormState();
}

class _ExpandableListFormState extends State<ExpandableListForm> {
  List<Expandable> _expandableItems;

  @override
  void initState() {
    super.initState();
    _expandableItems = widget.expandableItems;
  }

  @override
  Widget build(BuildContext context) {
    return Center(
      child: ListView(
        children: [
          Container(
            margin:
                const EdgeInsets.only(left: 8, top: 8, right: 8, bottom: 100),
            child: ExpansionPanelList(
                expansionCallback: (int index, bool isExpanded) {
                  setState(() {
                    _expandableItems[index].isExpanded = !isExpanded;
                  });
                },
                children:
                    _expandableItems.map<ExpansionPanel>((Expandable item) {
                  return ExpansionPanel(
                      isExpanded: item.isExpanded,
                      headerBuilder: item.headerBuilder,
                      body: item.build());
                }).toList()),
          )
        ],
      ),
    );
  }
}

abstract class Expandable {
  bool isExpanded = false;
  Widget build();
  ExpansionPanelHeaderBuilder get headerBuilder;
}

class ExpandableItemFromStream<T> with Expandable {
  ExpandableItemFromStream(
      {this.name, this.stream, this.hint, this.builder, this.valueToString});

  final String name;
  final String hint;
  final BodyBuilderFromValue<T> builder;
  final ValueToString<T> valueToString;
  Stream<T> stream;

  ExpansionPanelHeaderBuilder get headerBuilder {
    return (BuildContext context, bool isExpanded) {
      return StreamBuilder(
        stream: stream,
        builder: (BuildContext context, AsyncSnapshot snapshot) {
          if (!snapshot.hasData) return Text(name);
          var value = snapshot.data;
          return DualHeaderWithHint(
            name: name,
            value:
                valueToString != null ? valueToString(value) : value.toString(),
            hint: hint,
            showHint: isExpanded,
          );
        },
      );
    };
  }

  Widget build() => StreamBuilder(
        stream: stream,
        builder: (BuildContext context, AsyncSnapshot snapshot) {
          return Loader(
            loaded: snapshot.hasData,
            widget: () {
              return Padding(
                padding: const EdgeInsets.all(8.0),
                child: builder(snapshot.data),
              );
            },
          );
        },
      );
}

class ExpandableItemFromStreamOf2<T1, T2> with Expandable {
  ExpandableItemFromStreamOf2(
      {this.name,
      this.stream1,
      this.stream2,
      this.hint,
      this.builder,
      this.valueToString});

  final String name;
  final String hint;
  final BodyBuilderFromValueOf2<T1, T2> builder;
  final ValueToStringOf2<T1, T2> valueToString;
  Stream<T1> stream1;
  Stream<T2> stream2;

  ExpansionPanelHeaderBuilder get headerBuilder {
    return (BuildContext context, bool isExpanded) {
      return StreamBuilderOf2(
        stream1: stream1,
        stream2: stream2,
        builder:
            (BuildContext context, AsyncSnapshot<T1> v1, AsyncSnapshot<T2> v2) {
          if (!v1.hasData || !v2.hasData) return Text(name);
          return DualHeaderWithHint(
            name: name,
            value: valueToString(v1.data, v2.data),
            hint: hint,
            showHint: isExpanded,
          );
        },
      );
    };
  }

  Widget build() => StreamBuilderOf2(
        stream1: stream1,
        stream2: stream2,
        builder:
            (BuildContext context, AsyncSnapshot<T1> v1, AsyncSnapshot<T2> v2) {
          return Loader(
            loaded: v1.hasData && v2.hasData,
            widget: () {
              return builder(v1.data, v2.data);
            },
          );
        },
      );
}

class ExpandableItemFromStreamOf3<T1, T2, T3> with Expandable {
  ExpandableItemFromStreamOf3(
      {this.name,
      this.stream1,
      this.stream2,
      this.stream3,
      this.hint,
      this.builder,
      this.valueToString});

  final String name;
  final String hint;
  final BodyBuilderFromValueOf3<T1, T2, T3> builder;
  final ValueToStringOf3<T1, T2, T3> valueToString;
  Stream<T1> stream1;
  Stream<T2> stream2;
  Stream<T3> stream3;

  ExpansionPanelHeaderBuilder get headerBuilder {
    return (BuildContext context, bool isExpanded) {
      return StreamBuilderOf3(
        stream1: stream1,
        stream2: stream2,
        stream3: stream3,
        builder: (BuildContext context, AsyncSnapshot<T1> v1,
            AsyncSnapshot<T2> v2, AsyncSnapshot<T3> v3) {
          if (!v1.hasData || !v2.hasData || !v3.hasData) return Text(name);
          return DualHeaderWithHint(
            name: name,
            value: valueToString(v1.data, v2.data, v3.data),
            hint: hint,
            showHint: isExpanded,
          );
        },
      );
    };
  }

  Widget build() => StreamBuilderOf3(
        stream1: stream1,
        stream2: stream2,
        stream3: stream3,
        builder: (BuildContext context, AsyncSnapshot<T1> v1,
            AsyncSnapshot<T2> v2, AsyncSnapshot<T3> v3) {
          return Loader(
            loaded: v1.hasData && v2.hasData && v3.hasData,
            widget: () {
              return builder(v1.data, v2.data, v3.data);
            },
          );
        },
      );
}

// class ExpandableItemFromStreamOf4<T1, T2, T3, T4> with Expandable {
//   ExpandableItemFromStreamOf4(
//       {this.name,
//       this.stream1,
//       this.stream2,
//       this.stream3,
//       this.stream4,
//       this.hint,
//       this.builder,
//       this.valueToString});

//   final String name;
//   final String hint;
//   final BodyBuilderFromValueOf4<T1, T2, T3, T4> builder;
//   final ValueToStringOf4<T1, T2, T3, T4> valueToString;
//   Stream<T1> stream1;
//   Stream<T2> stream2;
//   Stream<T3> stream3;
//   Stream<T4> stream4;

//   ExpansionPanelHeaderBuilder get headerBuilder {
//     return (BuildContext context, bool isExpanded) {
//       return StreamBuilderOf4(
//         stream1: stream1,
//         stream2: stream2,
//         stream3: stream3,
//         stream4: stream4,
//         builder:
//             (BuildContext context, bool loaded, T1 v1, T2 v2, T3 v3, T4 v4) {
//           if (!loaded) return Text(name);
//           return DualHeaderWithHint(
//             name: name,
//             value: valueToString(v1, v2, v3, v4),
//             hint: hint,
//             showHint: isExpanded,
//           );
//         },
//       );
//     };
//   }

//   Widget build() => StreamBuilderOf4(
//         stream1: stream1,
//         stream2: stream2,
//         stream3: stream3,
//         stream4: stream4,
//         builder:
//             (BuildContext context, bool loaded, T1 v1, T2 v2, T3 v3, T4 v4) {
//           return Loader(
//             loaded: loaded,
//             widget: () {
//               return builder(v1, v2, v3, v4);
//             },
//           );
//         },
//       );
// }

// class ExpandableItemFromStreamOf5<T1, T2, T3, T4, T5> with Expandable {
//   ExpandableItemFromStreamOf5(
//       {this.name,
//       this.stream1,
//       this.stream2,
//       this.stream3,
//       this.stream4,
//       this.stream5,
//       this.hint,
//       this.builder,
//       this.valueToString});

//   final String name;
//   final String hint;
//   final BodyBuilderFromValueOf5<T1, T2, T3, T4, T5> builder;
//   final ValueToStringOf5<T1, T2, T3, T4, T5> valueToString;
//   Stream<T1> stream1;
//   Stream<T2> stream2;
//   Stream<T3> stream3;
//   Stream<T4> stream4;
//   Stream<T5> stream5;

//   ExpansionPanelHeaderBuilder get headerBuilder {
//     return (BuildContext context, bool isExpanded) {
//       return StreamBuilderOf5(
//         stream1: stream1,
//         stream2: stream2,
//         stream3: stream3,
//         stream4: stream4,
//         stream5: stream5,
//         builder: (BuildContext context, bool loaded, T1 v1, T2 v2, T3 v3, T4 v4,
//             T5 v5) {
//           if (!loaded) return Text(name);
//           return DualHeaderWithHint(
//             name: name,
//             value: valueToString(v1, v2, v3, v4, v5),
//             hint: hint,
//             showHint: isExpanded,
//           );
//         },
//       );
//     };
//   }

//   Widget build() => StreamBuilderOf5(
//         stream1: stream1,
//         stream2: stream2,
//         stream3: stream3,
//         stream4: stream4,
//         stream5: stream5,
//         builder: (BuildContext context, bool loaded, T1 v1, T2 v2, T3 v3, T4 v4,
//             T5 v5) {
//           return Loader(
//             loaded: loaded,
//             widget: () {
//               return builder(v1, v2, v3, v4, v5);
//             },
//           );
//         },
//       );
// }
