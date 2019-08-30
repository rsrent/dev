//enum ErrorState

import '../../../models.dart';
import '../../validators/validators.dart';

class UserCreateUpdateErrors {
  bool usernameValid = false;
  bool passwordValid = false;
  bool emailValid = false;
  bool userRoleCorrect = false;
  bool projectRoleCorrect = false;
  bool clientCorrect = false;

  void userLoginUpdated(
    User user,
    Login login,
    String userRole,
    ProjectRole projectRole,
    Client client,
  ) {
    usernameValid = validateUsername(login?.userName);
    passwordValid = validatePassword(login?.password);
    emailValid = validateEmail(user?.email);

    print('user?.email: ${user?.email}');
    print('emailValid: $emailValid');
    userRoleCorrect = userRole != null;
    projectRoleCorrect = projectRole != null;
    clientCorrect = client != null;
  }

  bool isValid(bool isCreate, bool isClient) => (!isCreate ||
      (usernameValid &&
          passwordValid &&
          projectRoleCorrect &&
          ((isClient && clientCorrect) ||
              (!isClient && emailValid && userRoleCorrect))));
}
