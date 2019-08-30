import 'package:bms_dart/src/models/date_time_converter.dart';

import 'user.dart';

class Post {
  String title;
  String body;
  User sender;
  DateTime sendTime;

  Post({
    this.title,
    this.body,
    this.sendTime,
  });

  factory Post.fromJson(json, {rootJson, path}) {
    if (json == null) return null;
    return Post._fromJson(json, rootJson ?? {}, path ?? '');
  }

  Post._fromJson(json, rootJson, path)
      : this.title = json['title'],
        this.body = json['body'],
        this.sendTime = toDateTime(json['sendTime']),
        this.sender =
            User.fromJson(json['sender'] ?? rootJson['${path}_sender']);

  Map<String, dynamic> toMap() => {
        'title': title,
        'body': body,
        'sendTime': sendTime.toString(),
      };
}
