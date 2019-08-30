import 'package:bms_dart/models.dart';
import 'dart:async';
import 'package:bms_dart/repositories.dart';
import 'client_faker.dart';

class CustomerFakeApi extends CustomerSource {
  ClientFaker _faker = ClientFaker<Customer>(
      generator: (i) => Customer(
            id: i,
            name: 'H&M',
          ));

  @override
  void dispose() {}

  @override
  Future<List<Customer>> fetchAllCustomers() => _faker.getMany();

  @override
  Future<Customer> fetch(int customerId) {
    // TODO: implement fetch
    return null;
  }

  @override
  Future<int> create(Customer customer) {
    // TODO: implement create
    return null;
  }

  @override
  Future<bool> update(Customer customer, int customerId) {
    // TODO: implement update
    return null;
  }

  @override
  Future<bool> disable(int customerId) {
    // TODO: implement disable
    return null;
  }

  @override
  Future<bool> enable(int customerId) {
    // TODO: implement enable
    return null;
  }
}
