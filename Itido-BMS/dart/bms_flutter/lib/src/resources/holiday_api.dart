import 'package:bms_dart/models.dart';
import 'package:http/http.dart' as http show Client;
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'client_controller.dart';

class HolidayApi extends HolidaySource {
  ClientController<Holiday> _client;

  String path = '${api.path}/api/Holiday';

  HolidayApi({
    http.Client client,
  }) {
    _client = ClientController(
        converter: (json) => Holiday.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<Holiday>> fetchHolidays(String countryCode) {
    return _client.getMany(
      '$path/GetAllOfCountryCode/$countryCode',
      headers: api.headers(),
    );
  }
}
