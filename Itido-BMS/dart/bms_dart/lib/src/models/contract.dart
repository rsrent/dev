import 'package:bms_dart/src/models/agreement.dart';

import 'date_time_converter.dart';
import 'user.dart';

class Contract {
  int id;
  double weeklyHours;
  DateTime from;
  DateTime to;
  User user;
  Agreement agreement;
  //String agreementName;

  //User user;

  Contract({
    //this.id,
    this.weeklyHours,
    this.from,
    this.to,
    //this.user,
    //this.agreement,
  });

  factory Contract.fromJson(json, {rootJson, path}) {
    if (json == null) return null;
    return Contract._fromJson(json, rootJson ?? {}, path ?? '');
  }

  Contract._fromJson(json, rootJson, path)
      : this.id = json['id'],
        this.user = User.fromJson(json['user'] ?? rootJson['${path}_user']),
        this.agreement = Agreement.fromJson(
            json['agreement'] ?? rootJson['${path}_agreement']),
        this.weeklyHours = json['weeklyHours'],
        this.from = toDateTime(json['from']),
        this.to = toDateTime(json['to']);

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'weeklyHours': this.weeklyHours,
        'from': this.from?.toString(),
        'to': this.to?.toString(),
      };
}
