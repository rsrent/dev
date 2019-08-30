class MoreWork {
  final int cleaningPlanID;
  final String description;
  final DateTime expectedTime;
  MoreWork.fromJson(json)
      : cleaningPlanID = json['cleaningPlanID'],
        description = json['description'],
        expectedTime = DateTime.parse(json['expectedCompletedTime']);
}
