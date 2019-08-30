import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/models/user_login_dto.dart';
import 'package:http/http.dart' as http show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'query_client_controller.dart';

class UserApi extends UserSource {
  QueryClientController<User> _client;

  String path = '${api.path}/api/Users';

  UserApi({
    http.Client client,
  }) {
    _client = QueryClientController(
        converter: (json) => User.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<QueryResult<User>> fetch(int userId) {
    return _client.get(
      '$path/$userId',
    );
  }

  @override
  Future<QueryResult<List<User>>> fetchAllUsers() {
    return _client.getMany(
      path,
    );
  }

  @override
  Future<QueryResult<List<User>>> fetchAllOfLocation(int locationId) {
    return _client.getMany(
      '$path/GetLocationUsers/$locationId',
    );
  }

  @override
  Future<QueryResult<List<User>>> fetchOfProject(int projectId) {
    return _client.getMany(
      '$path/GetOfProject/$projectId',
    );
  }

  @override
  Future<QueryResult<List<User>>> fetchOfProjectAvailableOnDate(
      int projectId, DateTime date) {
    return _client.getMany(
      '$path/GetOfProjectAvailableOnDate/$projectId/$date',
    );
  }

  @override
  Future<QueryResult<List<User>>> fetchOfNotProject(int projectId) {
    return _client.getMany(
      '$path/GetOfNotProject/$projectId',
    );
  }

  @override
  Future<QueryResult<bool>> addUsersToProject(
      int projectId, List<int> userIds) {
    return _client.put(
      '$path/AddUsersToProject/$projectId',
      body: userIds,
    );
  }

  @override
  Future<QueryResult<bool>> removeUsersFromProject(
      int projectId, List<int> userIds) {
    return _client.put(
      '$path/RemoveUsersFromProject/$projectId',
      body: userIds,
    );
  }

  @override
  Future<QueryResult<int>> createSystem(
      UserLoginDTO userLoginDTO, String userRole, int projectRoleId) async {
    return _client.postId(
      '${api.path}/api/Logins/CreateSystem/$userRole/$projectRoleId',
      body: userLoginDTO.toMap(),
    );
  }

  @override
  Future<QueryResult<int>> createClient(
      UserLoginDTO userLoginDTO, int clientId, int projectRoleId) async {
    return _client.postId(
      '${api.path}/api/Logins/CreateClient/$clientId/$projectRoleId',
      body: userLoginDTO.toMap(),
    );
  }

  @override
  Future<QueryResult<bool>> update(User user, int userId) async {
    return _client.put(
      '$path/Update/$userId',
      body: user.toMap(),
    );
  }

  @override
  Future<QueryResult<bool>> enable(int id) => _client.put('$path/Enable/$id');
  @override
  Future<QueryResult<bool>> disable(int id) => _client.put('$path/Disable/$id');
}
