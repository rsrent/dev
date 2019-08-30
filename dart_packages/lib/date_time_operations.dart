library date_time_operations;

DateTime adjustTime(DateTime t) {
  var mins = ((t.minute / 15.0).roundToDouble() * 15).round();
  return t.add(Duration(
      minutes: mins - t.minute,
      seconds: -t.second,
      milliseconds: -t.millisecond,
      microseconds: -t.microsecond));
}

/// returns the time of the DateTime as a string in the format HH:mm, e.g 16:30
String toHHmm(DateTime t) {
  if (t == null) return '-';
  return (t.hour < 10 ? '0${t.hour}:' : '${t.hour}:') +
      (t.minute < 10 ? '0${t.minute}' : '${t.minute}');
}

String durationToHHmm(Duration d) {
  if (d == null) return '-';
  var hours = d.inHours;
  var mins = d.inMinutes - (d.inHours * 60 * 60);
  return (hours < 10 ? '0$hours:' : '$hours:') +
      (mins < 10 ? '0$mins' : '$mins');
}

String toDDMM(DateTime t) {
  if (t == null) return '-';
  return (t.day < 10 ? '0${t.day}/' : '${t.day}/') +
      (t.month < 10 ? '0${t.month}' : '${t.month}');
}

bool isSameDate(DateTime t1, DateTime t2) {
  return t1 == null && t2 == null ||
      (t1.year == t2.year && t1.month == t2.month && t1.day == t2.day);
}

int compareDate(DateTime t1, DateTime t2) {
  return toDate(t1).compareTo(toDate(t2));
}

String toDDMMyy(DateTime t) {
  if (t == null) return '-';
  //return '${t.day}/${t.month}/${t.year.toString().substring(2)}';

  return (t.day < 10 ? '0${t.day}/' : '${t.day}/') +
      (t.month < 10 ? '0${t.month}/' : '${t.month}/') +
      '${t.year.toString().substring(2)}';
}

String dateTimeShortMonthName(DateTime dateTime) {
  if (dateTime.month == 1) return 'Jan';
  if (dateTime.month == 2) return 'Feb';
  if (dateTime.month == 3) return 'Mar';
  if (dateTime.month == 4) return 'Apr';
  if (dateTime.month == 5) return 'Maj';
  if (dateTime.month == 6) return 'Jun';
  if (dateTime.month == 7) return 'Jul';
  if (dateTime.month == 8) return 'Aug';
  if (dateTime.month == 9) return 'Sep';
  if (dateTime.month == 10) return 'Okt';
  if (dateTime.month == 11) return 'Nov';
  if (dateTime.month == 12) return 'Dec';
  return '';
}

int daysInMonth(DateTime dateTime) {
  return daysInMonthInt(dateTime.month, dateTime.year);
}

int daysInMonthInt(int month, int year) {
  if (month == 1) return 31;
  if (month == 2) {
    if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0) return 29;
    return 28;
  }
  if (month == 3) return 31;
  if (month == 4) return 30;
  if (month == 5) return 31;
  if (month == 6) return 30;
  if (month == 7) return 31;
  if (month == 8) return 31;
  if (month == 9) return 30;
  if (month == 10) return 31;
  if (month == 11) return 30;
  if (month == 12) return 31;
  return 0;
}

int dayOfYear(DateTime dateTime) {
  int month = dateTime.month;
  int days = 0;
  if (month > 1) {
    var counter = 1;
    while (counter + 1 <= month) {
      days += daysInMonthInt(counter, dateTime.year);
      counter++;
    }
  }
  days += dateTime.day;
  //print(days);
  return days;
}

int weekOfYear(DateTime date) {
  final startOfYear = new DateTime(date.year, 1, 1, 0, 0);
  final firstMonday = startOfYear.weekday;
  final daysInFirstWeek = 7 - firstMonday;
  final diff = date.difference(startOfYear);
  var weeks = ((diff.inDays - daysInFirstWeek) / 7).ceil();
// It might differ how you want to treat the first week
  // if (daysInFirstWeek > 3) {
  //   weeks += 1;
  // }
  if (weeks > 52) {
    weeks = weeks - 52;
  }
  print('weeks: ${date.weekday} ${toDDMM(date)}: $weeks');
  return weeks + (date.weekday == 1 ? 1 : 0);
}

String minsToHHmm(int minutes) {
  int hours = (minutes / 60).floor();
  int mins = minutes - (hours * 60);

  String hourString = '${hours}';
  if (hourString.length == 1) hourString = '0' + hourString;

  String minString = '${mins}';
  if (minString.length == 1) minString = '0' + minString;
  return '${hourString}:${minString}';
}

String minsToHours(int minutes) =>
    minutes == null ? '-' : (minutes / 60).toStringAsFixed(2);

DateTime toDate(DateTime date) => DateTime(date.year, date.month, date.day);
DateTime startOfMonth(DateTime date) => DateTime(date.year, date.month, 1);
DateTime endOfMonth(DateTime date) =>
    DateTime(date.year, date.month, daysInMonth(date));
DateTime toUtcDate(DateTime date) =>
    DateTime.utc(date.year, date.month, date.day);

DateTime endOfDay(DateTime date) =>
    DateTime(date.year, date.month, date.day, 23, 59, 59);
