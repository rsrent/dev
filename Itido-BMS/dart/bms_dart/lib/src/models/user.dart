import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/models/absence.dart';

class User {
  int id;
  String firstName;
  String lastName;
  String email;
  String phone;
  String comment;
  String userRole;
  String languageCode;
  String title;
  String imageLocation;
  int employeeNumber;
  bool disabled;
  bool hasProject;
  String customerName;
  bool hasAbsence;
  bool hasAbsenceRequest;
  Client client;
  ProjectRole projectRole;

  User({
    this.id,
    this.firstName,
    this.lastName,
    this.email,
    this.phone,
    this.comment,
    this.userRole,
    this.languageCode,
    this.title,
    this.imageLocation,
    this.employeeNumber,
    this.disabled,
    this.hasProject,
  });

  factory User.fromJson(json) {
    if (json == null) return null;
    return User._fromJson(json);
  }

  User._fromJson(json)
      : this.id = json['id'],
        this.firstName = json['firstName'],
        this.lastName = json['lastName'],
        this.email = json['email'],
        this.phone = json['phone'],
        this.comment = json['comment'],
        this.userRole = json['userRole'],
        this.languageCode = json['languageCode'],
        this.title = json['title'],
        this.imageLocation = json['imageLocation'],
        this.employeeNumber = json['employeeNumber'],
        this.disabled = json['disabled'],
        this.hasProject = json['hasProject'],
        this.hasAbsence = json['hasAbsence'],
        this.hasAbsenceRequest = json['hasAbsenceRequest'],
        this.client = Client.fromJson(json['client']),
        this.projectRole = ProjectRole.fromJson(json['projectRole']);

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'firstName': this.firstName,
        'lastName': this.lastName,
        'email': this.email,
        'phone': this.phone,
        'comment': this.comment,
        'userRole': this.userRole,
        'languageCode': this.languageCode,
        'title': this.title,
        'imageLocation': this.imageLocation,
        'employeeNumber': this.employeeNumber,
        'disabled': this.disabled,
      };

  // Returns a string representation of the users name and employee-number
  String get displayName =>
      (firstName ?? '') +
      ' ' +
      (lastName ?? '') +
      (employeeNumber != null ? ' ' + employeeNumber.toString() : '');
}
