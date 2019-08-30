import 'package:bms_dart/src/blocs/customer_create_update/customer_create_update_errors.dart';
import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/customer.dart';
import '../../models/login.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class CustomerCreateUpdateState extends Equatable {
  final Customer customer;
  final CustomerCreateUpdateErrors errors;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  CustomerCreateUpdateState({
    @required this.errors,
    @required this.customer,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          customer.toMap(),
          isValid,
          createUpdateStatePhase,
        ]);

  factory CustomerCreateUpdateState.createOrCopy(
    dynamic old, {
    Customer customer,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Customer) customerChanges,
  }) {
    CustomerCreateUpdateState previous;
    if (old is CustomerCreateUpdateState) previous = old;

    var _customer = customer ?? previous?.customer ?? Customer();
    var _isCreate = isCreate ?? previous?.isCreate ?? false;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = (createUpdateStatePhase ??
            (customerChanges != null
                ? CreateUpdateStatePhase.InProgress
                : null)) ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;
    var _errors = previous?.errors ?? CustomerCreateUpdateErrors();

    if (customerChanges != null) customerChanges(_customer);

    _errors.customerLoginUpdated(_customer);

    _isValid = _errors.isValid(_isCreate);

    return CustomerCreateUpdateState(
      errors: _errors,
      customer: _customer,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'CustomerCreateUpdateState { customer: ${customer.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
