import 'dart:async';
import 'customer_api.dart';
import '../models/customer.dart';

class CustomerRepository extends CustomerSource {
  List<CustomerSource> sources = <CustomerSource>[
    CustomerApi(),
  ];

  Future<List<Customer>> fetchCustomers() async {
    var customers;
    for (var source in sources) {
      customers = await source.fetchCustomers();
      if (customers != null) {
        break;
      }
    }
    return customers;
  }
}

abstract class CustomerSource {
  Future<List<Customer>> fetchCustomers();
}
