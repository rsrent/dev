class Holiday {
  String name;
  String countryCode;

  bool enabled;

  Holiday({
    this.name,
    this.countryCode,
    this.enabled = true,
  });

  Holiday.fromJson(json)
      : this.name = json['name'],
        this.countryCode = json['countryCode'];

  Map<String, dynamic> toMap() => {
        'name': this.name,
        'countryCode': this.countryCode,
      };
}
