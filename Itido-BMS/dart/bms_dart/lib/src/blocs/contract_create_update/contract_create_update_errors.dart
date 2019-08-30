//enum ErrorState

import '../../../models.dart';
import '../../validators/validators.dart';

class ContractCreateUpdateErrors {
  bool isAgreementValid;
  bool isHoursValid;
  void contractUpdated(Contract contract, Agreement agreement) {
    isAgreementValid = agreement != null;
    isHoursValid = contract.weeklyHours != null && contract.weeklyHours > 0;
  }

  bool isValid(bool isCreate) => isAgreementValid && isHoursValid;
}
