/*
public int ID { get; set; }
  public string NotiType { get; set; }
  public int SubjectID { get; set; }

  public DateTime SendTime { get; set; }
  public int? SenderID { get; set; }
  public virtual User Sender { get; set; }

  public int ReceiverID { get; set; }
  public virtual User Receiver { get; set; }
  public bool Seen { get; set; }
*/

class Noti {
  int id;
  String title;
  String body;
  String senderName;
  DateTime sendTime;
  int subjectId;
  int userId;
  String notiType;
  bool seen;

  Noti({
    this.id,
    this.title,
    this.body,
    this.senderName,
    this.sendTime,
    this.subjectId,
    this.userId,
    this.notiType,
    this.seen,
  });
  Noti.fromJson(json)
      : this.id = json['id'],
        this.title = json['title'],
        this.body = json['body'],
        this.senderName = json['senderName'],
        this.sendTime = DateTime.parse(json['sendTime']),
        this.subjectId = json['subjectID'],
        this.userId = json['userID'],
        this.notiType = json['notiType'],
        this.seen = json['seen'];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'title': this.title,
        'body': this.body,
        'senderName': this.senderName,
        'sendTime': this.sendTime.toString(),
        'subjectId': this.subjectId,
        'userId': this.userId,
        'notiType': this.notiType,
        'seen': this.seen,
      };
}
