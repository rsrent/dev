import 'package:bms_dart/src/models/date_time_converter.dart';

class TaskCompleted {
  int id;
  DateTime completedDate;
  String comment;
  bool confirmed;

  TaskCompleted({
    this.id,
    this.completedDate,
    this.comment,
    this.confirmed,
  });

  factory TaskCompleted.fromJson(json) {
    if (json == null) return null;
    return TaskCompleted._fromJson(json);
  }

  TaskCompleted._fromJson(json)
      : this.id = json['id'],
        this.completedDate = toDateTime(json['completedDate']),
        this.confirmed = json['confirmed'],
        this.comment = json['comment'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'completedDate': this.completedDate.toString(),
        'confirmed': this.confirmed,
        'comment': this.comment,
      };
}
