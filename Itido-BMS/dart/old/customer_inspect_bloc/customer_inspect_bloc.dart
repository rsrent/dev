import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/repositories.dart';
import './bloc.dart';

class CustomerInspectBloc
    extends Bloc<CustomerInspectEvent, CustomerInspectState> {
  final CustomerRepository _customerRepository =
      repositoryProvider.customerRepository();

  final customerId;

  CustomerInspectBloc(this.customerId);

  @override
  CustomerInspectState get initialState => InitialCustomerInspectState();

  @override
  Stream<CustomerInspectState> mapEventToState(
    CustomerInspectEvent event,
  ) async* {
    if (event is CustomerInspectEventFetch) {
      yield LoadingCustomerInspectState();
      _customerRepository.fetch(customerId).then((customer) {
        dispatch(CustomerInspectEventLoaded(customer: customer));
      });
    }

    if (event is CustomerInspectEventLoaded) {
      if (event.customer != null) {
        yield LoadedCustomerInspectState(customer: event.customer);
      } else {
        yield ErrorCustomerInspectState();
      }
    }

    if (event is EnableDisableCustomer) {
      var oldState = currentState;
      if (oldState is LoadedCustomerInspectState) {
        yield LoadedCustomerInspectState(
            customer: oldState.customer, loading: true);
        if (oldState.customer.disabled) {
          _customerRepository.enable(customerId).then((success) {
            dispatch(CustomerInspectEventFetch());
          });
        } else {
          _customerRepository.disable(customerId).then((success) {
            dispatch(CustomerInspectEventFetch());
          });
        }
      }
    }
  }
}
