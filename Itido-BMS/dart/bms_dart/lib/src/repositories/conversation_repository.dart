import 'dart:async';

import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';

import '../models/conversation.dart';
import '../models/message.dart';
import '../models/conversation_user.dart';
import 'source.dart';

abstract class ConversationSource extends Source {
  Future<QueryResult<List<FirestoreConversation>>>
      fetchFirestoreConversations();
  Stream<List<Conversation>> fetchConversations();
  Stream<List<Message>> fetchMessages(String conversationId, int count);
  Stream<Conversation> fetchConversation(String conversationId);
  Stream<List<Conversation>> fetchConversationsWithUsers(List<int> userIds,
      {bool preciesly = false});
  Future<bool> sendMessage(String conversationId, Message message,
      {ImageFile imageFile});

  Future<bool> setMessageSeen(String conversationId, String messageId);

  Future<String> createConversationWithUsers(List<int> userIds);
}

class ConversationRepository extends ConversationSource {
  final ConversationSource source;

  ConversationRepository(this.source);

  @override
  Future<QueryResult<List<FirestoreConversation>>>
      fetchFirestoreConversations() => source.fetchFirestoreConversations();
  Stream<List<Conversation>> fetchConversations() =>
      source.fetchConversations();

  @override
  Stream<Conversation> fetchConversation(String conversationId) =>
      source.fetchConversation(conversationId);

  @override
  Stream<List<Conversation>> fetchConversationsWithUsers(List<int> userIds,
          {bool preciesly = false}) =>
      source.fetchConversationsWithUsers(userIds, preciesly: preciesly);

  @override
  Stream<List<Message>> fetchMessages(String conversationId, int count) =>
      source.fetchMessages(conversationId, count);

  // @override
  // Stream<List<ConversationUser>> fetchConversationUsers(
  //         String conversationId) =>
  //     source.fetchConversationUsers(conversationId);

  @override
  Future<bool> sendMessage(String conversationId, Message message,
          {ImageFile imageFile}) =>
      source.sendMessage(conversationId, message, imageFile: imageFile);
  @override
  Future<bool> setMessageSeen(String conversationId, String messageId) =>
      source.setMessageSeen(conversationId, messageId);

  @override
  void dispose() {
    source?.dispose();
  }

  @override
  Future<String> createConversationWithUsers(List<int> userIds) =>
      source.createConversationWithUsers(userIds);
}
