import 'dart:ui';

import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';
import 'package:bms_dart/src/models/post_condition.dart';

import 'translations.dart';

class TranslationsEnglish extends Translations {
  TranslationsEnglish(Locale locale) : super(locale);

  String get buttonLogout => 'Log out';

  String get buttonUsers => 'Users';
  String get buttonClients => 'Customers';
  String get buttonLocations => 'Locations';
  String get buttonAgreements => 'Agreements';
  String get buttonAbsenseReasons => 'Absence reasons';
  String get buttonAbsense => 'Absence';
  String get buttonConversations => 'Conversations';
  String get buttonPosts => 'Posts';
  String get buttonContracts => 'Contracts';
  String get buttonWork => 'Work';
  String get buttonWorkContracts => 'Work contracts';
  String get buttonAccidentReports => 'Accident reports';
  String get buttonLogs => 'Logs';

  String get buttonApprove => 'Accept';
  String get buttonDeline => 'Decline';
  String get infoPending => 'Pending';
  String get infoErrorLoading => 'Oops something went wrong!';
  String get infoNoAgreements => 'No agreements';
  String get infoNoNotis => 'No notifications';
  String get infoNoCustomers => 'No customers';
  String get infoNoLocations => 'No locations';
  String get infoNoConversations => 'No conversations';
  String get infoNoMessages => 'No messages';
  String get infoNoAbsenceReasons => 'No absence reasons';
  String get infoNoAbsences => 'No absence';
  String get infoNoWorks => 'No work';
  String get infoNoWorkContracts => 'No work contracts';
  String get infoNoContracts => 'No contracts';
  String get infoNoPosts => 'No posts';
  String get infoNoUsers => 'No users';
  String get infoNoLogs => 'No logs';
  String get infoNoProjects => 'No projects';
  String get infoNoTasks => 'No tasks';
  String get infoNoTaskCompleteds => 'No completed tasks';
  String get infoNoQualityReports => 'No quality reports';
  String get infoNoQualityReportItems => 'No quality report items';
  String get infoNoFolders => 'No folders';

  String get infoNoAccidentReports => 'No accident reports';
  String get infoCreationFailed => 'Creation failed';
  String get infoUpdateFailed => 'Update failed';
  String get infoCreationSucceded => 'Created';
  String get infoUpdateSucceded => 'Updated';

  String get optionCreateWork => 'Creat work';
  String get optionCreateWorkContract => 'Create work contract';

  String get titlePostConditionType => 'Receiver must';
  String get labelPostConditionValue => 'The condtion value';

  // String postConditionTypeString(PostConditionType postConditionType) {
  //   switch (postConditionType) {
  //     case PostConditionType.HasRole:
  //       return 'Have role';
  //     case PostConditionType.AtLocation:
  //       return 'Have connection to location';
  //     case PostConditionType.AtAnyLocationUnderCustomer:
  //       return 'Have connection to location under customer';
  //   }
  //   return '';
  // }

  String accidentReportTypeString(AccidentReportType accidentReportType) {
    switch (accidentReportType) {
      case AccidentReportType.Accident:
        return 'Accident';
      case AccidentReportType.AlmostAccident:
        return 'Near-by-accident';
    }
    return '';
  }

  String get buttonAddCondition => 'Add condition';
  String get buttonBack => 'Back';
  String get buttonCreate => 'Create';
  String get buttonRequest => 'Anmod';
  String get buttonLogin => 'Login';
  String get buttonUpdate => 'Update';
  String get hintSelectAbsenceReason => 'Select absence reason';
  String get hintSelectAgreement => 'Select agreement';
  String get infoAbsent => 'Absent';
  String get infoApproved => 'Approved';
  String get infoDeclined => 'Declined';
  String get infoNoResults => 'No results';
  String get infoReplacing => 'Replacing';
  String get labelAgreement => 'Agreement';
  String get labelName => 'Name';
  String get labelBreakDuration => 'Break duration';
  String get labelCanManagerCreate => 'Can manager create';
  String get labelCanManagerRequest => 'Can manager request';
  String get labelCanUserCreate => 'Can user create';
  String get labelCanUserRequest => 'Can user create';
  String get labelComment => 'Comment';
  String get labelDate => 'Date';
  String get labelDescription => 'Description';
  String get labelEndTime => 'End time';
  String get labelEvenUnevenWeeks => 'Even/uneven weeks';
  String get labelEvenWeeks => 'Even weeks';
  String get labelFrom => 'From';
  String get labelHoursWeekly => 'Hours weekly';
  String get labelIsVisible => 'Visible';
  String get labelOrganization => 'Organization';
  String get labelPassword => 'Password';
  String get labelStartTime => 'Start time';
  String get labelTo => 'To';
  String get labelUnevenWeeks => 'Uneven weeks';
  String get labelEmail => 'Email';

  String get titleCreateAbsenceReason => 'Create absence reason';
  String get hintSelectContract => 'Select contract';
  String get hintSearch => 'Search...';
  String get hintSearchLocations => 'Search for locations...';
  String get hintSearchCustomers => 'Search for customers...';
  String get hintSearchUsers => 'Search for users...';
  String get infoCreationSuccessful => 'Created';
  String get infoLoginFailed => 'Login failed';
  String get infoUpdateSuccessful => 'Updated';
  String get titleUpdateAbsenceReason => 'Update absence reason';
  String get titleCreateAbsence => 'Create absence';
  String get titleCreateAgreement => 'Create agreement';
  String get titleCreateContract => 'Create contract';
  String get titleCreatePost => 'Create post';
  String get titleCreateWork => 'Create work';
  String get titleCreateAccidentReport => 'Register accident';
  String get titleCreateAlmostAccidentReport => 'Register near-by-accident';
  String get titleUpdateAccidentReport => 'Update accident';
  String get titleUpdateAlmostAccidentReport => 'Update near-by-accident';
  String get titleCreateWorkContract => 'Create work contract';
  String get titleUpdateWorkContract => 'Update work contract';
  String get titleUpdateWork => 'Update work';
  String get titleUpdateAbsence => 'Update absence';
  String get titleUpdateAgreement => 'Update agreement';
  String get titleUpdateContract => 'Update contract';
  String get titleCreateComment => 'Create comment';
  String get titleUpdateComment => 'Update comment';
  String get titleCreateAddress => 'Create address';
  String get titleUpdateAddress => 'Update address';

  String get hintFindUserWhoCanTakeWork => 'Find user for work';

  String get optionTryAgain => 'Try again';
  String get optionFindWorkReplacer => 'Find replacer';
  String get optionRegisterWork => 'Register work';
  String get optionRemoveWorkReplacer => 'Remove replacer';
  String get optionRemoveWorkUser => 'Remove user';
  String get optionFindWorkOwner => 'Find owner';

  String get infoWorkMissingOwner => 'Work has no owner';
  String get infoWorkMissingReplacer => 'Work has no replacer';
  String get infoWorkContractMissingOwner => 'Work contract has no owner';
  String get infoIsRequest => 'Request';

  String get infoToday => 'Today';
  String get infoTomorrow => 'Tomorrow';
  String get infoYesterday => 'Yesterday';

  String get labelAccidentTime => 'Date and time';
  String get labelAccidentPlace => 'Place';
  String get labelAccidentDescription => 'What happened';
  String get labelAccidentActionTaken => 'Action taken after the incident';
  String get labelAlmostAccidentActionTaken => 'Prevention on site';
  String get labelAccidentAbsenceDuration =>
      'Expected duration of sick leave in days';

  String get titleCreateUser => 'Create user';
  String get titleUpdateUser => 'Update user';
  String get titleCreateLocation => 'Create location';
  String get titleUpdateLocation => 'Update location';
  String get titleCreateCustomer => 'Create customer';
  String get titleUpdateCustomer => 'Update customer';
  String get titleCreateLog => 'Create log';
  String get titleUpdateLog => 'Update log';
  String get titleCreateTask => 'Create task';
  String get titleUpdateTask => 'Update task';
  String get titleCreateTaskCompleted => 'Create completed task';
  String get titleUpdateTaskCompleted => 'Update completed task';

  String get defaultErrorTitle => 'Error';
  String get defaultUnauthorizedAccessTitle => 'Unauthorized';
  String get defaultUnauthorizedAccessMessage =>
      'You dont have access to this function';

  @override
  QueryResultTranslations get addUser => QueryResultTranslations(
        successTitle: 'User added',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get removeUser => QueryResultTranslations(
        successTitle: 'User removed',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get inviteReplied => QueryResultTranslations(
        successTitle: 'Invitation answered',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get inviteUser => QueryResultTranslations(
        successTitle: 'User invited',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get registered => QueryResultTranslations(
        successTitle: 'Registered',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get createAttempted => QueryResultTranslations(
        successTitle: 'Created',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get requestAttempted => QueryResultTranslations(
        successTitle: 'Request sent',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get updateAttempted => QueryResultTranslations(
        successTitle: 'Updated',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get absenceCreated => QueryResultTranslations(
        successTitle: 'Absence created',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get absenceReplied => QueryResultTranslations(
        successTitle: 'Answer sent',
        errorTitle: 'Error',
        errorMessage: 'Your answer was not sent, try again',
      );
  @override
  QueryResultTranslations get absenceRequested => QueryResultTranslations(
        successTitle: 'Request sent',
        successMessage: 'Your request has been sent and is awaiting approval',
        errorTitle: 'Error',
      );
  @override
  QueryResultTranslations get inviteAttempted => QueryResultTranslations(
        successTitle: 'Invitation sent',
        errorTitle: 'Error',
        errorMessage: 'The invite was not sent',
      );
}
