import 'dart:io';
import 'package:path_provider/path_provider.dart';

class Storage {
  static Future<String> get _localPath async {
    final directory = await getApplicationDocumentsDirectory();
    return directory.path;
  }

  static Future<File> _localFile(String filename) async {
    final path = await _localPath;
    return File('$path/$filename.txt');
  }

  static Future<String> read(String filename) async {
    try {
      final file = await _localFile(filename);

      // Read the file
      String contents = await file.readAsString();

      return contents;
    } catch (e) {
      // If we encounter an error, return 0
      return null;
    }
  }

  static Future<File> write(String filename, String object) async {
    final file = await _localFile(filename);

    // Write the file
    return file.writeAsString('$object');
  }
}
