class Task {
  int id;
  int squareMeters;
  int timesOfYear;
  String frequency;
  String description;
  String comment;
  String place;
  bool active;

  Task({
    this.id,
    this.squareMeters,
    this.frequency,
    this.comment,
    this.description,
    this.place,
    this.active,
  });

  factory Task.fromJson(json) {
    if (json == null) return null;
    return Task._fromJson(json);
  }

  Task._fromJson(json)
      : this.id = json['id'],
        this.squareMeters = json['squareMeters'],
        this.timesOfYear = json['timesOfYear'],
        this.frequency = json['frequency'],
        this.comment = json['comment'],
        this.description = json['description'],
        this.place = json['place'],
        this.active = json['active'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'squareMeters': this.squareMeters,
        'timesOfYear': this.timesOfYear,
        'frequency': this.frequency,
        'comment': this.comment,
        'description': this.description,
        'place': this.place,
        'active': this.active,
      };
}
