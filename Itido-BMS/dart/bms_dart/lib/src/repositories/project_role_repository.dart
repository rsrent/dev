import 'dart:async';
import '../models/project_role.dart';
import 'source.dart';

abstract class ProjectRoleSource extends Source {
  Future<List<ProjectRole>> fetchProjectRoles();
}

class ProjectRoleRepository extends ProjectRoleSource {
  final List<ProjectRoleSource> sources;

  ProjectRoleRepository(this.sources);

  Future<List<ProjectRole>> fetchProjectRoles() async {
    var values;
    for (var source in sources) {
      values = await source.fetchProjectRoles();
      if (values != null) {
        break;
      }
    }
    return values;
  }

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
