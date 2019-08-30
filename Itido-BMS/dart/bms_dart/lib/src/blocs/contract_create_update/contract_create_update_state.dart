import 'package:bms_dart/src/blocs/contract_create_update/contract_create_update_errors.dart';
import 'package:bms_dart/src/models/accident_report.dart';
import 'package:bms_dart/src/models/agreement.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/contract.dart';
import '../../models/login.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class ContractCreateUpdateState extends Equatable {
  final Contract contract;
  final Agreement selectedAgreement;
  final List<Agreement> allAgreements;
  final ContractCreateUpdateErrors errors;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  ContractCreateUpdateState({
    @required this.errors,
    @required this.contract,
    @required this.selectedAgreement,
    @required this.allAgreements,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          contract.toMap(),
          selectedAgreement?.toMap(),
          allAgreements?.length,
          isValid,
          createUpdateStatePhase,
        ]);

  factory ContractCreateUpdateState.createOrCopy(
    dynamic old, {
    Contract contract,
    Agreement selectedAgreement,
    List<Agreement> allAgreements,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Contract) contractChanges,
  }) {
    ContractCreateUpdateState previous;
    if (old is ContractCreateUpdateState) previous = old;

    var _contract = contract ?? previous?.contract ?? Contract();
    var _selectedAgreement = selectedAgreement ?? previous?.selectedAgreement;
    var _allAgreements = allAgreements ?? previous?.allAgreements ?? [];
    var _isCreate = isCreate ?? previous?.isCreate ?? false;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = (createUpdateStatePhase ??
            (contractChanges != null
                ? CreateUpdateStatePhase.InProgress
                : null)) ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;
    var _errors = previous?.errors ?? ContractCreateUpdateErrors();

    if (contractChanges != null) contractChanges(_contract);

    _errors.contractUpdated(_contract, _selectedAgreement);

    _isValid = _errors.isValid(_isCreate);

    return ContractCreateUpdateState(
      errors: _errors,
      contract: _contract,
      selectedAgreement: _selectedAgreement,
      allAgreements: _allAgreements,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'ContractCreateUpdateState { contract: ${contract.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
