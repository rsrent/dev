void main() {
  var month = 12;
  var days = 31;
  var firstDate = DateTime(2019, month);
  var lastDate = DateTime(2019, month, days);

  var daysInFirstWeek = (7 - (firstDate.weekday - 1));
  var daysInLastWeek = (lastDate.weekday);

  print('daysInFirstWeek: $daysInFirstWeek');
  print('daysInLastWeek: $daysInLastWeek');

  var weeks = ((days - daysInFirstWeek - daysInLastWeek) / 7).round() + 2;
  print('weeks: $weeks');
}
