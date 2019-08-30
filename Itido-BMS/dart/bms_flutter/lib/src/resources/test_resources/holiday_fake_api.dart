import 'package:bms_dart/models.dart';
import 'dart:async';
import 'package:bms_dart/repositories.dart';
import 'client_faker.dart';

class HolidayFakeApi extends HolidaySource {
  ClientFaker _faker = ClientFaker<Holiday>(
      generator: (i) => Holiday(
            name: 'Juleaften',
            countryCode: 'DK$i',
          ));

  @override
  void dispose() {}

  @override
  Future<List<Holiday>> fetchHolidays(String countryCode) => _faker.getMany();
}
