class Comment {
  int id;
  String title;
  String body;

  Comment({this.title, this.body});

  factory Comment.fromJson(json) {
    if (json == null) return null;
    return Comment._fromJson(json);
  }

  Comment._fromJson(json)
      : this.id = json['id'],
        this.title = json['title'],
        this.body = json['body'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'title': this.title,
        'body': this.body,
      };
}
