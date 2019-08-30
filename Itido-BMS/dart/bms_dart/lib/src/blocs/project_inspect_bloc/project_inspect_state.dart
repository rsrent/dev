import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class ProjectInspectState extends Equatable {
  ProjectInspectState([List props = const []]) : super(props);
}

class InitialProjectInspectState extends ProjectInspectState {
  @override
  String toString() => 'InitialProjectInspectState';
}

class LoadingProjectInspectState extends ProjectInspectState {
  @override
  String toString() => 'LoadingProjectInspectState';
}

class LoadedProjectInspectState extends ProjectInspectState {
  final Project project;
  final List<ProjectItem> projectItems;
  final bool loading;
  LoadedProjectInspectState(
      {@required this.project,
      @required this.projectItems,
      this.loading = false})
      : super([project, projectItems, loading]);
  @override
  String toString() =>
      'LoadedProjectInspectState { project: ${project?.name} }';
}

class ErrorProjectInspectState extends ProjectInspectState {
  @override
  String toString() => 'ErrorProjectInspectState';
}
