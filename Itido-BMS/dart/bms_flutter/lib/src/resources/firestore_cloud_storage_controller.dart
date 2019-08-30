import 'package:firebase_storage/firebase_storage.dart';
import 'dart:async';
import 'dart:io';
import 'package:bms_dart/models.dart';
import 'dart:typed_data';
import 'package:mime/mime.dart' as mime;

class FirestoreCloudStorageController {
  static String _tempPath;
  static String _permPath;

  static String get tempPath => _tempPath;
  static String get permPath => _permPath;

  static initialize(String path) async {
    _tempPath = '$path/temp';
    _permPath = '$path/perm';
    var tempDir = Directory(_tempPath);
    if (await tempDir.exists()) {
      await Directory(_tempPath).delete(recursive: true);
    }
    await Directory(_tempPath).create(recursive: true);
    await Directory(_permPath).create(recursive: true);
  }

  static Future<String> uploadFile(File file, String path) async {
    if (file != null) {
      final StorageReference ref =
          FirebaseStorage.instance.ref().child('$path');
      var contentType = mime.lookupMimeType(file.path);
      final StorageUploadTask uploadTask = ref.putFile(
        file,
        StorageMetadata(
          contentType: contentType,
        ),
      );
      final StorageTaskSnapshot downloadUrl = (await uploadTask.onComplete);
      final String url = (await downloadUrl.ref.getDownloadURL());

      File('$_permPath/cacheLocations/$downloadUrl')
          .writeAsString('${file.path}');

      return url;
    }
    return null;
  }

  static Future<File> downloadFile(
    String downloadUrl, {
    String filePath,
    String cacheName,
  }) async {
    print('downloadFile...');
    print('cacheName: $cacheName');

    if (cacheName != null) {
      var cachedFilePath = File('$_permPath/cachePaths-$cacheName');
      var cachedFilePathExists = await cachedFilePath.exists();
      if (cachedFilePathExists) {
        print('cachedFilePathExists!!!!!');
        var cachedFile = File(await cachedFilePath.readAsString());
        var cachedFileExists = await cachedFile.exists();
        if (cachedFileExists) {
          print('cachedFileExists ALSO!!!!!');
          return cachedFile;
        }
      }
    }

    print('File is not cached - downloading');

    var file = File(filePath ?? _getTemporaryFilePath());

    print('new file-path: ${file.path}');

    final Uri resolved = Uri.base.resolve(downloadUrl);
    final HttpClientRequest request = await HttpClient().getUrl(resolved);
    final HttpClientResponse response = await request.close();
    if (response.statusCode != HttpStatus.ok)
      throw Exception(
          'HTTP request failed, statusCode: ${response?.statusCode}, $resolved');
    print('D1');
    final Uint8List bytes = await _consolidateHttpClientResponseBytes(response);
    if (bytes.lengthInBytes == 0)
      throw Exception('NetworkImage is an empty file: $resolved');
    print('D2');
    await file.writeAsBytes(bytes);
    print('D3');
    if (cacheName != null) {
      var cacheFile = File('$_permPath/cachePaths-$cacheName');
      await cacheFile.writeAsString('${file.path}');
    }
    print('D4');
    return file;
  }

  static Future<Uint8List> _consolidateHttpClientResponseBytes(
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

  static String _timestamp() =>
      DateTime.now().millisecondsSinceEpoch.toString();
  static String get _getFileName => _timestamp();
  static String _getTemporaryFilePath() => '$_tempPath/$_getFileName';
  static String getPermanentFilePath(String fileType, {String fileName}) =>
      '$_permPath/${fileName ?? _getFileName}.$fileType';
}
