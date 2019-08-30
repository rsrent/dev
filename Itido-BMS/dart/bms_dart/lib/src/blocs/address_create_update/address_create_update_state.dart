import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/address.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class AddressCreateUpdateState extends Equatable {
  final Address address;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  AddressCreateUpdateState({
    @required this.address,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          address.toMap(),
          isValid,
          createUpdateStatePhase,
        ]);

  factory AddressCreateUpdateState.createOrCopy(
    dynamic old, {
    Address address,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Address) changes,
  }) {
    AddressCreateUpdateState previous;
    if (old is AddressCreateUpdateState) previous = old;

    var _address = address ?? previous?.address ?? Address();
    var _isValid = isValid ?? previous?.isValid ?? true;
    var _createUpdateStatePhase = createUpdateStatePhase ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;

    if (changes != null) changes(_address);

    return AddressCreateUpdateState(
      address: _address,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'AddressCreateUpdateState { address: ${address.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
