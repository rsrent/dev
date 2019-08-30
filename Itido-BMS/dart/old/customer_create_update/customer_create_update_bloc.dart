import 'dart:async';
import 'package:bloc/bloc.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class CustomerCreateUpdateBloc
    extends Bloc<CustomerCreateUpdateEvent, CustomerCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final CustomerRepository _customerRepository =
      repositoryProvider.customerRepository();

  @override
  CustomerCreateUpdateState get initialState =>
      CustomerCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<CustomerCreateUpdateState> mapEventToState(
    CustomerCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      yield CustomerCreateUpdateState.createOrCopy(
        null,
        customer: Customer(
          disabled: false,
        ),
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: true,
      );
      yield CustomerCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }
    if (event is PrepareUpdate) {
      var customer = await _customerRepository.fetch(event.customer.id);
      yield CustomerCreateUpdateState.createOrCopy(
        null,
        customer: customer,
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: false,
      );
      yield CustomerCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is NameChanged)
      yield CustomerCreateUpdateState.createOrCopy(currentState,
          customerChanges: (customer) => customer.name = event.text);
    if (event is CommentChanged)
      yield CustomerCreateUpdateState.createOrCopy(currentState,
          customerChanges: (customer) => customer.comment = event.text);
    if (event is HasStandardFoldersChanged)
      yield CustomerCreateUpdateState.createOrCopy(currentState,
          customerChanges: (customer) =>
              customer.hasStandardFolders = event.isTrue);
    if (event is CanReadLogsChanged)
      yield CustomerCreateUpdateState.createOrCopy(currentState,
          customerChanges: (customer) => customer.canReadLogs = event.isTrue);

    if (event is Commit) {
      var newState = CustomerCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result;
      if (newState.isCreate) {
        result = (await _customerRepository.create(newState.customer)) != null;
      } else {
        result = await _customerRepository.update(
            newState.customer, newState.customer.id);
      }
      print('reslt: $result');
      if (result) {
        yield CustomerCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield CustomerCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
