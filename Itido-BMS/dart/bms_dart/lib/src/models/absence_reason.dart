class AbsenceReason {
  int id;
  String description;
  bool canUserRequest;
  bool canUserCreate;
  bool canManagerRequest;
  bool canManagerCreate;

  AbsenceReason({
    this.id,
    this.description,
    this.canUserRequest,
    this.canUserCreate,
    this.canManagerRequest,
    this.canManagerCreate,
  });

  factory AbsenceReason.fromJson(json) {
    if (json == null) return null;
    return AbsenceReason._fromJson(json);
  }

  AbsenceReason._fromJson(json)
      : this.id = json['id'],
        this.description = json['description'],
        this.canUserRequest = json['canUserRequest'],
        this.canUserCreate = json['canUserCreate'],
        this.canManagerRequest = json['canManagerRequest'],
        this.canManagerCreate = json['canManagerCreate'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'description': this.description,
        'canUserRequest': this.canUserRequest,
        'canUserCreate': this.canUserCreate,
        'canManagerRequest': this.canManagerRequest,
        'canManagerCreate': this.canManagerCreate,
      };
}
