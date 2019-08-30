import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class CustomerInspectState extends Equatable {
  CustomerInspectState([List props = const []]) : super(props);
}

class InitialCustomerInspectState extends CustomerInspectState {
  @override
  String toString() => 'InitialCustomerInspectState';
}

class LoadingCustomerInspectState extends CustomerInspectState {
  @override
  String toString() => 'LoadingCustomerInspectState';
}

class LoadedCustomerInspectState extends CustomerInspectState {
  final Customer customer;
  final bool loading;
  LoadedCustomerInspectState({@required this.customer, this.loading = false})
      : super([customer, loading]);
  @override
  String toString() =>
      'LoadedCustomerInspectState { customer: ${customer?.displayName} }';
}

class ErrorCustomerInspectState extends CustomerInspectState {
  @override
  String toString() => 'ErrorCustomerInspectState';
}
