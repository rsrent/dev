import 'dart:ui';

import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/models/post_condition.dart';
import 'package:bms_dart/src/models/query_result.dart';

import 'translations.dart';

class TranslationsDanish extends Translations {
  TranslationsDanish(Locale locale) : super(locale);

  String get buttonLogout => 'Log ud';

  String get buttonUsers => 'Brugere';
  String get buttonClients => 'Kunder';
  String get buttonLocations => 'Lokationer';
  String get buttonAgreements => 'Overenskomster';
  String get buttonAbsenseReasons => 'Fraværsgrunde';
  String get buttonAbsense => 'Fravær';
  String get buttonConversations => 'Samtaler';
  String get buttonPosts => 'Posts';
  String get buttonContracts => 'Kontrakter';
  String get buttonWork => 'Vagter';
  String get buttonWorkContracts => 'Vagtkontrakter';
  String get buttonAccidentReports => 'Ulykkesrapporter';
  String get buttonLogs => 'Logs';

  String get buttonApprove => 'Accepter';
  String get buttonDeline => 'Afvis';
  String get infoPending => 'Afventer';
  String get infoErrorLoading => 'Hov, noget gik galt!';
  String get infoNoAgreements => 'Ingen overenskomster';
  String get infoNoNotis => 'Ingen notifikationer';
  String get infoNoCustomers => 'Ingen kunder';
  String get infoNoLocations => 'Ingen lokationer';
  String get infoNoConversations => 'Ingen samtaler';
  String get infoNoMessages => 'Ingen beskeder';
  String get infoNoAbsenceReasons => 'Ingen fraværsgrunde';
  String get infoNoAbsences => 'Intet fravær';
  String get infoNoWorks => 'Ingen vagter';
  String get infoNoWorkContracts => 'Ingen vagterplaner';
  String get infoNoContracts => 'Ingen kontrakter';
  String get infoNoPosts => 'Ingen posts';
  String get infoNoUsers => 'Ingen brugere';
  String get infoNoLogs => 'Ingen logs';
  String get infoNoProjects => 'Ingen projekter';
  String get infoNoTasks => 'Ingen opgaver';
  String get infoNoTaskCompleteds => 'Ingen færdige opgaver';
  String get infoNoQualityReports => 'Ingen kvalitetsrapporter';
  String get infoNoQualityReportItems => 'Ingen kvalitetsrapportpunkter';
  String get infoNoFolders => 'Ingen foldere';

  String get infoNoAccidentReports => 'Ingen uheldsrapporter';
  String get infoCreationFailed => 'Oprettelse fejlede';
  String get infoUpdateFailed => 'Opdatering fejlede';
  String get infoCreationSucceded => 'Oprettet';
  String get infoUpdateSucceded => 'Opdateret';

  String get optionCreateWork => 'Opret vagt';
  String get optionCreateWorkContract => 'Opret vagtplan';

  String get titlePostConditionType => 'Modtager skal';
  String get labelPostConditionValue => 'Kravets værdi';

  // String postConditionTypeString(PostConditionType postConditionType) {
  //   switch (postConditionType) {
  //     case PostConditionType.HasRole:
  //       return 'Have rolle';
  //     case PostConditionType.AtLocation:
  //       return 'Være knyttet til lokation';
  //     case PostConditionType.AtAnyLocationUnderCustomer:
  //       return 'Være knyttet til lokation under kunde';
  //   }
  //   return '';
  // }

  String accidentReportTypeString(AccidentReportType accidentReportType) {
    switch (accidentReportType) {
      case AccidentReportType.Accident:
        return 'Arbejdsulykke';
      case AccidentReportType.AlmostAccident:
        return 'Nær-ved-ulykke';
    }
    return '';
  }

  String get buttonAddCondition => 'Tilføj begrænsning';
  String get buttonBack => 'Tilbage';
  String get buttonCreate => 'Opret';
  String get buttonRequest => 'Anmod';
  String get buttonLogin => 'Log ind';
  String get buttonUpdate => 'Opdater';
  String get hintSelectAbsenceReason => 'Vælg fraværsgrund';
  String get hintSelectAgreement => 'Vælg overenskomst';
  String get infoAbsent => 'Fraværende';
  String get infoApproved => 'Godkendt';
  String get infoDeclined => 'Afvist';
  String get infoNoResults => 'Ingen resultater';
  String get infoReplacing => 'Afløser';
  String get labelAgreement => 'Overenskomst';
  String get labelName => 'Navn';
  String get labelBreakDuration => 'Pauselængde';
  String get labelCanManagerCreate => 'Kan manager oprette';
  String get labelCanManagerRequest => 'Kan manager anmode';
  String get labelCanUserCreate => 'Kan bruger oprette';
  String get labelCanUserRequest => 'Kan bruger anmode';
  String get labelComment => 'Kommentar';
  String get labelDate => 'Dato';
  String get labelDescription => 'Beskrivelse';
  String get labelEndTime => 'Sluttidspunkt';
  String get labelEvenUnevenWeeks => 'Lige/ulige uger';
  String get labelEvenWeeks => 'Lige uger';
  String get labelFrom => 'Fra';
  String get labelHoursWeekly => 'Timer ugentligt';
  String get labelIsVisible => 'Synlig';
  String get labelOrganization => 'Organisation';
  String get labelPassword => 'Kodeord';
  String get labelStartTime => 'Starttidspunkt';
  String get labelTo => 'Til';
  String get labelUnevenWeeks => 'Ulige uger';
  String get labelEmail => 'Email';

  String get titleCreateAbsenceReason => 'Opret nyt fraværsgrund';
  String get hintSelectContract => 'Vælg kontrakt';
  String get hintSearch => 'Søg...';
  String get hintSearchLocations => 'Søg efter lokationer...';
  String get hintSearchCustomers => 'Søg efter kunder...';
  String get hintSearchUsers => 'Søg efter brugere...';
  String get infoCreationSuccessful => 'Oprettet';
  String get infoLoginFailed => 'Login fejlede';
  String get infoUpdateSuccessful => 'Opdateret';
  String get titleUpdateAbsenceReason => 'Opdater fraværsgrund';
  String get titleCreateAbsence => 'Opret fravær';
  String get titleCreateAgreement => 'Opret overenskomst';
  String get titleCreateContract => 'Opret kontrakt';
  String get titleCreatePost => 'Opret post';
  String get titleCreateWork => 'Opret vagt';
  String get titleCreateAccidentReport => 'Registrer arbejdsulykke';
  String get titleCreateAlmostAccidentReport => 'Registrer nær-ved-ulykke';
  String get titleUpdateAccidentReport => 'Opdater arbejdsulykke';
  String get titleUpdateAlmostAccidentReport => 'Opdater nær-ved-ulykke';
  String get titleUpdateWork => 'Opdater vagt';
  String get titleCreateWorkContract => 'Opret vagtplan';
  String get titleUpdateWorkContract => 'Opdater vagtplan';
  String get titleCreateComment => 'Opret kommentar';
  String get titleUpdateComment => 'Opdater kommentar';
  String get titleCreateAddress => 'Opret adresse';
  String get titleUpdateAddress => 'Opdater adresse';

  String get titleUpdateAbsence => 'Opdater fravær';
  String get titleUpdateAgreement => 'Opdater overenskomst';
  String get titleUpdateContract => 'Opdater kontrakt';
  String get hintFindUserWhoCanTakeWork => 'Find bruger til vagt';
  String get optionTryAgain => 'Prøv igen';
  String get optionFindWorkReplacer => 'Find afløser';
  String get optionRegisterWork => 'Registrer vagt';
  String get optionRemoveWorkReplacer => 'Fjern afløser';
  String get optionRemoveWorkUser => 'Fjern bruger';
  String get optionFindWorkOwner => 'Find ejer';
  String get infoWorkMissingOwner => 'Vagt har ingen ejer';
  String get infoWorkMissingReplacer => 'Vagt har ingen afløser';
  String get infoWorkContractMissingOwner => 'Vagtplan har ingen ejer';
  String get infoIsRequest => 'Anmodning';
  String get infoToday => 'Idag';
  String get infoTomorrow => 'Imorgen';
  String get infoYesterday => 'Igår';

  String get labelAccidentTime => 'Dato og tidspunkt';
  String get labelAccidentPlace => 'Sted';
  String get labelAccidentDescription => 'Hvad skete der';
  String get labelAccidentActionTaken => 'Handling foretaget efter ulykke';
  String get labelAlmostAccidentActionTaken => 'Forebyggelse på stedet';
  String get labelAccidentAbsenceDuration =>
      'Forventet varighed af sygemelding i dage';

  String get titleCreateUser => 'Opret bruger';
  String get titleUpdateUser => 'Opdater bruger';
  String get titleCreateLocation => 'Opret lokalitet';
  String get titleUpdateLocation => 'Opdater lokalitet';
  String get titleCreateCustomer => 'Opret kunde';
  String get titleUpdateCustomer => 'Opdater kunde';
  String get titleCreateLog => 'Opret log';
  String get titleUpdateLog => 'Opdater log';
  String get titleCreateTask => 'Opret opgave';
  String get titleUpdateTask => 'Opdater opgave';
  String get titleCreateTaskCompleted => 'Opret færdig opgave';
  String get titleUpdateTaskCompleted => 'Opdater færdig opgave';

  String get defaultErrorTitle => 'Fejl';
  String get defaultUnauthorizedAccessTitle => 'Ingen adgang';
  String get defaultUnauthorizedAccessMessage =>
      'Du har ikke adgang til denne funktion';

  @override
  QueryResultTranslations get addUser => QueryResultTranslations(
        successTitle: 'Bruger tilføjet',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get removeUser => QueryResultTranslations(
        successTitle: 'Bruger fjernet',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get inviteReplied => QueryResultTranslations(
        successTitle: 'Invitation besvaret',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get inviteUser => QueryResultTranslations(
        successTitle: 'Bruger inviteret',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get registered => QueryResultTranslations(
        successTitle: 'Registreret',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get createAttempted => QueryResultTranslations(
        successTitle: 'Oprettet',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get requestAttempted => QueryResultTranslations(
        successTitle: 'Anmodning sendt',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get updateAttempted => QueryResultTranslations(
        successTitle: 'Opdateret',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get absenceCreated => QueryResultTranslations(
        successTitle: 'Fravær oprettet',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get absenceReplied => QueryResultTranslations(
        successTitle: 'Svar sendt',
        errorTitle: 'Fejl',
        errorMessage: 'Dit svar blev ikke sendt, prøv igen',
      );
  @override
  QueryResultTranslations get absenceRequested => QueryResultTranslations(
        successTitle: 'Anmodning sendt',
        successMessage: 'Din anmodning er sendt og afvendter godkendelse',
        errorTitle: 'Fejl',
      );
  @override
  QueryResultTranslations get inviteAttempted => QueryResultTranslations(
        successTitle: 'Invitation sendt',
        errorTitle: 'Fejl',
        errorMessage: 'Invitationen blev ikke sendt afsted',
      );
}
