/*

import 'dart:math';
import './models/task.dart';
import './models/location.dart';
import './models/customer.dart';
import './models/user.dart';

Task getTaskDummy() {
  var t = new Task();
  t.plan = 'Vinduer';
  t.area = 'Udstilling';
  t.lastCleaned = DateTime.now().add(Duration(days: -30));
  t.lastCleanedBy = 'Mudassar Iqbal';
  t.nextCleaned = DateTime.now().add(Duration(days: Random().nextInt(30) * -1 + 25));
  return t;
}

User getUserDummy()
{
  var u = User();
  u.firstName = 'Tobias';
  u.lastName = 'Bang';
  u.id = Random().nextInt(3);
  return u;
}

Location getLocationDummy() {
  var l = new Location();
  l.name = 'Forborg';
  l.serviceLeader = getUserDummy();
  l.tasks = List();
  l.tasks..add(getTaskDummy())..add(getTaskDummy())..add(getTaskDummy())..add(getTaskDummy());
  return l;
}

Customer getCustomerDummy() {
  var c = Customer();
  c.name = 'H&M';
  c.locations = List()..add(getLocationDummy())..add(getLocationDummy())..add(getLocationDummy())..add(getLocationDummy());
  return c;
} 

List<Customer> getCustomersDummy()
{
  var cs = List<Customer>();
  cs..add(getCustomerDummy())..add(getCustomerDummy())..add(getCustomerDummy())..add(getCustomerDummy());
  return cs;
}*/
