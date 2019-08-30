import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class UserListEvent extends Equatable {
  UserListEvent([List props = const []]) : super(props);
}

class FetchAll extends UserListEvent {
  @override
  String toString() => 'FetchAll';
}

class FetchOfProject extends UserListEvent {
  final int projectId;
  FetchOfProject({@required this.projectId}) : super([projectId]);
  @override
  String toString() => 'FetchOfProject { projectId: $projectId }';
}

class FetchOfProjectAvailableOnDate extends UserListEvent {
  final int projectId;
  final DateTime date;
  FetchOfProjectAvailableOnDate({@required this.projectId, @required this.date})
      : super([projectId, date]);
  @override
  String toString() =>
      'FetchOfProjectAvailableOnDate { projectId: $projectId }';
}

class FetchOfNotProject extends UserListEvent {
  final int projectId;
  FetchOfNotProject({@required this.projectId}) : super([projectId]);
  @override
  String toString() => 'FetchOfNotProject { projectId: $projectId }';
}

class UsersFetched extends UserListEvent {
  final List<User> users;
  UsersFetched({@required this.users}) : super([users]);
  @override
  String toString() => 'UsersFetched { users: ${users.length} }';
}

class SearchTextUpdated extends UserListEvent {
  final String searchText;
  SearchTextUpdated({@required this.searchText}) : super([searchText]);
  @override
  String toString() => 'SearchTextUpdated { searchText: $searchText }';
}

class UserLongPressed extends UserListEvent {
  final User user;
  UserLongPressed({@required this.user}) : super([user]);
  @override
  String toString() => 'UserLongPressed { user: $user }';
}

class ToggleSelectable extends UserListEvent {
  @override
  String toString() => 'ToggleSelectable';
}

class ClearSelected extends UserListEvent {
  @override
  String toString() => 'ClearSelected';
}

class SelectAll extends UserListEvent {
  @override
  String toString() => 'SelectAll';
}

class AddSelectedToProject extends UserListEvent {
  final int projectId;
  AddSelectedToProject({@required this.projectId}) : super([projectId]);
  @override
  String toString() => 'AddSelectedToProject';
}

class RemoveSelectedFromProject extends UserListEvent {
  final int projectId;
  RemoveSelectedFromProject({@required this.projectId}) : super([projectId]);
  @override
  String toString() => 'RemoveSelectedFromProject';
}
