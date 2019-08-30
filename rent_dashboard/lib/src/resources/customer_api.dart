import 'package:http/http.dart' show Client;
import 'dart:convert';
import '../models/customer.dart';
import 'dart:async';
import 'customer_repository.dart';
import '../network.dart';

class CustomerApi extends CustomerSource {
  Client client = Client();

  Future<List<Customer>> fetchCustomers() async {
    final response = await client.get(
      '${Network.root}/Dashboard/Customers',
      headers: Network.getHeaders(),
    );
    final customers = json.decode(response.body);
    return List.from(customers.map((j) {
      return Customer.fromJson(j);
    }));
  }
}
