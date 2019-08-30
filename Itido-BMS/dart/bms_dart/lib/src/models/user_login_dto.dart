import 'login.dart';
import 'user.dart';

class UserLoginDTO {
  User user;
  Login login;

  UserLoginDTO(this.user, this.login);

  Map<String, dynamic> toMap() => {
        'user': this.user.toMap(),
        'login': this.login.toMap(),
      };
}
