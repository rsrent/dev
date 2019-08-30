import './data.dart';

class Location {
  final int id;
  final String name;
  final String customerName;
  final String serviceLeaderName;
  final DateTime nextQualityReport;
  final DateTime agreedNextQualityReport;
  final SimpleData smallData;

  Location.fromJson(json)
      : id = json['id'],
        name = json['name'],
        customerName = json['customerName'],
        serviceLeaderName = json['serviceLeaderName'],
        nextQualityReport = json['nextQualityReport'] != null
            ? DateTime.parse(json['nextQualityReport'])
            : null,
        agreedNextQualityReport = json['agreedNextQualityReport'] != null
            ? DateTime.parse(json['agreedNextQualityReport'])
            : null,
        smallData = SimpleData(json);
}
