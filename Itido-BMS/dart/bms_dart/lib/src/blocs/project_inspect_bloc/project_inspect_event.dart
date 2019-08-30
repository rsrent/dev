import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class ProjectInspectEvent extends Equatable {
  ProjectInspectEvent([List props = const []]) : super(props);
}

class ProjectInspectEventFetch extends ProjectInspectEvent {
  @override
  String toString() => 'ProjectInspectEventStarted';
}

class ProjectInspectEventLoaded extends ProjectInspectEvent {
  final Project project;
  final List<ProjectItem> projectItems;
  ProjectInspectEventLoaded(
      {@required this.project, @required this.projectItems})
      : super([project, projectItems]);
  @override
  String toString() => 'ProjectInspectEventLoaded';
}

class ProjectInspectEventAddProject extends ProjectInspectEvent {
  final String name;
  final int projectId;
  ProjectInspectEventAddProject({@required this.name, @required this.projectId})
      : super([name, projectId]);
  @override
  String toString() =>
      'ProjectInspectEventAddProject { name: $name, projectId: $projectId }';
}

class ProjectInspectEventAddAddress extends ProjectInspectEvent {
  final int projectId;
  ProjectInspectEventAddAddress({@required this.projectId})
      : super([projectId]);
  @override
  String toString() =>
      'ProjectInspectEventAddAddress { projectId: $projectId }';
}

class ProjectInspectEventAddLogs extends ProjectInspectEvent {
  final int projectId;
  ProjectInspectEventAddLogs({@required this.projectId}) : super([projectId]);
  @override
  String toString() => 'ProjectInspectEventAddLogs { projectId: $projectId }';
}

class ProjectInspectEventAddWork extends ProjectInspectEvent {
  final int projectId;
  ProjectInspectEventAddWork({@required this.projectId}) : super([projectId]);
  @override
  String toString() => 'ProjectInspectEventAddWork { projectId: $projectId }';
}

class ProjectInspectEventAddQualityReports extends ProjectInspectEvent {
  final int projectId;
  ProjectInspectEventAddQualityReports({@required this.projectId})
      : super([projectId]);
  @override
  String toString() =>
      'ProjectInspectEventAddQualityReports { projectId: $projectId }';
}

class ProjectInspectEventAddTasks extends ProjectInspectEvent {
  final int projectId;
  ProjectInspectEventAddTasks({@required this.projectId}) : super([projectId]);
  @override
  String toString() => 'ProjectInspectEventAddTasks { projectId: $projectId }';
}

class ProjectInspectEventAddComment extends ProjectInspectEvent {
  final int projectId;
  ProjectInspectEventAddComment({@required this.projectId})
      : super([projectId]);
  @override
  String toString() =>
      'ProjectInspectEventAddComment { projectId: $projectId }';
}

class ProjectInspectEventAddProfileImage extends ProjectInspectEvent {
  final int projectId;
  ProjectInspectEventAddProfileImage({@required this.projectId})
      : super([projectId]);
  @override
  String toString() =>
      'ProjectInspectEventAddProfileImage { projectId: $projectId }';
}

class ProjectInspectEventAddFolder extends ProjectInspectEvent {
  final String title;
  final int projectId;
  ProjectInspectEventAddFolder({@required this.title, @required this.projectId})
      : super([title, projectId]);
  @override
  String toString() =>
      'ProjectInspectEventAddFolder { title: $title, projectId: $projectId }';
}

class ProjectInspectEventAddPost extends ProjectInspectEvent {
  final String title;
  final String body;
  final int projectId;
  ProjectInspectEventAddPost(
      {@required this.title, @required this.body, @required this.projectId})
      : super([title, body, projectId]);
  @override
  String toString() =>
      'ProjectInspectEventAddPost { title: $title, body: $body, projectId: $projectId }';
}
