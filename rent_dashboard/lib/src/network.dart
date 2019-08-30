class Network {
  static String token;

  //static final String root = 'https://renttesting.azurewebsites.net/api';
  static final String root = 'https://rentapp.azurewebsites.net/api';
  //static String api = 'https://rentapp.azurewebsites.net';

  static Map<String, String> getHeaders() {
    var map = Map<String, String>();
    map.putIfAbsent("content-type", () => "content-type");
    map["content-type"] = "application/json";

    if (token != null) {
      map.putIfAbsent("Authorization", () => "Authorization");
      map["Authorization"] = 'Bearer $token';
    }

    return map;
  }
}
