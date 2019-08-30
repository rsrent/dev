import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class ProjectRoleApi extends ProjectRoleSource {
  //Client _client;

  ClientController<ProjectRole> _client;
  String path = '${api.path}/api/ProjectRole';

  ProjectRoleApi({
    http.Client client,
  }) {
    _client = ClientController<ProjectRole>(
        converter: (json) => ProjectRole.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<ProjectRole>> fetchProjectRoles() {
    return _client.getMany('$path/GetRoles', headers: api.headers());
  }
}
