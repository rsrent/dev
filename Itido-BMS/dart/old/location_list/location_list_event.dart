import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class LocationListEvent extends Equatable {
  LocationListEvent([List props = const []]) : super(props);
}

class LocationListFetchAll extends LocationListEvent {
  @override
  String toString() => 'LocationListFetchAll';
}

class LocationListFetchOfCustomer extends LocationListEvent {
  final int customerId;
  LocationListFetchOfCustomer({@required this.customerId})
      : super([customerId]);
  @override
  String toString() =>
      'LocationListFetchOfCustomer { customerId: $customerId }';
}

class LocationListFetchOfUser extends LocationListEvent {
  final int userId;
  LocationListFetchOfUser({@required this.userId}) : super([userId]);
  @override
  String toString() => 'LocationListFetchOfUser { userId: $userId }';
}

class LocationListFetchNotOfUser extends LocationListEvent {
  final int userId;
  LocationListFetchNotOfUser({@required this.userId}) : super([userId]);
  @override
  String toString() => 'LocationListFetchNotOfUser { userId: $userId }';
}

class SearchTextUpdated extends LocationListEvent {
  final String searchText;
  SearchTextUpdated({@required this.searchText}) : super([searchText]);
  @override
  String toString() => 'SearchTextUpdated { searchText: $searchText }';
}

class LocationsFetched extends LocationListEvent {
  final List<Location> locations;
  LocationsFetched({@required this.locations}) : super([locations]);
  @override
  String toString() => 'LocationsFetched { locations: ${locations.length} }';
}

class LocationLongPressed extends LocationListEvent {
  final Location location;
  LocationLongPressed({@required this.location}) : super([location]);
  @override
  String toString() => 'LocationLongPressed { location: $location }';
}

class ToggleSelectable extends LocationListEvent {
  @override
  String toString() => 'ToggleSelectable';
}

class ClearSelected extends LocationListEvent {
  @override
  String toString() => 'ClearSelected';
}

class SelectAll extends LocationListEvent {
  @override
  String toString() => 'SelectAll';
}

class AddUserToSelected extends LocationListEvent {
  final int userId;
  AddUserToSelected({@required this.userId}) : super([userId]);
  @override
  String toString() => 'AddUserToSelected { userId: $userId }';
}

class RemoveUserFromSelected extends LocationListEvent {
  final int userId;
  RemoveUserFromSelected({@required this.userId}) : super([userId]);
  @override
  String toString() => 'RemoveUserFromSelected { userId: $userId }';
}
