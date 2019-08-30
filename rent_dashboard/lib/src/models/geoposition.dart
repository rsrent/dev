class GeoPosition {
  final double lat;
  final double lon;
  final String name;
  final int serviceLeaderId;

  GeoPosition.fromJson(json)
      : lat = json['lat'],
        lon = json['lon'],
        name = json['name'],
        serviceLeaderId = json['serviceLeaderId'];
}
