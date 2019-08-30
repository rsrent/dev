import 'package:bms_dart/models.dart';

import 'conversation_user.dart';
import 'message.dart';

class Conversation {
  String id;
  bool locked;
  String name;
  Message latestMessage;
  DateTime latestUpdateTime;
  List<ConversationUser> users;
  FirestoreConversation firestoreConversation;

  Conversation({
    this.id,
    this.name,
    this.locked,
    this.users,
    this.latestUpdateTime,
  });

  Conversation.fromJson(json)
      : this.id = json['id'],
        this.locked = json['locked'],
        this.name = json['name'],
        this.latestUpdateTime = json['latestUpdateTime'].toDate(),
        this.latestMessage = json['latestMessage'] != null
            ? Message.fromJson(json['latestMessage'])
            : null,
        this.users = json['users'] != null
            ? Map.castFrom(json['users']).entries.map((v) {
                return ConversationUser.fromJson(v.value)
                  ..id = int.parse(v.key);
              }).toList()
            : [];

  Map<String, dynamic> toMap() => {
        'id': this.id,
        'locked': this.locked,
        'name': this.name,
        'latestUpdateTime': this.latestUpdateTime,
        'users': this.users.fold<Map<String, dynamic>>(
            Map<String, Map<String, dynamic>>(),
            (m, cu) => m.putIfAbsent('${cu.id}', () => cu.toMap())),
      };

  @override
  String toString() {
    return '';
  }

  List<String> getSearchStrings() {
    List<String> strings = [];
    strings.add(name ?? '');
    strings.addAll(usersInfo().split(','));
    return strings;
  }

  String getName() {
    if (name == null) {
      return usersFirstNames();
    }
    return name;
  }

  String usersFirstNames() {
    var usersString;

    usersString = users.fold<String>('', (acc, u) => acc += u.firstName + ', ');

    if (usersString.length > 0) {
      usersString = usersString.substring(0, usersString.length - 2);
    }
    return usersString;
  }

  String usersInfo() {
    var usersString;

    usersString = users.fold<String>(
        '',
        (acc, u) => acc +=
            u.firstName + ' ' + u.lastName + ' ' + u.employeeNumber + ', ');

    if (usersString.length > 0) {
      usersString = usersString.substring(0, usersString.length - 2);
    }
    return usersString;
  }

  bool hadLatestMessage() => latestMessage != null;

  String getLastMessage() {
    var latestMessageSender = '';

    if (latestMessage != null) {
      var sender = users.firstWhere((u) => u.id == latestMessage.senderId,
          orElse: () => null);

      if (sender != null) {
        latestMessageSender = '${sender.firstName}: ';
      }
    }
    return '$latestMessageSender: ${latestMessage.text}';
  }

  // String getSendTime() {
  //   return '${DateFormat.MMMd().format(latestMessage.senderTime)} ${DateFormat.Hm().format(latestMessage.senderTime)}';
  // }
}
