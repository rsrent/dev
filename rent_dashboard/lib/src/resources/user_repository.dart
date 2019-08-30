import 'dart:async';
import '../models/user.dart';
import 'user_api.dart';

class UserRepository extends UserSource {
  List<UserSource> sources = <UserSource>[
    UserApi(),
  ];

  Future<List<ServiceLeader>> fetchServiceLeaders() async {
    var users;
    for (var source in sources) {
      users = await source.fetchServiceLeaders();
      if (users != null) {
        break;
      }
    }
    return users;
  }
}

abstract class UserSource {
  Future<List<ServiceLeader>> fetchServiceLeaders();
}
