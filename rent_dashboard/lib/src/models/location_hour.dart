import '../../generic/date_time.dart';

List<DateTime> holidays = List();

class LocationHour {
  bool differentWeeks;

  double l_Mon;
  double l_Tue;
  double l_Wed;
  double l_Thu;
  double l_Fri;
  double l_Sat;
  double l_Sun;

  double u_Mon;
  double u_Tue;
  double u_Wed;
  double u_Thu;
  double u_Fri;
  double u_Sat;
  double u_Sun;

  //Nytårsdag
  bool newyearsDay;
  //Palmesøndag
  bool palmsunday;
  //Skærtorsdag
  bool maundyThursday;
  //Langfredag
  bool goodFriday;
  //1. påskedag
  bool easterDay;
  //2. påskedag
  bool secondEasterDay;
  //Store bededag
  bool prayerDay;
  //kristi himmelfartsdag
  bool christAscension;
  //Pinse
  bool whitSunday;
  //2. pinsedag
  bool sndPentecost;
  //Juleaften
  bool christmasEve;
  //Juledag
  bool christmasDay;
  //2. Juledag
  bool sndChristmasDay;
  //Nytårsaften
  bool newyearsEve;

  LocationHour({json}) {
    if(json == null)
    {
      //print('is null');
      return;
    }
    differentWeeks = json['differentWeeks'];

    l_Mon = json['l_Mon'];
    l_Tue = json['l_Tue'];
    l_Wed = json['l_Wed'];
    l_Thu = json['l_Thu'];
    l_Fri = json['l_Fri'];
    l_Sat = json['l_Sat'];
    l_Sun = json['l_Sun'];

    u_Mon = json['u_Mon'];
    u_Tue = json['u_Tue'];
    u_Wed = json['u_Wed'];
    u_Thu = json['u_Thu'];
    u_Fri = json['u_Fri'];
    u_Sat = json['u_Sat'];
    u_Sun = json['u_Sun'];

    newyearsDay = json['newyearsDay'];
    palmsunday = json['palmsunday'];
    maundyThursday = json['maundyThursday'];
    goodFriday = json['goodFriday'];
    easterDay = json['easterDay'];
    secondEasterDay = json['secondEasterDay'];
    prayerDay = json['prayerDay'];
    christAscension = json['christAscension'];
    whitSunday = json['whitSunday'];
    sndPentecost = json['sndPentecost'];
    christmasEve = json['christmasEve'];
    christmasDay = json['christmasDay'];
    sndChristmasDay = json['sndChristmasDay'];
    newyearsEve = json['newyearsEve'];
  }

  hoursBetween(DateTime start, DateTime end) {
    var total = 0.0;
    var date = start;
    while (date.compareTo(end) != 1) {
      var weekOfYear = getWeekOfYear(date);

      if (include(date)) {
        if (weekOfYear % 2 == 1 && differentWeeks) {
          if (date.weekday == 1) total += u_Mon;
          if (date.weekday == 2) total += u_Tue;
          if (date.weekday == 3) total += u_Wed;
          if (date.weekday == 4) total += u_Thu;
          if (date.weekday == 5) total += u_Fri;
          if (date.weekday == 6) total += u_Sat;
          if (date.weekday == 7) total += u_Sun;
        } else if (weekOfYear % 2 == 0 || !differentWeeks) {
          if (date.weekday == 1) total += l_Mon;
          if (date.weekday == 2) total += l_Tue;
          if (date.weekday == 3) total += l_Wed;
          if (date.weekday == 4) total += l_Thu;
          if (date.weekday == 5) total += l_Fri;
          if (date.weekday == 6) total += l_Sat;
          if (date.weekday == 7) total += l_Sun;
        }
      }

      date = date.add(Duration(days: 1));
    }
    return total;
  }

  bool sameDate(DateTime date1, DateTime date2)
  {
    return date1.month == date2.month && date1.day == date2.day;
  }

  bool include(DateTime date) {
    if(!newyearsDay && sameDate(date, holidays[0])) return false;
    if(!palmsunday && sameDate(date, holidays[1])) return false;
    if(!maundyThursday && sameDate(date, holidays[2])) return false;
    if(!goodFriday && sameDate(date, holidays[3])) return false;
    if(!easterDay && sameDate(date, holidays[4])) return false;
    if(!secondEasterDay && sameDate(date, holidays[5])) return false;
    if(!prayerDay && sameDate(date, holidays[6])) return false;
    if(!christAscension && sameDate(date, holidays[7])) return false;
    if(!whitSunday && sameDate(date, holidays[8])) return false;
    if(!sndPentecost && sameDate(date, holidays[9])) return false;
    if(!christmasEve && sameDate(date, holidays[10])) return false;
    if(!christmasDay && sameDate(date, holidays[11])) return false;
    if(!sndChristmasDay && sameDate(date, holidays[12])) return false;
    if(!newyearsEve && sameDate(date, holidays[13])) return false;
    return true;
  }
}
