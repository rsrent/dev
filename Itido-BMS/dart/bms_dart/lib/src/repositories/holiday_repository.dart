import 'dart:async';
import '../../models.dart';
import 'source.dart';

abstract class HolidaySource extends Source {
  Future<List<Holiday>> fetchHolidays(String countryCode);
}

class HolidayRepository extends HolidaySource {
  final List<HolidaySource> sources;

  HolidayRepository(this.sources);

  Future<List<Holiday>> fetchHolidays(String countryCode) async {
    var users;
    for (var source in sources) {
      users = await source.fetchHolidays(countryCode);
      if (users != null) {
        break;
      }
    }
    return users;
  }

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
