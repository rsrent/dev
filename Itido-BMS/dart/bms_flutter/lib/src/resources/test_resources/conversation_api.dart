/*
import 'package:bms_dart/models.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/src/models/conversation.dart';
import 'package:bms_dart/src/models/message.dart';

import 'client_faker.dart';

class ConversationApi extends ConversationSource {
  ClientFaker _conversationClient = ClientFaker<Conversation>(
      generator: (i) => Conversation(
            id: '$i',
            name: 'Test',
          ));

  ClientFaker _messageClient = ClientFaker<Message>(
      generator: (i) => Message(
            id: '$i',
            text:
                'Hej med dig Hej med dig Hej med dig Hej med dig Hej med dig Hej med dig Hej med dig Hej med dig Hej med dig Hej med dig Hej med dig',
            // text: 'Hej med dig ',
            senderTime:
                DateTime.now().subtract(Duration(minutes: i * i * i * i)),
            senderId: i % 3 + i % 2,
          ));
  ClientFaker _conversationUserClient = ClientFaker<ConversationUser>(
      generator: (i) => ConversationUser(
            id: i,
            name: 'Tobias',
            latestMessageSeenId: '0',
          ));

  @override
  void dispose() {
    // TODO: implement dispose
  }

  @override
  Stream<List<Conversation>> fetchConversations() async* {
    yield (await _conversationClient.getMany());
  }

  @override
  Stream<List<Message>> fetchMessages(String chatId) async* {
    yield (await _messageClient.getMany());
  }

  @override
  Future<bool> writeMessage(String chatId, Message message) async {
    await _messageClient.add(message);
    return true;
  }

  @override
  Stream<List<ConversationUser>> fetchConversationUsers(String chatId) async* {
    yield (await _conversationUserClient.getMany());
  }

  @override
  Future<bool> setMessageSeen(String conversationId, String messageId) {
    // TODO: implement setMessageSeen
    return null;
  }

  @override
  Stream<Conversation> fetchConversation(String conversationId) {
    // TODO: implement fetchConversation
    return null;
  }

  @override
  Stream<List<Conversation>> fetchConversationsWithUsers(List<int> userIds,
      {bool preciesly = false}) {
    // TODO: implement fetchConversationsWithUsers
    return null;
  }
}
*/
