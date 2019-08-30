import './data.dart';

class User {
  User({User u, ServiceLeader sl, json}) {
    if (json != null) {
      id = json['id'];
      name = json['name'];
    } else if (u != null) {
      id = u.id;
      name = u.name;
    } else if (sl != null) {
      id = sl.id;
      name = sl.name;
    } else {
      instanciated = false;
    }
  }
  int id;
  String name;
  bool instanciated = true;

  User.fromJson(json)
      : id = json['id'],
        name = json['name'];
}

class ServiceLeader extends User {
  ServiceLeader({User u, ServiceLeader sl, json})
      : super(u: u, sl: sl, json: json) {
    if (json != null) {
      id = json['id'];
      name = json['name'];
      smallData = SimpleData(json);
    }
  }

  ServiceLeader.fromJson(json)
      : smallData = SimpleData(json),
        super.fromJson(json);

  SimpleData smallData;
}
