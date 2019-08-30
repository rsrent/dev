import 'flutter_repository_provider.dart';

class Api {
  String path = 'https://renttesting.azurewebsites.net';
  // String path = 'https://localhost:5001';

  var map = Map<String, String>();

  Map<String, String> headers() {
    map.putIfAbsent("content-type", () => "content-type");
    map["content-type"] = "application/json";

    if (repositoryProvider.authenticationRepository().getToken() != null) {
      map.putIfAbsent("Authorization", () => "Authorization");
      map["Authorization"] =
          'Bearer ${repositoryProvider.authenticationRepository().getToken()}';
    } else {
      map.remove('Authorization');
    }

    return map;
  }
}

Api api;
