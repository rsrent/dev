class WorkHoliday {
  int workContractid;
  String holidayName;
  String holidayCountryCode;

  WorkHoliday({
    this.holidayName,
    this.holidayCountryCode,
  });

  factory WorkHoliday.fromJson(json) {
    if (json == null) return null;
    return WorkHoliday._fromJson(json);
  }

  WorkHoliday._fromJson(json)
      : this.workContractid = json['workContractid'],
        this.holidayName = json['holidayName'],
        this.holidayCountryCode = json['holidayCountryCode'];

  Map<String, dynamic> toMap() => {
        'workContractid': this.workContractid,
        'holidayName': this.holidayName,
        'holidayCountryCode': this.holidayCountryCode,
      };
}
