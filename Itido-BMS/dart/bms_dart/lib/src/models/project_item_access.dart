class ProjectItemAccess {
  int projectItemID;
  int projectRoleID;
  bool read;
  bool write;

  ProjectItemAccess({
    this.projectItemID,
    this.projectRoleID,
    this.read,
    this.write,
  });
  ProjectItemAccess.fromJson(json)
      : this.projectItemID = json['projectItemID'],
        this.projectRoleID = json['projectRoleID'],
        this.read = json['read'],
        this.write = json['write'];

  Map<String, dynamic> toMap() => {
        'projectItemID': this.projectItemID,
        'projectRoleID': this.projectRoleID,
        'read': this.read,
        'write': this.write,
      };
}
