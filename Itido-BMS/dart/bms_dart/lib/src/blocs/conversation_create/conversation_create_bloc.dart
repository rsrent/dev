import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/src/repositories/user_repository.dart';
import 'package:dart_packages/streamer.dart';
import './bloc.dart';

class ConversationCreateBloc
    extends Bloc<ConversationCreateEvent, ConversationCreateState> {
  AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  UserRepository _userRepository = repositoryProvider.userRepository();
  ConversationRepository _conversationRepository =
      repositoryProvider.conversationRepository();

  Streamer<String> searchText = Streamer();

  @override
  ConversationCreateState get initialState => Initial();

  @override
  Stream<ConversationCreateState> mapEventToState(
    ConversationCreateEvent event,
  ) async* {
    if (event is Prepare) {
      yield Loading();
      _userRepository.fetchAllUsers().then((usersResult) {
        if (usersResult is Ok<List<User>>) {
          dispatch(UsersLoaded(users: usersResult.value));
        }
      });
    }

    if (event is UsersLoaded) {
      yield ConversationInCreation.createOrCopy(currentState,
          loadedUsers: event.users);
    }

    if (event is SearchTextUpdated) {
      yield ConversationInCreation.createOrCopy(currentState,
          searchText: event.searchText);
    }

    if (event is UserSelected) {
      var oldState = currentState;
      var selectedUsers = [];
      if (oldState is ConversationInCreation) {
        selectedUsers = oldState.selectedUsers;
      }
      selectedUsers.add(event.user);
      yield ConversationInCreation.createOrCopy(currentState,
          selectedUsers: selectedUsers, searchText: '');
    }
    if (event is UserRemoved) {
      var oldState = currentState;
      var selectedUsers = [];
      if (oldState is ConversationInCreation) {
        selectedUsers = oldState.selectedUsers;
        selectedUsers.remove(event.user);
      }
      yield ConversationInCreation.createOrCopy(currentState,
          selectedUsers: selectedUsers);
    }

    if (event is CreateConversation) {
      var oldState = currentState;

      if (oldState is ConversationInCreation) {
        yield Loading();
        var success = await _conversationRepository.createConversationWithUsers(
            oldState.selectedUsers.map<int>((u) => u.id).toList()
              ..add(_authenticationRepository.getUserId()));

        if (success != null) {
          yield ConversationCreated(id: success);
          print('Conversation created!!! $success');
        } else {
          print('Error :-(');
        }
      }
    }
  }
}
