import 'package:flutter/material.dart';
import 'package:dart_packages/streamer.dart';

class LoadingOverlay extends StatefulWidget {
  final Streamer<bool> loading;

  const LoadingOverlay({Key key, @required this.loading}) : super(key: key);
  @override
  _LoadingOverlayState createState() => _LoadingOverlayState();
}

class _LoadingOverlayState extends State<LoadingOverlay>
    with TickerProviderStateMixin {
  AnimationController _controller;
  Animation _animation;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    _controller =
        AnimationController(vsync: this, duration: Duration(milliseconds: 500));

    _animation = CurvedAnimation(
        parent: _controller,
        curve: Curves.easeIn,
        reverseCurve: Curves.easeOut);

    widget.loading.stream.listen((_loading) {
      print('_loading: $_loading');
      if (_loading) {
        _controller.forward();
      } else {
        _controller.reverse();
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    // return IgnorePointer(
    //   ignoring: !widget.loading,
    //   child: AnimatedOpacity(
    //     opacity: widget.loading ? 0.5 : 0,
    //     duration: Duration(milliseconds: 250),
    //     child: Container(
    //       color: Colors.grey,
    //       child: Center(child: CircularProgressIndicator()),
    //     ),
    //   ),
    // );

    return IgnorePointer(
      child: Container(
        color: Colors.black12,
        child: Center(
          child: AnimatedBuilder(
            animation: _controller,
            builder: (context, widget) {
              if (_animation.value > 0) {
                var y = -500 + (500 * _animation.value);

                print('_animation.value: ${_animation.value}');
                print('y: $y');

                return Transform.translate(
                  offset: Offset(0, y),
                  child: Container(
                    child: CircularProgressIndicator(),
                    color: Colors.yellow,
                  ),
                );
              }
              return Container();
            },
          ),
        ),
      ),
    );
  }
}
