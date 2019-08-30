import 'package:bms_dart/models.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'dart:async';
import 'package:bms_dart/repositories.dart';

import 'client_faker.dart';

class AbsenceReasonApi extends AbsenceReasonSource {
  ClientFaker _faker = ClientFaker<AbsenceReason>(
    generator: (i) => AbsenceReason(
      id: i,
      description: 'Test beskrivelse',
      canUserCreate: true,
      canUserRequest: true,
      canManagerCreate: true,
      canManagerRequest: true,
    ),
    updateId: (val, i) => val.id = i,
    valueToUpdate: (other, t) => other.id == t.id,
  );

  @override
  void dispose() {}

  @override
  Future<List<AbsenceReason>> fetchAllAbsenceReasons() => _faker.getMany();

  @override
  Future<int> createAbsenceReason(AbsenceReason absenceReason) async {
    await _faker.add(absenceReason);
    return absenceReason.id;
  }

  @override
  Future<bool> updateAbsenceReason(AbsenceReason absenceReason) =>
      _faker.update(absenceReason);
}
