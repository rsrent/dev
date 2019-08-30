import 'package:bms_dart/src/models/date_time_converter.dart';

class Log {
  int id;
  String title;
  String log;
  DateTime dateCreated;

  Log({
    this.id,
    this.title,
    this.log,
    this.dateCreated,
  });
  Log.fromJson(json)
      : this.id = json['id'],
        this.title = json['title'],
        this.log = json['log'],
        this.dateCreated = toDateTime(json['dateCreated']);

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'title': this.title,
        'log': this.log,
        'dateCreated': this.dateCreated.toString(),
      };
}
