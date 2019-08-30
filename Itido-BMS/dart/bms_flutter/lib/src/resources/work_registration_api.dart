import 'package:bms_dart/models.dart';
import 'package:http/http.dart' as http show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'query_client_controller.dart';

class WorkRegistrationApi extends WorkRegistrationSource {
  QueryClientController<WorkRegistration> _client;

  String path = '${api.path}/api/Work';

  WorkRegistrationApi({
    http.Client client,
  }) {
    _client = QueryClientController(
      converter: (json) => WorkRegistration.fromJson(json),
      client: client,
      getHeaders: () => api.headers(),
    );
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<QueryResult<bool>> register(int workId,
      {int startTimeMins, int endTimeMins}) {
    return _client.postNoContent(
      '$path/RegisterWork/$workId' +
          (startTimeMins != null ? '/$startTimeMins/$endTimeMins' : ''),
    );
  }
}
