import './data.dart';

class Customer {
  int id;
  String name;
  SimpleData smallData;
  Customer.fromJson(json) {
    id = json['id'];
    name = json['name'];
    smallData = SimpleData(json);
  }
}
