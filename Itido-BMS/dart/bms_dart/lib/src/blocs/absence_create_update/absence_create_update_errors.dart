//enum ErrorState

import '../../../models.dart';
import '../../validators/validators.dart';

class AbsenceCreateUpdateErrors {
  bool isAbsenceReasonValid;
  bool canCreate;
  bool canRequest;
  void absenceUpdated(User requester, int userId, Absence absence,
      AbsenceReason absenceReason) {
    isAbsenceReasonValid = absenceReason != null;

    canCreate = absenceReason != null &&
        (absenceReason.canManagerCreate &&
                (requester.userRole == 'Admin' ||
                    requester.userRole == 'Manager') ||
            (absenceReason.canUserCreate && requester.id == userId));

    canRequest = absenceReason != null &&
        (absenceReason.canManagerRequest &&
                (requester.userRole == 'Admin' ||
                    requester.userRole == 'Manager') ||
            (absenceReason.canUserRequest && requester.id == userId));
  }

  bool isValid(bool isCreate) => isAbsenceReasonValid;
}
