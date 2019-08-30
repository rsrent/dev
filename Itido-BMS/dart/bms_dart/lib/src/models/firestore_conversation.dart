import 'package:bms_dart/models.dart';

class FirestoreConversation {
  String conversationId;
  Project project;

  FirestoreConversation.fromJson(json)
      : this.conversationId = json['conversationID'],
        this.project = Project.fromJson(json['projectItem_project']);
}
