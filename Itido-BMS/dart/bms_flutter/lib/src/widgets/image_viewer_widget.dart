import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import '../resources/image_controller.dart';

class ImageViewerWidget extends StatelessWidget {
  final ImageFile image;
  final BoxFit fit;
  ImageViewerWidget(this.image, {this.fit});

  @override
  Widget build(BuildContext context) {
    print(image.url);
    return image == null || image.file == null && image.url == null
        ? Container()
        : FutureBuilder(
            future: ImageController.itemImageToFile(image),
            builder: (context, AsyncSnapshot<ImageFile> future) {
              if (!future.hasData)
                return AspectRatio(
                  aspectRatio: 2.0 / 3.0,
                  child: Container(
                    color: Colors.grey[200],
                  ),
                );
              // return Center(
              //   child: CircularProgressIndicator(),
              // );
              return Image.file(future.data.file, fit: fit);
            },
          );
  }
}
