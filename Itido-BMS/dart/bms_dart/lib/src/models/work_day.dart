import 'package:flutter/material.dart';

class WorkDay {
  int index;
  int workContractId;
  int dayOfWeek;
  int isEvenWeek;
  int startTimeMins;
  int endTimeMins;
  int breakMins;

  WorkDay({
    this.index,
    this.workContractId,
    this.dayOfWeek,
    this.isEvenWeek,
    this.startTimeMins,
    this.endTimeMins,
    this.breakMins,
  });

  factory WorkDay.fromJson(json) {
    if (json == null) return null;
    return WorkDay._fromJson(json);
  }

  WorkDay._fromJson(json)
      : this.workContractId = json['workContractId'],
        this.dayOfWeek = json['dayOfWeek'],
        this.isEvenWeek = json['isEvenWeek'],
        this.startTimeMins = json['startTimeMins'],
        this.endTimeMins = json['endTimeMins'],
        this.breakMins = json['breakMins'];

  Map<String, dynamic> toMap() => {
        'workContractId': this.workContractId,
        'dayOfWeek': this.dayOfWeek,
        'isEvenWeek': this.isEvenWeek,
        'startTimeMins': this.startTimeMins,
        'endTimeMins': this.endTimeMins,
        'breakMins': this.breakMins,
      };
}
