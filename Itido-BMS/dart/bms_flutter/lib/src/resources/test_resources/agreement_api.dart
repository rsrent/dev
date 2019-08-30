import 'package:bms_dart/models.dart';
import 'dart:async';
import 'package:bms_dart/repositories.dart';
import 'client_faker.dart';

class AgreementApi extends AgreementSource {
  ClientFaker<Agreement> _faker = ClientFaker<Agreement>(
    generator: (i) => Agreement(
      id: i,
      name: 'RENT',
    ),
    updateId: (val, i) => val.id = i,
    valueToUpdate: (other, t) => other.id == t.id,
  );

  @override
  void dispose() {}

  @override
  Future<List<Agreement>> fetchAllAgreements() => _faker.getMany();

  @override
  Future<int> createAgreement(Agreement agreement) => _faker.add(agreement);

  @override
  Future<bool> updateAgreement(Agreement agreement) => _faker.update(agreement);
}
