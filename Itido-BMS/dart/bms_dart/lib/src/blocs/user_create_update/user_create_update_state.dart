import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/blocs/user_create_update/user_create_update_errors.dart';
import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/user.dart';
import '../../models/login.dart';
import '../../models/project_role.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class UserCreateUpdateState extends Equatable {
  final User user;
  final Login login;
  final bool isClientUser;
  final List<String> userRoles;
  final String selectedUserRole;
  final List<ProjectRole> projectRoles;
  final ProjectRole selectedProjectRole;
  final List<Client> clients;
  final Client selectedClient;
  final UserCreateUpdateErrors errors;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  UserCreateUpdateState({
    @required this.errors,
    @required this.user,
    @required this.isClientUser,
    @required this.userRoles,
    @required this.selectedUserRole,
    @required this.projectRoles,
    @required this.selectedProjectRole,
    @required this.clients,
    @required this.selectedClient,
    @required this.login,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          user.toMap(),
          login.toMap(),
          isClientUser,
          isValid,
          createUpdateStatePhase,
          selectedProjectRole?.toMap(),
          selectedUserRole,
          selectedClient?.toMap(),
        ]);

  factory UserCreateUpdateState.createOrCopy(
    dynamic old, {
    User user,
    Login login,
    bool isClientUser,
    List<String> userRoles,
    String selectedUserRole,
    List<ProjectRole> projectRoles,
    ProjectRole selectedProjectRole,
    List<Client> clients,
    Client selectedClient,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(User) userChanges,
    Function(Login) loginChanges,
  }) {
    UserCreateUpdateState previous;
    if (old is UserCreateUpdateState) previous = old;

    var _user = user ?? previous?.user ?? User();
    var _login = login ?? previous?.login ?? Login();
    var _isCreate = isCreate ?? previous?.isCreate ?? false;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = (createUpdateStatePhase ??
            (userChanges != null || loginChanges != null
                ? CreateUpdateStatePhase.InProgress
                : null)) ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;
    var _errors = previous?.errors ?? UserCreateUpdateErrors();

    var _selectedUserRole = selectedUserRole ?? previous?.selectedUserRole;
    var _selectedProjectRole =
        selectedProjectRole ?? previous?.selectedProjectRole;
    var _selectedClient = selectedClient ?? previous?.selectedClient;

    if (userChanges != null) userChanges(_user);
    if (loginChanges != null) loginChanges(_login);

    _errors.userLoginUpdated(
      _user,
      _login,
      _selectedUserRole,
      _selectedProjectRole,
      _selectedClient,
    );

    _isValid = _errors.isValid(_isCreate, isClientUser);

    return UserCreateUpdateState(
      errors: _errors,
      user: _user,
      isClientUser: isClientUser ?? previous?.isClientUser,
      login: _login,
      userRoles: userRoles ?? previous?.userRoles,
      selectedUserRole: _selectedUserRole,
      projectRoles: projectRoles ?? previous?.projectRoles,
      selectedProjectRole: _selectedProjectRole,
      clients: clients ?? previous?.clients,
      selectedClient: _selectedClient,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'UserCreateUpdateState { user: ${user.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
