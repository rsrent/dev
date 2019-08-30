import 'dart:async';
import '../models/data.dart';
import '../models/news.dart';
import 'data_api.dart';

class DataRepository extends DataSource {
  List<DataSource> sources = <DataSource>[
    DataApi(),
  ];


  Future<CompleteData> fetchData(int customerId, int userId, int locationId) async {
    var data;
    for (var source in sources) {
      data = await source.fetchData(customerId, userId, locationId);
      if (data != null) {
        break;
      }
    }
    return data;
  }

  Future<List<News>> fetchNews(int customerId, int userId, int locationId) async {
    var data;
    for (var source in sources) {
      data = await source.fetchNews(customerId, userId, locationId);
      if (data != null) {
        break;
      }
    }
    return data;
  }
}

abstract class DataSource {
  Future<CompleteData> fetchData(int customerId, int userId, int locationId);
  Future<List<News>> fetchNews(int customerId, int userId, int locationId);
}