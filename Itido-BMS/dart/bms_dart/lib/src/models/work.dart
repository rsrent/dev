import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;

import 'date_time_converter.dart';

class Work {
  int id;
  String note;
  DateTime date;
  int startTimeMins;
  int endTimeMins;
  int breakMins;
  bool isVisible;
  bool isInvited;
  int inviteCount;
  bool canRegisterWork;

  Contract contract;
  Project project;
  WorkReplacement workReplacement;
  WorkRegistration workRegistration;

  int workContractId;

  // int _projectId;
  // int _contractId;
  // int workTypeId;
  //int workReplacementId;
  //int workRegistrationId;

  //User user;

  Work({
    // this.workContractId,
    // this.contractId,
    // this._projectId,
    // this.workTypeId,
    this.note,
    this.date,
    this.startTimeMins,
    this.endTimeMins,
    this.breakMins,
    //this.workReplacementId,
    this.isVisible,
    // int projectId,
    // this.workRegistrationId,
  });

  Work.fromJson(json)
      : this.id = json['id'],
        this.workContractId = json['workContractID'],
        // this._contractId = json['contractId'],
        // this._projectId = json['projectID'],
        // this.workTypeId = json['workTypeId'],
        this.note = json['note'],
        this.date = toDateTime(json['date']),
        this.startTimeMins = json['startTimeMins'],
        this.endTimeMins = json['endTimeMins'],
        this.breakMins = json['breakMins'],
        //this.workReplacementId = json['workReplacementId'],
        this.isVisible = json['isVisible'],
        this.isInvited = json['isInvited'],
        this.inviteCount = json['inviteCount'],
        this.canRegisterWork = json['canRegisterWork'],
        // this.workRegistrationId = json['workRegistrationId'],

        this.project = Project.fromJson(json['projectItem_project']),
        this.contract = Contract.fromJson(
          json['contract'],
          rootJson: json,
          path: 'contract',
        ),
        this.workReplacement = WorkReplacement.fromJson(
          json['workReplacement'],
          rootJson: json,
          path: 'workReplacement',
        ),
        this.workRegistration = WorkRegistration.fromJson(
          json['workRegistration'],
          rootJson: json,
          path: 'workRegistration',
        );

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'note': this.note,
        'date': this.date.toString(),
        'startTimeMins': this.startTimeMins,
        'endTimeMins': this.endTimeMins,
        'breakMins': this.breakMins,
        'isVisible': this.isVisible,
        'isInvited': this.isInvited,
        'inviteCount': this.inviteCount,
        // 'workContractId': this.workContractId,
        // 'projectId': this._projectId,
        // 'contractId': this._contractId,
        'modifications': 0,
      };

  bool get isRegistered => workRegistration != null;

  bool get isOwned => contract != null;

  bool get isOwnerAbsent => workReplacement != null;
  bool get isReplaced => workReplacement?.contract != null;

  bool get isTaken =>
      contract != null && workReplacement == null ||
      workReplacement != null && workReplacement.contract != null;

  bool get isUserAbsent =>
      workReplacement != null && workReplacement.contract == null;

  bool get isElegibleForRegistration =>
      !isRegistered && dtOps.compareDate(DateTime.now(), date) >= 0;

  bool get isLate =>
      !isRegistered && dtOps.compareDate(DateTime.now(), date) == 1;

  bool get isPartOfWorkContract => workContractId != null;

  User get user => isOwned && contract.user != null ? contract.user : null;
}
