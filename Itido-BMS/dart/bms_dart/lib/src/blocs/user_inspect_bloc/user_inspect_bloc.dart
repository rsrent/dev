import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';
import 'package:bms_dart/repositories.dart';
import './bloc.dart';

class UserInspectBloc extends Bloc<UserInspectEvent, UserInspectState> {
  final UserRepository _userRepository = repositoryProvider.userRepository();

  final userId;

  UserInspectBloc(this.userId);

  @override
  UserInspectState get initialState => InitialUserInspectState();

  @override
  Stream<UserInspectState> mapEventToState(
    UserInspectEvent event,
  ) async* {
    if (event is UserInspectEventFetch) {
      yield LoadingUserInspectState();
      _userRepository.fetch(userId).then((userResult) {
        if (userResult is Ok<User>)
          dispatch(UserInspectEventLoaded(user: userResult.value));
      });
    }

    if (event is UserInspectEventLoaded) {
      if (event.user != null) {
        yield LoadedUserInspectState(user: event.user);
      } else {
        yield ErrorUserInspectState();
      }
    }

    if (event is EnableDisableUser) {
      var oldState = currentState;
      if (oldState is LoadedUserInspectState) {
        yield LoadedUserInspectState(user: oldState.user, loading: true);
        if (oldState.user.disabled) {
          _userRepository.enable(userId).then((success) {
            dispatch(UserInspectEventFetch());
          });
        } else {
          _userRepository.disable(userId).then((success) {
            dispatch(UserInspectEventFetch());
          });
        }
      }
    }
  }
}
