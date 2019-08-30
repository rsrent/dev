import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class CustomerListEvent extends Equatable {
  CustomerListEvent([List props = const []]) : super(props);
}

class CustomerListFetch extends CustomerListEvent {
  @override
  String toString() => 'CustomerListFetch';
}

class SearchTextUpdated extends CustomerListEvent {
  final String searchText;
  SearchTextUpdated({@required this.searchText}) : super([searchText]);
  @override
  String toString() => 'SearchTextUpdated { searchText: $searchText }';
}

class CustomersFetched extends CustomerListEvent {
  final List<Customer> customers;
  CustomersFetched({@required this.customers}) : super([customers]);
  @override
  String toString() => 'CustomersFetched { customers: ${customers.length} }';
}

class CustomerLongPressed extends CustomerListEvent {
  final Customer customer;
  CustomerLongPressed({@required this.customer}) : super([customer]);
  @override
  String toString() => 'CustomerLongPressed { customer: $customer }';
}

class ToggleSelectable extends CustomerListEvent {
  @override
  String toString() => 'ToggleSelectable';
}

class ClearSelected extends CustomerListEvent {
  @override
  String toString() => 'ClearSelected';
}

class SelectAll extends CustomerListEvent {
  @override
  String toString() => 'SelectAll';
}
