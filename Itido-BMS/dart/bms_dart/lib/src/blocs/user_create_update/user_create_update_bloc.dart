import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/sprog.dart';
import 'package:bms_dart/src/blocs/dispatch_query_result.dart';
import 'package:bms_dart/src/models/user_login_dto.dart';
import 'package:flutter/material.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class UserCreateUpdateBloc
    extends Bloc<UserCreateUpdateEvent, UserCreateUpdateState>
    with DispatchQueryResult {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final UserRepository _userRepository = repositoryProvider.userRepository();
  final ProjectRoleRepository _projectRoleRepository =
      repositoryProvider.projectRoleRepository();
  final ClientRepository _clientRepository =
      repositoryProvider.clientRepository();

  final int customerId;

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  UserCreateUpdateBloc(
      {this.customerId, @required this.queryResultBloc, @required this.sprog});

  @override
  UserCreateUpdateState get initialState =>
      UserCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<UserCreateUpdateState> mapEventToState(
    UserCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      var projectRoles = await _projectRoleRepository.fetchProjectRoles();
      var clients = await _clientRepository.fetchClients();
      var userRoles = ['Admin', 'Manager', 'User', 'Client'];
      yield UserCreateUpdateState.createOrCopy(
        null,
        user: User(userRole: 'User', disabled: false),
        login: Login(),
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: true,
        projectRoles: projectRoles,
        userRoles: userRoles,
        clients: clients,
        isClientUser: false,
      );
      yield UserCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }
    if (event is PrepareUpdate) {
      var projectRoles = await _projectRoleRepository.fetchProjectRoles();
      var userRoles = ['Admin', 'Manager', 'User', 'Client'];
      var userResult = await _userRepository.fetch(event.user.id);
      if (userResult is Ok<User>) {
        yield UserCreateUpdateState.createOrCopy(
          null,
          user: userResult.value,
          createUpdateStatePhase: CreateUpdateStatePhase.Initial,
          isCreate: false,
          projectRoles: projectRoles,
          userRoles: userRoles,
        );
        yield UserCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
      } else {
        dispatchQueryResult(userResult);
        yield UserCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }

    if (event is UserNameChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          loginChanges: (login) => login.userName = event.text);
    if (event is PasswordChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          loginChanges: (login) => login.password = event.text);

    if (event is FirstNameChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          userChanges: (user) => user.firstName = event.text);
    if (event is LastNameChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          userChanges: (user) => user.lastName = event.text);
    if (event is EmailChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          userChanges: (user) => user.email = event.text);
    if (event is PhoneChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          userChanges: (user) => user.phone = event.text);
    if (event is CommentChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          userChanges: (user) => user.comment = event.text);
    if (event is EmployeeNumberChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          userChanges: (user) => user.employeeNumber =
              event.text != '' ? int.parse(event.text) : null);
    if (event is UserRoleChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          selectedUserRole: event.text,
          userChanges: (user) => user.userRole = event.text);
    if (event is ProjectRoleChanged) {
      print(event.projectRole.id);
      print(event.projectRole.toMap());
      yield UserCreateUpdateState.createOrCopy(currentState,
          selectedProjectRole: event.projectRole);
    }
    if (event is LanguageCodeChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          userChanges: (user) => user.languageCode = event.text);
    if (event is ClientChanged)
      yield UserCreateUpdateState.createOrCopy(currentState,
          selectedClient: event.client);
    if (event is IsClientChanged) {
      yield UserCreateUpdateState.createOrCopy(currentState,
          isClientUser: event.isOn);
    }

    if (event is Commit) {
      var newState = UserCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      QueryResult commitResult;
      if (newState.isCreate) {
        if (newState.isClientUser) {
          commitResult = await _userRepository.createClient(
              UserLoginDTO(newState.user, newState.login),
              newState.selectedClient.id,
              newState.selectedProjectRole.id);
        } else {
          commitResult = await _userRepository.createSystem(
              UserLoginDTO(newState.user, newState.login),
              newState.selectedUserRole,
              newState.selectedProjectRole.id);
        }
        dispatchQueryResult(commitResult, sprog().createAttempted);
        // commitResult = (await _userRepository.createSystem(
        //         UserLoginDTO(newState.user, newState.login), customerId)) !=
        //     null;
      } else {
        commitResult =
            await _userRepository.update(newState.user, newState.user.id);
        dispatchQueryResult(commitResult, sprog().updateAttempted);
      }
      if (commitResult.successful) {
        yield UserCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield UserCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
