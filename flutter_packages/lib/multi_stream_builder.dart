import 'package:flutter/material.dart';
import 'dart:async';
import 'package:rxdart/rxdart.dart';
import 'loader.dart';

typedef AsyncWidgetBuilderOf1<T1> = Widget Function(
    BuildContext context, AsyncSnapshot<T1> val1);

class StreamBuilderOf1<T1> extends StatelessWidget {
  final Stream<T1> stream1;
  final AsyncWidgetBuilderOf1<T1> builder;
  final bool loader;
  final bool sliverLoader;

  StreamBuilderOf1({
    this.stream1,
    this.builder,
    this.loader = false,
    this.sliverLoader = false,
  });

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: stream1,
      builder: (BuildContext context, AsyncSnapshot<T1> snapshot) {
        if (loader || sliverLoader) {
          var loaderWidget = Loader(
            loaded: (snapshot.hasData),
            widget: () => builder(context, snapshot),
          );
          if (sliverLoader)
            return SliverToBoxAdapter(child: loaderWidget);
          else
            return loaderWidget;
        } else {
          return builder(context, snapshot);
        }
      },
    );
  }
}

typedef AsyncWidgetBuilderOf2<T1, T2> = Widget Function(
    BuildContext context, AsyncSnapshot<T1> val1, AsyncSnapshot<T2> val2);

class StreamBuilderOf2<T1, T2> extends StatelessWidget {
  final Stream<T1> stream1;
  final Stream<T2> stream2;
  final bool loader;
  final bool sliverLoader;
  final AsyncWidgetBuilderOf2<T1, T2> builder;

  StreamBuilderOf2({
    this.stream1,
    this.stream2,
    this.builder,
    this.loader = false,
    this.sliverLoader = false,
  });

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: stream1,
      builder: (BuildContext context, AsyncSnapshot<T1> snapshot1) {
        return StreamBuilder(
          stream: stream2,
          builder: (BuildContext context, AsyncSnapshot<T2> snapshot2) {
            if (loader || sliverLoader) {
              var loaderWidget = Loader(
                loaded: (snapshot1.hasData && snapshot2.hasData),
                widget: () => builder(context, snapshot1, snapshot2),
              );
              if (sliverLoader)
                return SliverToBoxAdapter(child: loaderWidget);
              else
                return loaderWidget;
            } else {
              return builder(context, snapshot1, snapshot2);
            }
          },
        );
      },
    );
  }
}

typedef AsyncWidgetBuilderOf3<T1, T2, T3> = Widget Function(
    BuildContext context,
    AsyncSnapshot<T1> val1,
    AsyncSnapshot<T2> val2,
    AsyncSnapshot<T3> val3);

class StreamBuilderOf3<T1, T2, T3> extends StatelessWidget {
  final Stream<T1> stream1;
  final Stream<T2> stream2;
  final Stream<T3> stream3;
  final bool loader;
  final bool sliverLoader;
  final AsyncWidgetBuilderOf3<T1, T2, T3> builder;

  StreamBuilderOf3({
    this.stream1,
    this.stream2,
    this.stream3,
    this.builder,
    this.loader = false,
    this.sliverLoader = false,
  });

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: stream1,
      builder: (BuildContext context, AsyncSnapshot<T1> snapshot1) {
        return StreamBuilder(
          stream: stream2,
          builder: (BuildContext context, AsyncSnapshot<T2> snapshot2) {
            return StreamBuilder(
              stream: stream3,
              builder: (BuildContext context, AsyncSnapshot<T3> snapshot3) {
                if (loader || sliverLoader) {
                  var loaderWidget = Loader(
                    loaded: (snapshot1.hasData &&
                        snapshot2.hasData &&
                        snapshot3.hasData),
                    widget: () =>
                        builder(context, snapshot1, snapshot2, snapshot3),
                  );
                  if (sliverLoader)
                    return SliverToBoxAdapter(child: loaderWidget);
                  else
                    return loaderWidget;
                } else {
                  return builder(context, snapshot1, snapshot2, snapshot3);
                }
              },
            );
          },
        );
      },
    );
  }
}

typedef AsyncWidgetBuilderOf4<T1, T2, T3, T4> = Widget Function(
  BuildContext context,
  AsyncSnapshot<T1> val1,
  AsyncSnapshot<T2> val2,
  AsyncSnapshot<T3> val3,
  AsyncSnapshot<T4> val4,
);

class StreamBuilderOf4<T1, T2, T3, T4> extends StatelessWidget {
  final Stream<T1> stream1;
  final Stream<T2> stream2;
  final Stream<T3> stream3;
  final Stream<T4> stream4;
  final bool loader;
  final bool sliverLoader;
  final AsyncWidgetBuilderOf4<T1, T2, T3, T4> builder;

  StreamBuilderOf4({
    this.stream1,
    this.stream2,
    this.stream3,
    this.stream4,
    this.builder,
    this.loader = false,
    this.sliverLoader = false,
  });

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: stream1,
      builder: (BuildContext context, AsyncSnapshot<T1> snapshot1) {
        return StreamBuilder(
          stream: stream2,
          builder: (BuildContext context, AsyncSnapshot<T2> snapshot2) {
            return StreamBuilder(
              stream: stream3,
              builder: (BuildContext context, AsyncSnapshot<T3> snapshot3) {
                return StreamBuilder(
                  stream: stream4,
                  builder: (BuildContext context, AsyncSnapshot<T4> snapshot4) {
                    if (loader || sliverLoader) {
                      var loaderWidget = Loader(
                        loaded: (snapshot1.hasData &&
                            snapshot2.hasData &&
                            snapshot3.hasData &&
                            snapshot4.hasData),
                        widget: () => builder(context, snapshot1, snapshot2,
                            snapshot3, snapshot4),
                      );
                      if (sliverLoader)
                        return SliverToBoxAdapter(child: loaderWidget);
                      else
                        return loaderWidget;
                    } else {
                      return builder(
                          context, snapshot1, snapshot2, snapshot3, snapshot4);
                    }
                  },
                );
              },
            );
          },
        );
      },
    );
  }
}

// typedef AsyncWidgetBuilderOf5<T1, T2, T3, T4, T5> = Widget Function(
//     BuildContext context,
//     bool hasData,
//     T1 val1,
//     T2 val2,
//     T3 val3,
//     T4 val4,
//     T5 val5);

// class StreamBuilderOf5<T1, T2, T3, T4, T5> extends StatelessWidget {
//   final Stream<T1> stream1;
//   final Stream<T2> stream2;
//   final Stream<T3> stream3;
//   final Stream<T4> stream4;
//   final Stream<T5> stream5;
//   final AsyncWidgetBuilderOf5<T1, T2, T3, T4, T5> builder;

//   StreamBuilderOf5(
//       {this.stream1,
//       this.stream2,
//       this.stream3,
//       this.stream4,
//       this.stream5,
//       this.builder});

//   @override
//   Widget build(BuildContext context) {
//     return StreamBuilder(
//       stream: Observable.combineLatest5(stream1, stream2, stream3, stream4,
//           stream5, (s1, s2, s3, s4, s5) => [s1, s2, s3, s4, s5]),
//       builder: (BuildContext context, AsyncSnapshot<List<dynamic>> snapshot) {
//         if (snapshot.hasData) {
//           var v1 = snapshot.data[0] as T1;
//           var v2 = snapshot.data[1] as T2;
//           var v3 = snapshot.data[2] as T3;
//           var v4 = snapshot.data[3] as T4;
//           var v5 = snapshot.data[4] as T5;
//           return builder(context, true, v1, v2, v3, v4, v5);
//         } else {
//           return builder(context, false, null, null, null, null, null);
//         }
//       },
//     );
//   }
// }
