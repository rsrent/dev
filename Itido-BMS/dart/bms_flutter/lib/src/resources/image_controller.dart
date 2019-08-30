import 'package:firebase_storage/firebase_storage.dart';
import 'dart:async';
import 'dart:io';
import 'package:bms_dart/models.dart';
import 'dart:async';
// import 'flutter_native_image.dart';
import 'dart:typed_data';
import 'package:path_provider/path_provider.dart';
// import 'package:flutter_packages/storage.dart';
import 'package:flutter_native_image/flutter_native_image.dart';

class ImageController {
  static String _tempPath;
  static String _permPath;

  static String get tempPath => _tempPath;
  static String get permPath => _permPath;

  static initialize(String path) async {
    //print('getApplicationDocumentsDirectory: $dir');
    _tempPath = '$path/temp';
    _permPath = '$path/perm';
    var tempDir = Directory(_tempPath);
    if (await tempDir.exists()) {
      await Directory(_tempPath).delete(recursive: true);
    }
    await Directory(_tempPath).create(recursive: true);
    await Directory(_permPath).create(recursive: true);
  }

  static Future<ImageFile> saveImage(ImageFile itemImage) async {
    var file = itemImage?.file;
    if (file != null) {
      if (itemImage.url != null) {
        FirebaseStorage.instance.ref().child(itemImage.url).delete();
      }

      var size = 1000;
      ImageProperties properties =
          await FlutterNativeImage.getImageProperties(file.path);
      file = await FlutterNativeImage.compressImage(file.path,
          quality: 80,
          targetWidth: size,
          targetHeight: (properties.height * size / properties.width).round());

      var fileName = DateTime.now().millisecondsSinceEpoch.toString();

      final StorageReference storageRef =
          FirebaseStorage.instance.ref().child('0/$fileName');

      final StorageUploadTask uploadTask = storageRef.putFile(
        file,
        StorageMetadata(
          contentType: 'image/jpg',
        ),
      );

      final StorageTaskSnapshot downloadUrl = (await uploadTask.onComplete);
      final String url = (await downloadUrl.ref.getDownloadURL());

      itemImage.url = url;
      itemImage.file = null;
    }
    return itemImage;
  }

  static Future<ImageFile> itemImageToFile(ImageFile itemImage) async {
    if (itemImage != null) {
      if (itemImage.file != null) {
        return itemImage;
      } else if (itemImage.url != null) {
        var path = getTemporaryFilePath('jpg', fileName: itemImage.cacheName);
        var file = File(path);
        var exists = await file.exists();

        if (exists) {
          print('Found cashed file!!');

          itemImage.file = file;
          return itemImage;
        }

        final Uri resolved = Uri.base.resolve(itemImage.url);
        final HttpClientRequest request = await HttpClient().getUrl(resolved);
        final HttpClientResponse response = await request.close();
        if (response.statusCode != HttpStatus.ok)
          throw Exception(
              'HTTP request failed, statusCode: ${response?.statusCode}, $resolved');

        final Uint8List bytes =
            await consolidateHttpClientResponseBytes(response);
        if (bytes.lengthInBytes == 0)
          throw Exception('NetworkImage is an empty file: $resolved');

        await file.writeAsBytes(bytes);
        itemImage.file = file;
        return itemImage;
      }
    }
    return null;
  }

  static Future<Uint8List> consolidateHttpClientResponseBytes(
      HttpClientResponse response) {
    // response.contentLength is not trustworthy when GZIP is involved
    // or other cases where an intermediate transformer has been applied
    // to the stream.
    final Completer<Uint8List> completer = Completer<Uint8List>.sync();
    final List<List<int>> chunks = <List<int>>[];
    int contentLength = 0;
    response.listen((List<int> chunk) {
      chunks.add(chunk);
      contentLength += chunk.length;
    }, onDone: () {
      final Uint8List bytes = Uint8List(contentLength);
      int offset = 0;
      for (List<int> chunk in chunks) {
        bytes.setRange(offset, offset + chunk.length, chunk);
        offset += chunk.length;
      }
      completer.complete(bytes);
    }, onError: completer.completeError, cancelOnError: true);

    return completer.future;
  }

  static String timestamp() => DateTime.now().millisecondsSinceEpoch.toString();
  static String get getFileName => timestamp();
  static String getTemporaryFilePath(String fileType, {String fileName}) =>
      '$_tempPath/${fileName ?? getFileName}.$fileType';
  static String getPermanentFilePath(String fileType, {String fileName}) =>
      '$_permPath/${fileName ?? getFileName}.$fileType';
}
