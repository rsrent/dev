import 'package:bms_dart/models.dart';
import 'dart:async';
import 'package:bms_dart/repositories.dart';

import 'client_faker.dart';

class ContractApi extends ContractSource {
  ClientFaker _faker = ClientFaker<Contract>(
      generator: (i) => Contract(
            weeklyHours: (i % 20).toDouble(),
            from: DateTime.now(),
            to: DateTime.now(),
          )
            ..user = User(firstName: 'Tobias')
            ..agreement = Agreement(name: 'RENT'));

  @override
  Future<List<Contract>> fetchContractsOfUser(int userId) => _faker.getMany();

  @override
  Future<bool> createContract(
      Contract contract, int userId, int agreementId) async {
    await _faker.add(contract);
    return true;
  }

  @override
  Future<bool> updateContract(Contract contract) => _faker.update(contract);

  @override
  void dispose() {
    // TODO: implement dispose
  }

  @override
  Future<Contract> fetchContract(int id) {
    // TODO: implement fetchContract
    return null;
  }
}
