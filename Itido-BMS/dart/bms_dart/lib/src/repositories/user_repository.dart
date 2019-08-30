import 'dart:async';
import 'package:bms_dart/query_result.dart';
import 'package:bms_dart/src/models/user_login_dto.dart';

import '../models/user.dart';
import 'source.dart';

abstract class UserSource extends Source {
  Future<QueryResult<User>> fetch(int userId);
  Future<QueryResult<List<User>>> fetchAllUsers();
  Future<QueryResult<List<User>>> fetchOfProject(int projectId);
  Future<QueryResult<List<User>>> fetchOfProjectAvailableOnDate(
      int projectId, DateTime date);
  Future<QueryResult<List<User>>> fetchOfNotProject(int projectId);
  Future<QueryResult<bool>> addUsersToProject(int projectId, List<int> userIds);
  Future<QueryResult<bool>> removeUsersFromProject(
      int projectId, List<int> userIds);

  Future<QueryResult<int>> createSystem(
      UserLoginDTO userLoginDTO, String userRole, int projectRoleId);
  Future<QueryResult<int>> createClient(
      UserLoginDTO userLoginDTO, int clientId, int projectRoleId);

  Future<QueryResult<bool>> update(User user, int userId);

  Future<QueryResult<bool>> disable(int userId);
  Future<QueryResult<bool>> enable(int userId);
}

class UserRepository extends UserSource {
  final List<UserSource> sources;

  UserRepository(this.sources);

  Future<QueryResult<User>> fetch(int userId) async {
    var users;
    for (var source in sources) {
      users = await source.fetch(userId);
      if (users != null) {
        break;
      }
    }
    return users;
  }

  Future<QueryResult<List<User>>> fetchAllUsers() async {
    var users;
    for (var source in sources) {
      users = await source.fetchAllUsers();
      if (users != null) {
        break;
      }
    }
    return users;
  }

  Future<QueryResult<List<User>>> fetchOfProject(int projectId) async {
    var users;
    for (var source in sources) {
      users = await source.fetchOfProject(projectId);
      if (users != null) {
        break;
      }
    }
    return users;
  }

  Future<QueryResult<List<User>>> fetchOfProjectAvailableOnDate(
      int projectId, DateTime date) async {
    var users;
    for (var source in sources) {
      users = await source.fetchOfProjectAvailableOnDate(projectId, date);
      if (users != null) {
        break;
      }
    }
    return users;
  }

  Future<QueryResult<List<User>>> fetchOfNotProject(int projectId) async {
    var users;
    for (var source in sources) {
      users = await source.fetchOfNotProject(projectId);
      if (users != null) {
        break;
      }
    }
    return users;
  }

  Future<QueryResult<bool>> addUsersToProject(
          int projectId, List<int> userIds) =>
      sources[0].addUsersToProject(projectId, userIds);
  Future<QueryResult<bool>> removeUsersFromProject(
          int projectId, List<int> userIds) =>
      sources[0].removeUsersFromProject(projectId, userIds);

  Future<QueryResult<int>> createSystem(
          UserLoginDTO userLoginDTO, String userRole, int projectRoleId) =>
      sources[0].createSystem(userLoginDTO, userRole, projectRoleId);
  Future<QueryResult<int>> createClient(
          UserLoginDTO userLoginDTO, int clientId, int projectRoleId) =>
      sources[0].createClient(userLoginDTO, clientId, projectRoleId);

  Future<QueryResult<bool>> update(User user, int userId) =>
      sources[0].update(user, userId);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }

  Future<QueryResult<bool>> disable(int userId) => sources[0].disable(userId);
  Future<QueryResult<bool>> enable(int userId) => sources[0].enable(userId);
}
