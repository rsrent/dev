import 'package:bms_dart/src/models/image_file.dart';

class Message {
  String id;
  String text;
  ImageFile imageFile;
  int senderId;
  DateTime senderTime;

  Message({
    this.id,
    this.text,
    this.imageFile,
    this.senderId,
    this.senderTime,
  });

  Message.fromJson(json)
      : this.id = json['id'],
        this.text = json['text'],
        this.senderId = json['senderId'],
        this.senderTime = json['senderTime'].toDate(),
        this.imageFile = json['url'] != null
            ? (ImageFile(url: json['url'])
              ..cacheName = 'message_${json['senderTime']}')
            : null;

  Map<String, dynamic> toMap() => {
        //'id': this.id,
        'text': this.text,
        'url': this.imageFile?.url,
        //'senderId': this.senderId,
        //'senderTime': this.senderTime,
      };
}
