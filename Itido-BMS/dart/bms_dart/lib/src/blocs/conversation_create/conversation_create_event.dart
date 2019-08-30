import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

import '../../../models.dart';

@immutable
abstract class ConversationCreateEvent extends Equatable {
  ConversationCreateEvent([List props = const []]) : super(props);
}

class Prepare extends ConversationCreateEvent {
  @override
  String toString() => 'Prepare';
}

class UsersLoaded extends ConversationCreateEvent {
  final List<User> users;
  UsersLoaded({@required this.users}) : super([users]);
  @override
  String toString() => 'UsersLoaded { users: ${users.length} }';
}

class SearchTextUpdated extends ConversationCreateEvent {
  final String searchText;
  SearchTextUpdated({@required this.searchText}) : super([searchText]);
  String toString() => 'SearchTextUpdated: { searchText: $searchText }';
}

class UserSelected extends ConversationCreateEvent {
  final User user;

  UserSelected({@required this.user}) : super([user]);
  String toString() =>
      'UserSelected: { user: ${user.firstName} ${user.lastName} }';
}

class UserRemoved extends ConversationCreateEvent {
  final User user;

  UserRemoved({@required this.user}) : super([user]);
  String toString() =>
      'UserRemoved: { user: ${user.firstName} ${user.lastName} }';
}

class CreateConversation extends ConversationCreateEvent {
  @override
  String toString() => 'CreateConversation';
}
