import 'package:bms_dart/src/models/date_time_converter.dart';

class QualityReport {
  int id;
  DateTime createdTime;
  DateTime completedTime;

  QualityReport({
    this.id,
    this.createdTime,
    this.completedTime,
  });

  factory QualityReport.fromJson(json) {
    if (json == null) return null;
    return QualityReport._fromJson(json);
  }

  QualityReport._fromJson(json)
      : this.id = json['id'],
        this.createdTime = toDateTime(json['time']),
        this.completedTime = toDateTime(json['completedTime']);

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'time': this.createdTime.toString(),
        'completedTime': this.completedTime.toString(),
      };
}
