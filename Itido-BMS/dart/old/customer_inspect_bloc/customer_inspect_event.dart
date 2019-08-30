import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class CustomerInspectEvent extends Equatable {
  CustomerInspectEvent([List props = const []]) : super(props);
}

class CustomerInspectEventFetch extends CustomerInspectEvent {
  @override
  String toString() => 'CustomerInspectEventStarted';
}

class CustomerInspectEventLoaded extends CustomerInspectEvent {
  final Customer customer;
  CustomerInspectEventLoaded({@required this.customer}) : super([customer]);
  @override
  String toString() => 'CustomerInspectEventLoaded';
}

class EnableDisableCustomer extends CustomerInspectEvent {
  @override
  String toString() => 'EnableDisableCustomer';
}
