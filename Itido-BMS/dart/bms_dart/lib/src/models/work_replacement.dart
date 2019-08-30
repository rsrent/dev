import 'package:bms_dart/src/models/absence.dart';
import 'package:bms_dart/src/models/contract.dart';

import 'user.dart';

class WorkReplacement {
  // int workId;
  // int contractId;
  // int absenceId;

  Absence absence;
  Contract contract;

  WorkReplacement();

  factory WorkReplacement.fromJson(json, {rootJson, path}) {
    if (json == null) return null;
    return WorkReplacement._fromJson(json, rootJson ?? {}, path ?? '');
  }

  WorkReplacement._fromJson(json, rootJson, path)
      :
        // this.workId = json['workId'],
        // this.contractId = json['contractId'],
        // this.absenceId = json['absenceId'],
        this.absence = Absence.fromJson(
            json['absence'] ?? rootJson['${path}_absence'],
            rootJson: rootJson,
            path: '${path}_absence'),
        this.contract = Contract.fromJson(
            json['contract'] ?? rootJson['${path}_contract'],
            rootJson: rootJson,
            path: '${path}_contract');

  // Map<String, dynamic> toMap() => {
  //       'workId': this.workId,
  //       'contractId': this.contractId,
  //       'absenceId': this.absenceId,
  //     };
}
