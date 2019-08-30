import 'package:bloc/bloc.dart';
import 'package:bms_dart/blocs.dart';
import 'package:bms_flutter/src/components/animated_transition.dart';
import 'package:bms_flutter/src/language/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:dart_packages/streamer.dart';
import 'package:flutter/material.dart';
import 'package:flutter/physics.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:keyboard_visibility/keyboard_visibility.dart';
import 'dart:math' as math;

class InspectScreen extends StatefulWidget {
  final InspectScreenController controller;
  final List<InspectScreenItem> items;
  final String title;
  final Widget child;
  final int startIndex;
  final Color backgroundColor;
  final Color middlegroundColor;

  const InspectScreen({
    Key key,
    this.controller,
    @required this.items,
    @required this.child,
    this.title,
    this.startIndex,
    this.backgroundColor,
    this.middlegroundColor,
  }) : super(key: key);
  @override
  _InspectScreenState createState() => _InspectScreenState();
}

class _InspectScreenState extends State<InspectScreen>
    with TickerProviderStateMixin {
  final PageStorageBucket bucket = PageStorageBucket();
  InspectScreenController _controller;
  TextEditingController _textEditingController;
  TabController _tabController;
  FocusNode _textFocusNode = FocusNode();
  // bool showBody = false;
  // bool showSeachBar = false;

  AnimationController _animationController;
  // TickerFuture _tickerFuture;
  // Animation _animation;

  List<Widget> pages;
  //List<GlobalKey> keys;

  @override
  void dispose() {
    // TODO: implement dispose
    //_tickerFuture.timeout(timeLimit)
    //_tickerFuture.
    _animationController.dispose();
    super.dispose();
  }

  @override
  void initState() {
    Future.delayed(Duration.zero, () {
      pages = widget.items.map((i) => i.child).toList();
      //keys = widget.items.map((i) => GlobalKey()).toList();

      _controller = widget.controller ??
          InspectScreenController(
            length: widget.items.length,
            startFrontOpen: true,
            startIndex: widget.startIndex,
          );
      _textEditingController = TextEditingController();
      _tabController = TabController(
        initialIndex: _controller.stateStreamer.value.index,
        length: _controller.length,
        vsync: this,
      )..addListener(() {
          //_tabController.
          _controller.stateStreamer.update(_controller.stateStreamer.value
              .copyWith(index: _tabController.index));
        });

      KeyboardVisibilityNotification().addNewListener(
        onChange: (bool visible) {
          _controller.stateStreamer.update(
              _controller.stateStreamer.value.copyWith(keyboardShown: visible));
        },
      );

      backgroundColor =
          widget.backgroundColor ?? Theme.of(context).primaryColor;

      var backgroundBrightness =
          ThemeData.estimateBrightnessForColor(backgroundColor);

      if (backgroundBrightness == Brightness.dark) {
        backgroundContrastColor = Colors.white;
        middlegroundColor = widget.middlegroundColor ?? Colors.grey[300];
      } else {
        backgroundContrastColor = Colors.black;
        middlegroundColor = widget.middlegroundColor ?? Colors.grey[800];
      }

      _animationController = AnimationController(
          vsync: this, duration: Duration(milliseconds: 500));

      _animationController.addListener(() {
        // print('AC: ${_animationController.value}');

        _controller.stateStreamer.update(
          _controller.stateStreamer.value.copyWith(
            hidePercent: _animationController.value,
          ),
        );
      });

      var middlegroundBrightness =
          ThemeData.estimateBrightnessForColor(middlegroundColor);

      if (middlegroundBrightness == Brightness.dark) {
        middlegroundContrastColor = backgroundBrightness == Brightness.dark
            ? Colors.white
            : backgroundColor;
      } else {
        middlegroundContrastColor = backgroundBrightness == Brightness.light
            ? Colors.white
            : backgroundColor;
      }

      setState(() {});
    });
    super.initState();
  }

  Color backgroundColor;
  Color middlegroundColor;
  Color backgroundContrastColor;
  Color middlegroundContrastColor;

  double paddingTop;

  double startPercent;
  double startPosition;
  double localStartTouch;
  double globalStartTouch;

  double middleMin;
  double middleMax;
  double frontMin;
  double frontMax;

  @override
  Widget build(BuildContext context) {
    var mediaQuery = MediaQuery.of(context);

    var height = mediaQuery.size.height -
        (mediaQuery.padding.top + mediaQuery.padding.bottom);

    middleMin = 60.0;
    middleMax = height - 72;
    frontMin = 140;
    frontMax = height;
    paddingTop = mediaQuery.padding.top;

    return Scaffold(
      backgroundColor: backgroundColor,
      body: Padding(
        padding: EdgeInsets.only(top: mediaQuery.padding.top),
        child: _controller != null
            ? StreamBuilder<InspectScreenState>(
                stream: _controller.stateStreamer.stream,
                builder: (context, snapshot) {
                  var state = snapshot.data;
                  if (state == null) return Container();

                  var middleGroundDist =
                      (middleMax - middleMin) * state.hidePercent + middleMin;
                  // var middleGroundDist = (state.showFront ? 60.0 : height - 60);
                  var foregroundDist =
                      ((frontMax - frontMin) * state.hidePercent + frontMin) +
                          (state.keyboardShown ? -140 : 0);
                  // var foregroundDist = (state.showFront ? 120.0 : height) +
                  //     (state.keyboardShown ? -120 : 0);

                  return Stack(
                    children: <Widget>[
                      Positioned.fill(
                        child: ListTileTheme(
                          iconColor: backgroundContrastColor,
                          textColor: backgroundContrastColor,
                          child: Theme(
                            data: ThemeData(
                              iconTheme: IconThemeData(
                                color: backgroundContrastColor,
                              ),
                            ),
                            child: Container(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.stretch,
                                children: <Widget>[
                                  Container(
                                    height: 60,
                                    child: Row(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.center,
                                      mainAxisAlignment:
                                          MainAxisAlignment.spaceBetween,
                                      children: <Widget>[
                                        IconButton(
                                          icon: Icon(Icons.arrow_back),
                                          onPressed: () {
                                            Navigator.of(context).pop();
                                          },
                                        ),
                                        Expanded(
                                          child: GestureDetector(
                                            onTap: () {
                                              print('Tap ${state.hidePercent}');
                                              if (!_animationController
                                                      .isAnimating &&
                                                  state.hidePercent == 0.0) {
                                                _animationController.fling(
                                                    velocity: 1.0);
                                              }
                                            },
                                            child: Container(
                                              color: Colors.transparent,
                                              child: Padding(
                                                padding: const EdgeInsets.only(
                                                    left: 8, right: 8),
                                                child: Text(
                                                  widget.title ?? '',
                                                  style: Theme.of(context)
                                                      .textTheme
                                                      .title
                                                      .copyWith(
                                                        color:
                                                            backgroundContrastColor,
                                                        fontSize: 18,
                                                      ),
                                                ),
                                              ),
                                            ),
                                          ),
                                        ),
                                        // IconButton(
                                        //   icon: Icon(Icons.menu),
                                        //   onPressed: () {
                                        //     if (!_animationController
                                        //         .isAnimating) {
                                        //       _animationController.fling(
                                        //           velocity:
                                        //               state.hidePercent == 0.0
                                        //                   ? 1.0
                                        //                   : -1.0);
                                        //     }
                                        //   },
                                        // ),
                                      ],
                                    ),
                                  ),
                                  Expanded(
                                    child: widget.child,
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ),
                      ),
                      Positioned(
                        left: 0,
                        bottom: 0,
                        right: 0,
                        top: middleGroundDist,
                        child: GestureDetector(
                          onVerticalDragStart: (details) {
                            startPercent =
                                _controller.stateStreamer.value.hidePercent;
                            startPosition = details.globalPosition.dy;
                            localStartTouch = details.localPosition.dy;
                            globalStartTouch = details.globalPosition.dy;
                          },
                          onVerticalDragUpdate: (details) {
                            var _p =
                                ((details.globalPosition.dy - startPosition) /
                                        (middleMax - middleMin)) +
                                    startPercent;
                            var _hidePercent = math.max(math.min(1.0, _p), 0.0);
                            _controller.stateStreamer.update(
                              state.copyWith(
                                hidePercent: _hidePercent,
                              ),
                            );
                          },
                          onVerticalDragEnd: (details) {
                            var start =
                                _controller.stateStreamer.value.hidePercent;

                            var end = (details.primaryVelocity > 0 ? 1.0 : 0.0);
                            var velocity = details.primaryVelocity /
                                (middleMax - middleMin);
                            if (velocity.abs() < 1) {
                              _animationController.forward(from: start);
                              _animationController.fling(
                                  velocity: (start > 0.5 ? 1.0 : -1.0));
                            } else {
                              _animationController
                                  .animateWith(FrictionSimulation.through(
                                start,
                                end,
                                velocity,
                                (velocity > 0 ? 1.0 : -1.0),
                              ));
                            }
                          },
                          child: Container(
                            decoration: BoxDecoration(
                              borderRadius: const BorderRadius.only(
                                topLeft: const Radius.circular(24),
                                topRight: const Radius.circular(24),
                              ),
                              color: middlegroundColor,
                            ),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.stretch,
                              children: <Widget>[
                                Center(
                                  child: Container(
                                    margin: EdgeInsets.only(top: 8),
                                    width: 60,
                                    height: 4,
                                    decoration: BoxDecoration(
                                      color: backgroundColor,
                                      borderRadius:
                                          BorderRadius.all(Radius.circular(3)),
                                    ),
                                  ),
                                ),
                                Row(
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: <Widget>[
                                    Expanded(child: _buildFrontTopRow(state)),
                                  ],
                                ),
                                Expanded(
                                  child: Container(),
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),
                      Positioned(
                        left: 0,
                        bottom: 0,
                        right: 0,
                        top: foregroundDist,
                        child: ClipRRect(
                          borderRadius: const BorderRadius.only(
                            topLeft: const Radius.circular(24),
                            topRight: const Radius.circular(24),
                          ),
                          child: Container(
                            decoration: BoxDecoration(
                              color: Theme.of(context).scaffoldBackgroundColor,
                            ),
                            child: PageStorage(
                              bucket: bucket,
                              child: TabBarView(
                                controller: _tabController,
                                children: pages,
                              ),
                            ),
                          ),
                        ),
                      ),
                    ],
                  );
                },
              )
            : Container(),
      ),
    );
  }

  Widget _buildFrontTopRow(InspectScreenState state) {
    var currentItem = widget.items[state.index];

    return Container(
      height: 60,
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: List<Widget>.generate(
          _controller.length,
          (i) => _buildNavigationButton(state, widget.items[i], i),
        ),
      ),
    );
  }

  Widget _buildNavigationButton(
      InspectScreenState state, InspectScreenItem item, int index) {
    return IconButton(
      color:
          state.index == index ? middlegroundContrastColor : Colors.grey[500],
      icon: item.icon,
      onPressed: () {
        if (!_animationController.isAnimating && state.hidePercent == 1.0) {
          _animationController.fling(velocity: -1.0);
        }
        // _animationController.fling()
        _controller.stateStreamer.update(state.copyWith(
          index: index,
          // hidePercent: 0.0,
          // showFront: true,
        ));
        _tabController.animateTo(index);
      },
    );
  }
}

class InspectScreenController {
  Streamer<InspectScreenState> stateStreamer;
  int length;

  InspectScreenController({this.length, int startIndex, bool startFrontOpen}) {
    stateStreamer = Streamer(
      seedValue: InspectScreenState(
        index: startIndex ?? 0,
        showSearchBar: false,
        keyboardShown: false,
        hidePercent: startFrontOpen ? 0.0 : 1.0,
      ),
    );
  }
}

class InspectScreenState {
  final int index;
  final bool showSearchBar;
  final bool keyboardShown;
  final double hidePercent;

  InspectScreenState({
    this.index,
    this.showSearchBar,
    this.keyboardShown,
    this.hidePercent,
  });

  InspectScreenState copyWith({
    int index,
    bool showSearchBar,
    bool keyboardShown,
    double hidePercent,
  }) {
    var newIndex = (index ?? this.index) != this.index;

    return InspectScreenState(
      index: index ?? this.index,
      showSearchBar: (showSearchBar ?? this.showSearchBar) && !newIndex,
      keyboardShown: keyboardShown ?? this.keyboardShown,
      hidePercent: hidePercent ?? this.hidePercent,
    );
  }
}

class InspectScreenItem {
  final Widget icon;
  final Widget child;
  InspectScreenItem({
    @required this.icon,
    @required this.child,
  });
}
