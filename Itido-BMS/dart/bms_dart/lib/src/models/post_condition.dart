class PostCondition {
  PostConditionType postConditionType;
  String postConditionValue;
  dynamic postConditionSubject;

  PostCondition({
    this.postConditionType,
    this.postConditionValue,
  });

  Map<String, dynamic> toMap() => {
        'postConditionType': postConditionType.index,
        'postConditionValue': postConditionValue,
      };
}

enum PostConditionType {
  HasRole,
  AtProject,
}
