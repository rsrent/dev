import 'package:bms_dart/src/models/project_item_access.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/project_item.dart';

@immutable
abstract class ProjectItemListEvent extends Equatable {
  ProjectItemListEvent([List props = const []]) : super(props);
}

class ProjectItemListFetchOfProject extends ProjectItemListEvent {
  final int projectId;
  ProjectItemListFetchOfProject({@required this.projectId})
      : super([projectId]);
  @override
  String toString() => 'ProjectItemListFetchOfProject';
}

class ProjectItemListFetchDetailedOfProject extends ProjectItemListEvent {
  final int projectId;
  ProjectItemListFetchDetailedOfProject({@required this.projectId})
      : super([projectId]);
  @override
  String toString() => 'ProjectItemListFetchDetailedOfProject';
}

class ProjectItemListFetched extends ProjectItemListEvent {
  final List<ProjectItem> projectItems;
  ProjectItemListFetched({@required this.projectItems}) : super([projectItems]);
  @override
  String toString() =>
      'ProjectItemListFetched { projectItems: ${projectItems.length} }';
}

class ProjectItemListUpdateAccessOfItem extends ProjectItemListEvent {
  final int projectItemId;
  final List<ProjectItemAccess> access;
  ProjectItemListUpdateAccessOfItem(
      {@required this.projectItemId, @required this.access})
      : super([projectItemId, access]);
  @override
  String toString() => 'ProjectItemListUpdateAccessOfItem';
}
