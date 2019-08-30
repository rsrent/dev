class Address {
  int id;
  String addressName;
  double lat;
  double lon;

  Address({
    this.addressName,
    this.lat,
    this.lon,
  });

  factory Address.fromJson(json) {
    if (json == null) return null;
    return Address._fromJson(json);
  }

  Address._fromJson(json)
      : this.id = json['id'],
        this.addressName = json['addressName'],
        this.lat = json['lat'],
        this.lon = json['lon'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'addressName': this.addressName,
        'lat': this.lat,
        'lon': this.lon,
      };
}
