import 'package:bms_dart/models.dart';

import 'date_time_converter.dart';

class WorkContract {
  int id;
  String note;
  DateTime fromDate;
  DateTime toDate;
  bool isVisible;

  Contract contract;
  Project project;

  List<WorkDay> workDays;
  List<WorkHoliday> workHolidays;

  WorkContract({
    // this.contractId,
    // this._projectId,
    this.note,
    this.fromDate,
    this.toDate,
    this.isVisible,
    this.workDays,
    this.workHolidays,
  });

  WorkContract.fromJson(json)
      : this.id = json['id'],
        this.note = json['note'],
        this.fromDate = toDateTime(json['fromDate']),
        this.toDate = toDateTime(json['toDate']),
        this.isVisible = json['isVisible'],
        this.project = Project.fromJson(json['projectItem_project']),
        this.contract = Contract.fromJson(
          json['contract'],
          rootJson: json,
          path: 'contract',
        ),
        this.workDays = List.castFrom(json['workDays'] ?? [])
            .map((j) => WorkDay.fromJson(j))
            .toList(),
        this.workHolidays = List.castFrom(json['workHolidays'] ?? [])
            .map((j) => WorkHoliday.fromJson(j))
            .toList();

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'note': this.note,
        'fromDate': this.fromDate.toString(),
        'toDate': this.toDate.toString(),
        'isVisible': this.isVisible,
        'workDays': this.workDays?.map((w) => w.toMap())?.toList(),
        'workHolidays': this.workHolidays?.map((w) => w.toMap())?.toList(),
      };

  bool get isOwned => contract != null;

  User get user => isOwned && contract.user != null ? contract.user : null;
}
