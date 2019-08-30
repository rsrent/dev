class ConversationUser {
  int id;
  String firstName;
  String lastName;
  String employeeNumber;
  String latestMessageSeenId;

  ConversationUser({
    this.id,
    this.firstName,
    this.lastName,
    this.employeeNumber,
    this.latestMessageSeenId,
  });

  ConversationUser.fromJson(json)
      : this.firstName = json['firstName'],
        this.lastName = json['lastName'],
        this.employeeNumber = '${json['employeeNumber']}',
        this.latestMessageSeenId = json['latestMessageSeenId'];

  Map<String, dynamic> toMap() => {
        'firstName': this.firstName,
        'lastName': this.lastName,
        'employeeNumber': this.employeeNumber,
        'latestMessageSeenId': this.latestMessageSeenId,
      };
}
