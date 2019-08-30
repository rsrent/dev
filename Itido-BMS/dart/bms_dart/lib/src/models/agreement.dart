class Agreement {
  int id;
  String name;

  Agreement({
    this.id,
    this.name,
  });

  factory Agreement.fromJson(json) {
    if (json == null) return null;
    return Agreement._fromJson(json);
  }

  Agreement._fromJson(json)
      : this.id = json['id'],
        this.name = json['name'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'name': this.name,
      };
}
