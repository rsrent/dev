import 'dart:io';

class Document {
  int id;
  String title;
  File file;
  String url;

  Document({
    this.id,
    this.title,
    this.url,
    this.file,
  });
  Document.fromJson(json)
      : this.id = json['id'],
        this.url = json['url'],
        this.title = json['title'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'title': this.title,
        'url': this.url,
      };
}
