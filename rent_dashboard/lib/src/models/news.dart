class News {
  int id;
  String title;
  String body;
  DateTime time;

  News.fromJson(json)
      : id = json['id'],
        title = json['title'],
        body = json['body'],
        time = DateTime.parse(json['time']) {
          
        }
}
