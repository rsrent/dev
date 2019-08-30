import 'task.dart';

class QualityReportItem {
  int id;
  int rating;
  String comment;
  String imageLocation;
  Task task;

  QualityReportItem({
    this.id,
    this.rating,
    this.comment,
    this.imageLocation,
  });

  factory QualityReportItem.fromJson(json) {
    if (json == null) return null;
    return QualityReportItem._fromJson(json);
  }

  QualityReportItem._fromJson(json)
      : this.id = json['id'],
        this.rating = json['rating'],
        this.comment = json['comment'],
        this.imageLocation = json['imageLocation'],
        this.task = Task.fromJson(json['task']);

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'rating': this.rating,
        'comment': this.comment,
        'imageLocation': this.imageLocation,
      };
}
