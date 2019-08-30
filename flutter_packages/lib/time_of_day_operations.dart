import 'package:flutter/material.dart';

TimeOfDay minsToTimeOfDay(int mins) {
  if (mins == null) return null;
  var _hours = mins ~/ 60;
  var _mins = mins % 60;

  if (0 <= _hours && _hours <= 23 && 0 <= _mins && _mins <= 59) {
    return TimeOfDay(hour: _hours, minute: _mins);
  } else {
    throw Exception('Mins to large');
  }
}

int timeOfDayToMins(TimeOfDay timeOfDay) {
  if (timeOfDay == null) return null;
  return timeOfDay.hour * 60 + timeOfDay.minute;
}
