import 'dart:async';
import '../models/customer.dart';
import 'source.dart';

abstract class CustomerSource extends Source {
  Future<Customer> fetch(int customerId);
  Future<List<Customer>> fetchAllCustomers();

  Future<int> create(Customer customer);
  Future<bool> update(Customer customer, int customerId);

  Future<bool> disable(int customerId);
  Future<bool> enable(int customerId);
}

class CustomerRepository extends CustomerSource {
  final List<CustomerSource> sources;

  CustomerRepository(this.sources);

  Future<Customer> fetch(int customerId) async {
    var customer;
    for (var source in sources) {
      customer = await source.fetch(customerId);
      if (customer != null) {
        break;
      }
    }
    return customer;
  }

  Future<List<Customer>> fetchAllCustomers() async {
    var customers;
    for (var source in sources) {
      customers = await source.fetchAllCustomers();
      if (customers != null) {
        break;
      }
    }
    return customers;
  }

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }

  @override
  Future<int> create(Customer customer) => sources[0].create(customer);

  @override
  Future<bool> update(Customer customer, int customerId) =>
      sources[0].update(customer, customerId);

  Future<bool> disable(int customerId) => sources[0].disable(customerId);
  Future<bool> enable(int customerId) => sources[0].enable(customerId);
}
