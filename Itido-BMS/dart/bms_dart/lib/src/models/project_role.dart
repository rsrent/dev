class ProjectRole {
  int id;
  String name;
  bool hasAllPermissions;

  ProjectRole({
    this.id,
    this.name,
  });

  factory ProjectRole.fromJson(json, {rootJson, path}) {
    if (json == null) return null;
    return ProjectRole._fromJson(json, rootJson ?? {}, path ?? '');
  }

  ProjectRole._fromJson(json, rootJson, path)
      : this.id = json['id'],
        this.name = json['name'],
        this.hasAllPermissions = json['hasAllPermissions'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'name': this.name,
      };
}
