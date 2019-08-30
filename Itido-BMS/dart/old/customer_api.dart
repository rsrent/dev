import 'package:bms_dart/models.dart';
import 'package:http/http.dart' show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'client_controller.dart';

class CustomerApi extends CustomerSource {
  ClientController<Customer> _client;

  String path = '${api.path}/api/Customers';

  CustomerApi({
    Client client,
  }) {
    _client = ClientController(
        converter: (json) => Customer.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<Customer> fetch(int customerId) {
    return _client.get(
      '$path/$customerId',
      headers: api.headers(),
    );
  }

  @override
  Future<List<Customer>> fetchAllCustomers() {
    return _client.getMany(
      path,
      headers: api.headers(),
    );
  }

  @override
  Future<int> create(Customer customer) {
    return _client.postId(
      '$path/Create',
      body: customer.toMap(),
    );
  }

  @override
  Future<bool> update(Customer customer, int customerId) {
    return _client.put(
      '$path/Update/$customerId',
      body: customer.toMap(),
    );
  }

  @override
  Future<bool> enable(int id) => _client.put('$path/Enable/$id');
  @override
  Future<bool> disable(int id) => _client.put('$path/Disable/$id');
}
