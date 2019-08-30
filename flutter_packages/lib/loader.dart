import 'package:flutter/material.dart';
import 'snackbar_controller.dart';

class Loader extends StatelessWidget {
  final bool loaded;
  final Widget Function() widget;

  Loader({this.loaded, this.widget});
  @override
  Widget build(BuildContext context) {
    if (loaded) {
      return widget();
    } else {
      return Center(child: CircularProgressIndicator());
    }
  }
}

class LoadingScreen extends StatelessWidget {
  final Stream<bool> loading;
  final Widget child;

  LoadingScreen({this.loading, this.child});

  @override
  Widget build(BuildContext context) {
    var screenSize = MediaQuery.of(context).size;

    return Stack(
      children: <Widget>[
        child,
        Positioned(
            top: 0,
            left: 0,
            width: screenSize.width,
            height: screenSize.height,
            child: StreamBuilder(
              stream: loading,
              builder: (context, snapshot) {
                var loading = snapshot.data ?? false;
                if (!loading) {
                  return IgnorePointer(
                    child: Container(),
                  );
                } else {
                  return Container(
                    color: Colors.black26,
                    child: Center(
                      child: CircularProgressIndicator(),
                    ),
                  );
                }
              },
            )),
      ],
    );
  }
}

typedef String SnackBarText<T>(T result);

Future loadingAndSnackbar<T>({
  BuildContext context,
  String text,
  Future<T> future,
  SnackBarText<T> snackbarText,
}) {
  loadingOverlayWhile(
      context,
      text,
      showSnackbar(
        context: context,
        future: future,
        onCompleteShowMessage: snackbarText,
      ));
}

Future loadingOverlayWhile(BuildContext context, String text, Future future) {
  var overlay = loadingOverlay(context, text);

  future.then((_) => overlay.remove());
}

OverlayEntry loadingOverlay(BuildContext context, String text) {
  OverlayEntry _backGroundOverlay;

  _backGroundOverlay = OverlayEntry(builder: (context) {
    return Container(
      color: Colors.black.withAlpha(200),
      child: Material(
        color: Colors.transparent,
        child: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              Text(
                text,
                style: TextStyle(color: Colors.white, fontSize: 20),
              ),
              Container(
                height: 50,
              ),
              CircularProgressIndicator(
                valueColor: new AlwaysStoppedAnimation<Color>(Colors.white),
              ),
            ],
          ),
        ),
      ),
    );
  });
  Overlay.of(context).insertAll([_backGroundOverlay]);
  return _backGroundOverlay;
}
