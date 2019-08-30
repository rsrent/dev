import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/project.dart';

@immutable
abstract class ProjectListEvent extends Equatable {
  ProjectListEvent([List props = const []]) : super(props);
}

class ProjectListFetchOfProject extends ProjectListEvent {
  final int projectId;
  ProjectListFetchOfProject({@required this.projectId}) : super([projectId]);
  @override
  String toString() => 'ProjectListFetchOfProject';
}

class ProjectListFetchOfUser extends ProjectListEvent {
  final int userId;
  ProjectListFetchOfUser({@required this.userId}) : super([userId]);
  @override
  String toString() => 'ProjectListFetchOfUser';
}

class ProjectListFetchNotOfUser extends ProjectListEvent {
  final int userId;
  ProjectListFetchNotOfUser({@required this.userId}) : super([userId]);
  @override
  String toString() => 'ProjectListFetchOfUser';
}

class ProjectListFetched extends ProjectListEvent {
  final List<Project> projects;
  ProjectListFetched({@required this.projects}) : super([projects]);
  @override
  String toString() => 'ProjectListFetched { projects: ${projects.length} }';
}

class ProjectListAddNew extends ProjectListEvent {
  final int projectId;
  final String name;
  ProjectListAddNew({@required this.projectId, @required this.name})
      : super([projectId, name]);
  @override
  String toString() => 'ProjectListAddNew';
}

class ProjectLongPressed extends ProjectListEvent {
  final Project project;
  ProjectLongPressed({@required this.project}) : super([project]);
  @override
  String toString() => 'ProjectLongPressed { project: $project }';
}

class SearchTextUpdated extends ProjectListEvent {
  final String searchText;
  SearchTextUpdated({@required this.searchText}) : super([searchText]);
  @override
  String toString() => 'SearchTextUpdated { searchText: $searchText }';
}

class ToggleSelectable extends ProjectListEvent {
  @override
  String toString() => 'ToggleSelectable';
}

class ClearSelected extends ProjectListEvent {
  @override
  String toString() => 'ClearSelected';
}

class SelectAll extends ProjectListEvent {
  @override
  String toString() => 'SelectAll';
}

class AddUserToSelected extends ProjectListEvent {
  final int userId;
  AddUserToSelected({@required this.userId}) : super([userId]);
  @override
  String toString() => 'AddUserToSelected { userId: $userId }';
}

class RemoveUserFromSelected extends ProjectListEvent {
  final int userId;
  RemoveUserFromSelected({@required this.userId}) : super([userId]);
  @override
  String toString() => 'RemoveUserFromSelected { userId: $userId }';
}
