import 'dart:io';

class ImageFile {
  String url;
  File file;
  String cacheName;

  ImageFile({this.file, this.url});

  ImageFile.fromJson(json) : this.url = json['url'];

  Map<String, dynamic> toMap() => {
        'url': this.url,
      };
}
