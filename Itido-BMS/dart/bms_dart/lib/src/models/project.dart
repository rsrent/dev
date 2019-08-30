class Project {
  int id;
  String name;
  int childrenCount;
  bool disabled = false;
  bool isClient;
  List<String> path;

  Project({
    this.id,
    this.name,
    this.childrenCount,
  });
  Project.fromJson(json)
      : this.id = json['id'],
        this.name = json['name'],
        this.childrenCount = json['childrenCount'],
        this.isClient = json['isClient'] ?? false,
        this.path = (json['path'] ?? '').toString().split('*/*');

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'name': this.name,
        'childrenCount': this.childrenCount,
      };
}
