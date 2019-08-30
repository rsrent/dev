class Log {
  final String title;
  final String log;
  final String locationName;
  final String customerName;
  final String userName;
  final DateTime time;
  Log.fromJson(json)
      : title = json['title'],
        log = json['log'],
        locationName = json['locationName'],
        customerName = json['customerName'],
        userName = json['userName'],
        time = DateTime.parse(json['time']);
}


