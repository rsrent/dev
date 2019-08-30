import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';
import 'package:intl/intl.dart';

abstract class Sprog {
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
}
