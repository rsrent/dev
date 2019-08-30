import 'package:http/http.dart' show Client;
import 'dart:convert';
import '../models/data.dart';
import '../models/news.dart';
import 'dart:async';
import 'data_repository.dart';
import '../network.dart';

class DataApi extends DataSource {
  Client client = Client();

  Future<CompleteData> fetchData(int customerId, int userId, int locationId) async {
    final response = await client.get(
      //'${Network.root}/Dashboard/Data/$customerId/$userId',
      '${Network.root}/Dashboard/Data/$customerId/$userId/$locationId',
      headers: Network.getHeaders(),
    );
    final data = json.decode(response.body);
    return CompleteData.fromJson(data);
  }

  Future<List<News>> fetchNews(int customerId, int userId, int locationId) async {
    final response = await client.get(
      '${Network.root}/Dashboard/News/$customerId/$userId/$locationId',
      headers: Network.getHeaders(),
    );
    print(response.body);
    final news = json.decode(response.body);
    return List.from(news.map((j) {
      return News.fromJson(j);
    }));
  }
}
