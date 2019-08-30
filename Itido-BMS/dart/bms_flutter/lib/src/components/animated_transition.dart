import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';

class AnimatedTransition extends StatefulWidget {
  static TransitionWidget loadingTransition = TransitionWidget(
    name: 'Loading',
    curveReverse: Curves.bounceOut,
    revealTypeReverse: RevealType.Scale,
    child: Center(
      child: RefreshProgressIndicator(),
    ),
    durationReverse: Duration(milliseconds: 1000),
  );
  static TransitionWidget emptyTransition = TransitionWidget(
    name: 'empty',
    curveReverse: Curves.bounceOut,
    revealTypeReverse: RevealType.Scale,
    child: Container(),
    durationReverse: Duration(milliseconds: 1000),
  );

  final TransitionWidget child;
  final Duration duration;
  final RevealType revealType;
  final RevealType revealTypeReverse;
  final Curve curve;
  final Curve curveReverse;
  final bool animateFirst;
  final double width;
  final double height;

  static const Duration _defaultDuration = Duration(milliseconds: 500);

  const AnimatedTransition({
    Key key,
    @required this.child,
    Duration duration,
    RevealType revealType,
    Curve curve,
    RevealType revealTypeReverse,
    Curve curveReverse,
    this.width,
    this.height,
    this.animateFirst = true,
  })  : this.revealType = revealType ?? RevealType.FadeIn,
        this.curve = curve ?? Curves.easeOut,
        this.revealTypeReverse = revealTypeReverse ?? RevealType.Instant,
        this.curveReverse = curveReverse ?? Curves.easeOut,
        this.duration = duration ?? _defaultDuration,
        super(key: key);
  @override
  _AnimatedTransitionState createState() =>
      _AnimatedTransitionState(animateFirst);
}

class _AnimatedTransitionState extends State<AnimatedTransition>
    with TickerProviderStateMixin {
  TransitionWidget _old;
  TransitionWidget _current;

  AnimationController controller;
  AnimationController controllerReverse;

  _AnimatedTransitionState(bool animateFirst) {
    _current = animateFirst
        ? TransitionWidget(
            child: Container(), name: 'AnimatedTransitionNewChildPlaceholder')
        : null;
  }

  @override
  void initState() {
    print('initState');
    controller = AnimationController(vsync: this, duration: widget.duration);
    controllerReverse =
        AnimationController(vsync: this, duration: widget.duration);
    super.initState();
  }

  @override
  void dispose() {
    controller.dispose();
    controllerReverse.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (_current != null && widget.child.name != _current.name) {
      _old = _current;
      _current = widget.child;

      controller?.dispose();
      controller = AnimationController(
        vsync: this,
        duration: _current.duration ?? widget.duration,
      );
      controller.forward(from: 0);

      controllerReverse?.dispose();
      controllerReverse = AnimationController(
        vsync: this,
        duration: _old.durationReverse ?? widget.duration,
      );
      controllerReverse.reverse(from: 1);
    } else {
      _current = widget.child;
    }

    // print('name: ${_current.name}');

    return SizedBox(
      width: widget.width,
      height: widget.height,
      child: _old != null
          ? Stack(
              children: <Widget>[
                Positioned.fill(
                  child: IgnorePointer(
                    child: AnimatedBuilder(
                      animation: controllerReverse,
                      builder: (context, child) {
                        return _buildChild(
                          context,
                          controllerReverse.value,
                          _old,
                          true,
                        );
                      },
                    ),
                  ),
                ),
                Positioned.fill(
                  child: AnimatedBuilder(
                    animation: controller,
                    builder: (context, child) {
                      return _buildChild(
                        context,
                        controller.value,
                        _current,
                        false,
                      );
                    },
                  ),
                ),
              ],
            )
          : _current.child,
    );
  }

  Widget _buildChild(BuildContext context, double revealValue,
      TransitionWidget transitionWidget, bool reverse) {
    var rt = !reverse
        ? transitionWidget.revealType != null
            ? transitionWidget.revealType
            : widget.revealType
        : transitionWidget.revealTypeReverse != null
            ? transitionWidget.revealTypeReverse
            : widget.revealTypeReverse;

    var curv = !reverse
        ? transitionWidget.curve != null ? transitionWidget.curve : widget.curve
        : transitionWidget.curveReverse != null
            ? transitionWidget.curveReverse
            : widget.curveReverse;

    var rv = curv.transform(revealValue);

    switch (rt) {
      case RevealType.Instant:
        return _buildFadeIn(rv > 0 ? 1 : 0, transitionWidget.child);
      case RevealType.FadeIn:
        return _buildFadeIn(rv, transitionWidget.child);
      case RevealType.SlideInFromRight:
        return _buildSlideInFromRight(context, rv, transitionWidget.child);
      case RevealType.SlideInFromLeft:
        return _buildSlideInFromLeft(context, rv, transitionWidget.child);
      case RevealType.SlideInFromTop:
        return _buildSlideInFromTop(context, rv, transitionWidget.child);
      case RevealType.SlideInFromBottom:
        return _buildSlideInFromBottom(context, rv, transitionWidget.child);
      case RevealType.Scale:
        return _buildScale(context, rv, transitionWidget.child);
      case RevealType.VerticalClipInTop:
        return _buildVerticalClipInTop(rv, transitionWidget.child);
      case RevealType.VerticalClipInCenter:
        return _buildVerticalClipInCenter(rv, transitionWidget.child);
      case RevealType.VerticalClipInBottom:
        return _buildVerticalClipInBottom(rv, transitionWidget.child);
      case RevealType.HorizontalClipInLeft:
        return _buildHorizontalClipInLeft(rv, transitionWidget.child);
      case RevealType.HorizontalClipInCenter:
        return _buildHorizontalClipInCenter(rv, transitionWidget.child);
      case RevealType.HorizontalClipInRight:
        return _buildHorizontalClipInRight(rv, transitionWidget.child);
    }
    return transitionWidget.child;
  }

  Widget _buildFadeIn(double revealValue, Widget child) {
    return Opacity(
      opacity: revealValue,
      child: child,
    );
  }

  Widget _buildSlideInFromRight(
      BuildContext context, double revealValue, Widget child) {
    var width = MediaQuery.of(context).size.width;
    return Transform.translate(
      offset: Offset((1 - revealValue) * width, 0),
      child: child,
    );
  }

  Widget _buildSlideInFromLeft(
      BuildContext context, double revealValue, Widget child) {
    var width = MediaQuery.of(context).size.width;
    return Transform.translate(
      offset: Offset(-width + (revealValue * width), 0),
      child: child,
    );
  }

  Widget _buildSlideInFromTop(
      BuildContext context, double revealValue, Widget child) {
    var height = MediaQuery.of(context).size.height;
    return Transform.translate(
      offset: Offset(0, -height + (revealValue * height)),
      child: child,
    );
  }

  Widget _buildSlideInFromBottom(
      BuildContext context, double revealValue, Widget child) {
    var height = MediaQuery.of(context).size.height;
    return Transform.translate(
      offset: Offset(0, (1 - revealValue) * height),
      child: child,
    );
  }

  Widget _buildVerticalClipInTop(double revealValue, Widget child) {
    return Align(
      alignment: Alignment.topCenter,
      child: ClipRect(
        child: Align(
          heightFactor: revealValue,
          child: child,
        ),
      ),
    );
  }

  Widget _buildVerticalClipInCenter(double revealValue, Widget child) {
    return Align(
      child: ClipRect(
        child: Align(
          heightFactor: revealValue,
          child: child,
        ),
      ),
    );
  }

  Widget _buildVerticalClipInBottom(double revealValue, Widget child) {
    return Align(
      alignment: Alignment.bottomCenter,
      child: ClipRect(
        child: Align(
          heightFactor: revealValue,
          child: child,
        ),
      ),
    );
  }

  Widget _buildHorizontalClipInLeft(double revealValue, Widget child) {
    return Align(
      alignment: Alignment.centerLeft,
      child: ClipRect(
        child: Align(
          widthFactor: revealValue,
          child: child,
        ),
      ),
    );
  }

  Widget _buildHorizontalClipInCenter(double revealValue, Widget child) {
    return Align(
      child: ClipRect(
        child: Align(
          widthFactor: revealValue,
          child: child,
        ),
      ),
    );
  }

  Widget _buildHorizontalClipInRight(double revealValue, Widget child) {
    return Align(
      alignment: Alignment.centerRight,
      child: ClipRect(
        child: Align(
          widthFactor: revealValue,
          child: child,
        ),
      ),
    );
  }

  Widget _buildScale(BuildContext context, double revealValue, Widget child) {
    return Transform.scale(
      scale: revealValue,
      child: child,
    );
  }
}

enum RevealType {
  Instant,
  FadeIn,
  SlideInFromRight,
  SlideInFromLeft,
  SlideInFromTop,
  SlideInFromBottom,
  VerticalClipInCenter,
  VerticalClipInTop,
  VerticalClipInBottom,
  HorizontalClipInCenter,
  HorizontalClipInLeft,
  HorizontalClipInRight,
  Scale,
}

class TransitionWidget {
  final String name;
  final Widget child;
  final RevealType revealType;
  final Curve curve;
  final RevealType revealTypeReverse;
  final Curve curveReverse;
  final Duration duration;
  final Duration durationReverse;

  TransitionWidget({
    @required this.name,
    @required this.child,
    this.revealType,
    this.curve,
    this.revealTypeReverse,
    this.curveReverse,
    this.duration,
    this.durationReverse,
  });
}
