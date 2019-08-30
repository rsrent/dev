import 'package:flutter/material.dart';
import 'package:dart_packages/streamer.dart';
import 'package:camera/camera.dart';
import 'dart:io';
import '../resources/camera_bloc.dart';
import 'image_viewer_widget.dart';
import '../resources/image_controller.dart';
import 'package:bms_dart/models.dart';
import 'dart:async';

class ImagePickerWidget extends StatefulWidget {
  final Function(ImageFile) onSelect;
  final Streamer<ImageFile> imageStreamer;
  final bool inspectable;
  ImagePickerWidget(this.imageStreamer,
      {this.inspectable = true, this.onSelect});

  @override
  _ImagePickerWidgetState createState() => _ImagePickerWidgetState();
}

class _ImagePickerWidgetState extends State<ImagePickerWidget> {
  CameraBloc _cameraBloc;

  _ImagePickerWidgetState() {
    _cameraBloc = CameraBloc();
  }

  @override
  void dispose() {
    // TODO: implement dispose
    _cameraBloc?.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Center(
      child: StreamBuilder(
        stream: widget.imageStreamer.stream,
        builder: (context, AsyncSnapshot<ImageFile> imageSnap) {
          return Stack(
            fit: StackFit.expand,
            alignment: Alignment.center,
            children: <Widget>[
              if (!imageSnap.hasData)
                FutureBuilder(
                  future: _cameraBloc.cameraLoaded,
                  builder: (context, future) {
                    if (!future.hasData)
                      return Center(child: CircularProgressIndicator());

                    var controller = _cameraBloc.primaryCamera;

                    return ValueListenableBuilder(
                      valueListenable: controller,
                      builder: (context, CameraValue cameraValue, widget) {
                        return AspectRatio(
                            aspectRatio: controller.value.aspectRatio,
                            child: CameraPreview(controller));
                      },
                    );
                  },
                ),
              if (imageSnap.hasData) ImageViewerWidget(imageSnap.data),
              if (imageSnap.hasData)
                Positioned(
                  bottom: 16,
                  left: 16,
                  child: FloatingActionButton(
                    heroTag: 'clear_btn',
                    child: Icon(Icons.clear),
                    onPressed: () {
                      widget.imageStreamer.update(null);
                    },
                  ),
                ),
              if (!imageSnap.hasData)
                Positioned(
                  bottom: 16,
                  child: FloatingActionButton(
                    child: Icon(Icons.camera),
                    onPressed: () {
                      var controller = _cameraBloc.primaryCamera;
                      takePicture(controller);
                    },
                  ),
                ),
              if (imageSnap.hasData)
                Positioned(
                  bottom: 16,
                  right: 16,
                  child: FloatingActionButton(
                    heroTag: 'use_btn',
                    child: Icon(Icons.send),
                    onPressed: () {
                      widget.onSelect(imageSnap.data);
                    },
                  ),
                ),
            ],
          );

          // if (imageSnap.hasData && (imageSnap.data != null)) {
          // } else {
          //   return FutureBuilder(
          //     future: _cameraBloc.cameraLoaded,
          //     builder: (context, future) {
          //       if (!future.hasData)
          //         return Center(child: CircularProgressIndicator());

          //       var controller = _cameraBloc.primaryCamera;

          //       return Stack(
          //         alignment: Alignment.center,
          //         children: <Widget>[
          //           ValueListenableBuilder(
          //             valueListenable: controller,
          //             builder: (context, CameraValue cameraValue, widget) {
          //               return AspectRatio(
          //                   aspectRatio: controller.value.aspectRatio,
          //                   child: CameraPreview(controller));
          //             },
          //           ),
          //           FloatingActionButton(
          //             child: Icon(Icons.camera),
          //             onPressed: () {
          //               var controller = _cameraBloc.primaryCamera;
          //               takePicture(controller);
          //             },
          //           ),
          //         ],
          //       );
          //     },
          //   );
          // }
        },
      ),
    );
  }

  String timestamp() => DateTime.now().millisecondsSinceEpoch.toString();

  takePicture(CameraController controller) async {
    if (!controller.value.isInitialized) {
      //showInSnackBar('Error: select a camera first.');
      return null;
    }
    final String filePath = ImageController.getTemporaryFilePath('jpg');

    if (controller.value.isTakingPicture) {
      // A capture is already pending, do nothing.
      return null;
    }

    try {
      print(filePath);

      await controller.takePicture(filePath);
    } on CameraException catch (e) {
      print('ERROR');
      print(e);

      return null;
    }

    widget.imageStreamer.update(ImageFile(file: File(filePath)));
  }
}

class ImagePickerScreen extends StatelessWidget {
  final Streamer<ImageFile> imageStreamer;
  final Function(ImageFile) onSelected;
  ImagePickerScreen(this.imageStreamer, {this.onSelected});
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(),
      body: Container(
        color: Colors.black,
        child: ImagePickerWidget(
          imageStreamer,
          inspectable: false,
          onSelect: onSelected,
        ),
      ),
    );
  }
}

Future<ImageFile> displayImagePicker(BuildContext context) {
  //Completer<ItemImage> _completer = Completer();
  Streamer<ImageFile> imageStreamer = Streamer();
  return Navigator.of(context)
      .push<ImageFile>(MaterialPageRoute(builder: (context) {
    return ImagePickerScreen(imageStreamer, onSelected: (image) {
      //_completer.complete(image);
      Navigator.of(context, rootNavigator: false).pop(image);
    });
  }));

  //return _completer.future;
}
