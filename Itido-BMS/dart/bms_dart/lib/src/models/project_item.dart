import 'package:bms_dart/src/models/project_item_access.dart';

import 'post.dart';

import 'comment.dart';
import 'address.dart';

enum ProjectItemType {
  profileImage,
  comment,
  client,
  location,
  logs,
  tasks,
  qualityReports,
  work,
  workContracts,
  documentFolders,
  firestoreConversations,
  address,
  post,
}

class ProjectItem {
  int id;
  ProjectItemType projectItemType;
  String name;
  Comment comment;
  Address address;
  dynamic documentFolder;
  Post post;
  List<ProjectItemAccess> projectItemAccesses;

  ProjectItem({
    this.id,
    this.projectItemType,
    this.name,
    this.comment,
    this.documentFolder,
  });
  ProjectItem.fromJson(json)
      : this.id = json['id'],
        this.name = json['name'],
        this.comment = Comment.fromJson(json['comment']),
        this.address = Address.fromJson(json['address']),
        this.post = Post.fromJson(json['post']),
        this.documentFolder = json['documentFolder'],
        this.projectItemAccesses = json['projectItemAccesses'] != null
            ? List.castFrom<dynamic, ProjectItemAccess>(
                List.of(json['projectItemAccesses'])
                    .map((accessJson) => ProjectItemAccess.fromJson(accessJson))
                    .toList())
            : null,
        this.projectItemType = ProjectItemType.values[json['projectItemType']];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'name': this.name,
        'projectItemType': this.projectItemType.index,
      };
}
