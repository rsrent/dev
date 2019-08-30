import 'dart:async';
import 'package:bms_dart/models.dart';
import 'package:flutter/material.dart';
import 'package:flutter/foundation.dart' show SynchronousFuture;
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:intl/intl.dart';
import 'package:bms_dart/sprog.dart';
import 'translations_danish.dart';
import 'translations_english.dart';

import 'package:bms_dart/query_result.dart';

import 'package:flutter_packages/storage.dart' as storage;

abstract class Translations with Sprog {
  Translations(this.locale);

  final Locale locale;

  static Translations of(BuildContext context) {
    return Localizations.of<Translations>(context, Translations);
  }

  String dateString(DateTime dateTime) => dateTime != null
      ? DateFormat.yMMMd(locale.languageCode).format(dateTime)
      : '-';

  String timeString(DateTime dateTime) => dateTime != null
      ? DateFormat.Hm(locale.languageCode).format(dateTime)
      : '-';

  String dateTimeString(DateTime dateTime) => dateTime != null
      ? '${dateString(dateTime)} ${timeString(dateTime)}'
      : '-';
  String get buttonLogin;
  String get buttonLogout;
  String get buttonCreate;
  String get buttonRequest;
  String get buttonUpdate;
  String get buttonAddCondition;
  String get buttonBack;

  String get buttonApprove;
  String get buttonDeline;

  String get buttonUsers;
  String get buttonClients;
  String get buttonLocations;
  String get buttonAgreements;
  String get buttonAbsenseReasons;
  String get buttonAbsense;
  String get buttonConversations;
  String get buttonPosts;
  String get buttonContracts;
  String get buttonWork;
  String get buttonWorkContracts;
  String get buttonAccidentReports;
  String get buttonLogs;

  String get infoApproved;
  String get infoDeclined;
  String get infoPending;
  String get infoErrorLoading;
  String get infoNoAccidentReports;
  String get infoNoAgreements;
  String get infoNoNotis;
  String get infoNoCustomers;
  String get infoNoLocations;
  String get infoNoConversations;
  String get infoNoMessages;
  String get infoNoAbsenceReasons;
  String get infoNoAbsences;
  String get infoNoWorks;
  String get infoNoWorkContracts;
  String get infoNoContracts;
  String get infoNoPosts;
  String get infoNoUsers;
  String get infoNoLogs;
  String get infoNoProjects;
  String get infoNoTasks;
  String get infoNoTaskCompleteds;
  String get infoNoQualityReports;
  String get infoNoQualityReportItems;
  String get infoNoFolders;

  String get infoNoResults;
  String get infoAbsent;
  String get infoReplacing;

  String get infoWorkMissingOwner;
  String get infoWorkMissingReplacer;
  String get infoWorkContractMissingOwner;

  String get infoCreationFailed;
  String get infoUpdateFailed;
  String get infoCreationSuccessful;
  String get infoUpdateSuccessful;

  String get infoLoginFailed;

  /// Used to show when an entity is a request and hence not enabled.
  String get infoIsRequest;

  String get optionTryAgain;
  String get optionCreateWork;
  String get optionCreateWorkContract;
  String get optionRegisterWork;
  String get optionRemoveWorkUser;
  String get optionRemoveWorkReplacer;
  String get optionFindWorkReplacer;
  String get optionFindWorkOwner;

  // String postConditionTypeString(PostConditionType postConditionType);
  String accidentReportTypeString(AccidentReportType accidentReportType);

  String get titlePostConditionType;
  String get labelPostConditionValue;

  String get hintSelectAbsenceReason;

  String get labelDate;
  String get labelFrom;
  String get labelTo;
  String get labelStartTime;
  String get labelEndTime;
  String get labelBreakDuration;
  String get labelComment;
  String get labelDescription;

  String get labelIsVisible;
  String get labelEvenUnevenWeeks;
  String get labelEvenWeeks;
  String get labelUnevenWeeks;

  String get labelCanUserCreate;
  String get labelCanUserRequest;
  String get labelCanManagerCreate;
  String get labelCanManagerRequest;

  String get labelName;
  String get labelHoursWeekly;
  String get labelAgreement;
  String get hintSelectAgreement;
  String get hintSearch;

  String get labelEmail;
  String get labelPassword;
  String get labelOrganization;

  String get titleCreateAbsenceReason;
  String get titleUpdateAbsenceReason;

  String get titleCreateAbsence;
  String get titleUpdateAbsence;
  String get titleCreateContract;
  String get titleUpdateContract;
  String get titleCreateAgreement;
  String get titleUpdateAgreement;

  String get titleCreatePost;
  String get titleCreateWork;
  String get titleCreateAccidentReport;
  String get titleCreateAlmostAccidentReport;
  String get titleUpdateAccidentReport;
  String get titleUpdateAlmostAccidentReport;
  String get titleCreateWorkContract;
  String get titleUpdateWorkContract;
  String get titleUpdateWork;
  String get titleCreateUser;
  String get titleUpdateUser;
  String get titleCreateLocation;
  String get titleUpdateLocation;
  String get titleCreateCustomer;
  String get titleUpdateCustomer;
  String get titleCreateLog;
  String get titleUpdateLog;
  String get titleCreateTask;
  String get titleUpdateTask;
  String get titleCreateTaskCompleted;
  String get titleUpdateTaskCompleted;
  String get titleCreateComment;
  String get titleUpdateComment;
  String get titleCreateAddress;
  String get titleUpdateAddress;

  String get hintSelectContract;

  String get hintFindUserWhoCanTakeWork;
  String get hintSearchLocations;
  String get hintSearchCustomers;

  String get hintSearchUsers;

  String get infoYesterday;
  String get infoToday;
  String get infoTomorrow;

  String get labelAccidentTime;
  String get labelAccidentPlace;
  String get labelAccidentDescription;
  String get labelAccidentActionTaken;
  String get labelAlmostAccidentActionTaken;
  String get labelAccidentAbsenceDuration;

  String get defaultErrorTitle;
  String get defaultUnauthorizedAccessTitle;
  String get defaultUnauthorizedAccessMessage;

/*
  QueryResultTranslations get addUser;
  QueryResultTranslations get removeUser;
  QueryResultTranslations get inviteUser;
  QueryResultTranslations get inviteReplied;
  QueryResultTranslations get registered;


  QueryResultTranslations get createAttempted;
  QueryResultTranslations get updateAttempted;
  QueryResultTranslations get requestAttempted;
  QueryResultTranslations get inviteAttempted;

  QueryResultTranslations get absenceRequested;
  QueryResultTranslations get absenceCreated;
  QueryResultTranslations get absenceReplied;
  */
}

class BmsLocalizationsDelegate extends LocalizationsDelegate<Translations> {
  final Locale newLocale;
  const BmsLocalizationsDelegate({this.newLocale});

  @override
  bool isSupported(Locale locale) {
    return ['en', 'da'].contains(locale.languageCode);
  }

  @override
  Future<Translations> load(Locale locale) {
    var _newLocale = newLocale ?? locale;

    if (newLocale == null) {
      var result = storage.Storage.readSync('prefered_language_code');

      if (result != null && result.length == 2) {
        _newLocale = Locale(result);
      } else {
        _newLocale = locale;
      }
    }

    var languageCode = _newLocale.languageCode;
    storage.Storage.writeSync('prefered_language_code', languageCode);

    if (languageCode == 'da')
      return SynchronousFuture<Translations>(TranslationsDanish(_newLocale));
    if (languageCode == 'en')
      return SynchronousFuture<Translations>(TranslationsEnglish(_newLocale));
    return SynchronousFuture<Translations>(TranslationsEnglish(_newLocale));
  }

  @override
  bool shouldReload(BmsLocalizationsDelegate old) {
    return true;
  }
}
