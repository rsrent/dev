class Client {
  int id;
  String name;

  Client({
    this.id,
    this.name,
  });

  factory Client.fromJson(json) {
    if (json == null) return null;
    return Client._fromJson(json);
  }

  Client._fromJson(json)
      : this.id = json['id'],
        this.name = json['name'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'name': this.name,
      };
}
