import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

import '../../../models.dart';

@immutable
abstract class ConversationCreateState extends Equatable {
  ConversationCreateState([List props = const []]) : super(props);
}

class Initial extends ConversationCreateState {
  @override
  String toString() => 'Initial';
}

class Loading extends ConversationCreateState {
  @override
  String toString() => 'Loading';
}

class ConversationInCreation extends ConversationCreateState {
  final List<User> loadedUsers;
  final String searchText;
  final List<User> searchedUsers;
  final List<User> selectedUsers;

  ConversationInCreation({
    @required this.loadedUsers,
    @required this.searchText,
    @required this.searchedUsers,
    @required this.selectedUsers,
  }) : super([
          loadedUsers,
          searchedUsers,
          searchText,
          selectedUsers,
        ]);

  factory ConversationInCreation.createOrCopy(
    dynamic old, {
    List<User> loadedUsers,
    String searchText,
    List<User> searchedUsers,
    List<User> selectedUsers,
  }) {
    ConversationInCreation previous;
    if (old is ConversationInCreation) previous = old;

    var _loadedUsers = loadedUsers ?? previous?.loadedUsers ?? [];
    var _searchText = (searchText ?? previous?.searchText ?? '').toLowerCase();
    var _selectedUsers = selectedUsers ?? previous?.selectedUsers ?? [];

    var _searchedUsers = _loadedUsers
        .where((u) => '${u.firstName ?? ''} ${u.lastName ?? ''}'
            .toLowerCase()
            .contains(_searchText))
        .where((u) => !_selectedUsers.any((su) => su.id == u.id))
        .toList();

    return ConversationInCreation(
      loadedUsers: _loadedUsers,
      searchText: _searchText,
      searchedUsers: _searchedUsers,
      selectedUsers: _selectedUsers,
    );
  }

  @override
  String toString() =>
      'ConversationInCreation { loadedUsers: ${loadedUsers.length}, searchText: $searchText, selectedUsers: ${selectedUsers.length},  searchedUsers: ${searchedUsers.length}, selectedUsers: ${selectedUsers.length} }';
}

class ConversationCreated extends ConversationCreateState {
  final String id;

  ConversationCreated({@required this.id}) : super([id]);

  @override
  String toString() => 'ConversationCreated: $id';
}
