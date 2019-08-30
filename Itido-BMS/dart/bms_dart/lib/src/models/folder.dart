class Folder {
  int id;
  String title;

  Folder({
    this.id,
    this.title,
  });
  Folder.fromJson(json)
      : this.id = json['id'],
        this.title = json['title'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'title': this.title,
      };
}
