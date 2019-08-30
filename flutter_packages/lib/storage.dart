import 'dart:io';
import 'package:path_provider/path_provider.dart';
import 'dart:async';

class Storage {
  static String _localPath;

  static String get localPath => _localPath;

  static Future<void> setPath() async {
    final directory = await getApplicationDocumentsDirectory();
    _localPath = directory.path;
  }

  // static Future<String> get _localPath async {
  //   final directory = await getApplicationDocumentsDirectory();
  //   return directory.path;
  // }

  static File createLocalFile(String filename, {String path}) {
    final _path = path ?? _localPath;
    return File('$_path/$filename.txt');
  }

  static Future<String> read(String filename, {String path}) async {
    try {
      final file = createLocalFile(filename, path: path);

      // Read the file
      String contents = await file.readAsString();

      return contents;
    } catch (e) {
      // If we encounter an error, return 0
      return null;
    }
  }

  static Future<File> write(String filename, String object,
      {String path}) async {
    final file = createLocalFile(filename, path: path);

    // Write the file
    return file.writeAsString('$object');
  }

  static Future<File> readFile(String filename, {String path}) async {
    try {
      final file = createLocalFile(filename, path: path);

      // Read the file
      // String contents = await file.readAsString();

      return file;
    } catch (e) {
      // If we encounter an error, return 0
      return null;
    }
  }

  static Future<File> writeFile(String filename, File object,
      {String path}) async {
    final file = createLocalFile(filename, path: path);

    // Write the file
    return file.writeAsBytes(await object.readAsBytes());
  }

  static String readSync(String filename, {String path}) {
    try {
      final file = createLocalFile(filename, path: path);

      // Read the file
      String contents = file.readAsStringSync();

      return contents;
    } catch (e) {
      // If we encounter an error, return 0
      return null;
    }
  }

  static File writeSync(String filename, String object, {String path}) {
    final file = createLocalFile(filename, path: path);

    // Write the file
    file.writeAsStringSync('$object');

    return file;
  }
}
