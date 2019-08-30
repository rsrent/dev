import 'package:bms_dart/models.dart';
import 'package:firebase_messaging/firebase_messaging.dart';
import 'authentication_api.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'dart:async';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'package:cloud_firestore/cloud_firestore.dart';
import 'image_controller.dart';
import 'query_client_controller.dart';

class ConversationFirestore extends ConversationSource {
  String path = '${api.path}/api/Firestore';

  final http.Client _client;
  QueryClientController<FirestoreConversation> _queryClient;

  @override
  void dispose() {
    _client.close();
    _queryClient.close();
  }

  final AuthenticationRepository _authenticationApi;
  CollectionReference get _ref => Firestore.instance
      .collection('organizations')
      .document('${_authenticationApi.getOrganizationId()}')
      .collection('conversations');

  ConversationFirestore(
    this._authenticationApi, {
    http.Client client,
  }) : this._client = client ?? http.Client() {
    _queryClient = QueryClientController(
      converter: (json) => FirestoreConversation.fromJson(json),
      client: client,
      getHeaders: () => api.headers(),
    );
  }

  @override
  Stream<List<Conversation>> fetchConversations() {
    print('_authenticationApi.isAdmin()');
    print(_authenticationApi.isAdmin());

    if (_authenticationApi.isAdmin()) {
      return _ref.snapshots().transform(_conversationQuerySnapshotTransformer);
    }

    return _ref
        .where('users.${_authenticationApi.getUserId()}.active',
            isEqualTo: true)
        .snapshots()
        .transform(_conversationQuerySnapshotTransformer);
  }

  Stream<List<Conversation>> fetchConversationsWithUsers(List<int> userIds,
      {bool preciesly = false}) {
    Query queryWithUser(Query q, int userId) {
      return q.where('users.$userId.active', isEqualTo: true);
    }

    var query = _ref.where('users.${_authenticationApi.getUserId()}.active',
        isEqualTo: true);
    userIds.forEach((uId) {
      query = queryWithUser(query, uId);
    });

    if (preciesly) {
      query = query.where('userCount', isEqualTo: userIds.length + 1);
    }

    return query.snapshots().transform(_conversationQuerySnapshotTransformer);
  }

  @override
  Stream<List<Message>> fetchMessages(String conversationId, int count) {
    return _ref
        .document(conversationId)
        .collection('messages')
        .orderBy('senderTime', descending: true)
        .limit(count)
        .snapshots()
        .transform(_messageQuerySnapshotTransformer);
  }

  @override
  Stream<Conversation> fetchConversation(String conversationId) {
    return _ref
        .document(conversationId)
        .snapshots()
        .transform(_conversationDocumentSnapshotTransformer);
  }

  // @override
  // Stream<List<ConversationUser>> fetchConversationUsers(String conversationId) {
  //   return _ref
  //       .document(conversationId)
  //       .collection('conversationUsers')
  //       .snapshots()
  //       .transform(_conversationUserQuerySnapshotTransformer);
  // }

  @override
  Future<bool> sendMessage(String conversationId, Message message,
      {ImageFile imageFile}) async {
    try {
      if (imageFile != null) {
        print('adding image!!');
        var _imageFile = await ImageController.saveImage(imageFile);
        message.imageFile = _imageFile;
        print('message.url: ${_imageFile.url}');
      }

      print('sendMessage: ' + '$path/PostMessage/$conversationId');

      print('message: ' + json.encode(message.toMap()));

      var response = await _client.post(
        '$path/PostMessage/$conversationId',
        headers: api.headers(),
        body: json.encode(message.toMap()),
      );

      if (response.statusCode == 200 && response.body != null) {
        bool result = json.decode(response.body);
        return result;
      }

      // (await _ref
      //         .document(conversationId)
      //         .collection('messages')
      //         .add(message.toMap()))
      //     .documentID;

      // await _ref.document(conversationId).updateData({
      //   'latestMessage': message.toMap(),
      //   'latestUpdateTime': message.senderTime,
      // });

      // return true;
    } catch (e) {
      print("sendMessage ERROR: $e");
    }
    return false;
  }

  @override
  Future<bool> setMessageSeen(String conversationId, String messageId) async {
    try {
      await _ref.document(conversationId).setData(
        {
          'users': {
            '${_authenticationApi.getUserId()}': {
              'latestMessageSeenId': messageId
            }
          }
        },
        merge: true,
      );
      return true;
    } catch (e) {}
    return false;
  }

  @override
  Future<QueryResult<List<FirestoreConversation>>>
      fetchFirestoreConversations() async {
    return _queryClient.getMany('$path/GetFirestoreConversations');
  }

  final _conversationQuerySnapshotTransformer =
      StreamTransformer<QuerySnapshot, List<Conversation>>.fromHandlers(
    handleData: (snapshot, sink) {
      if (snapshot != null) {
        sink.add(List<Conversation>.from(snapshot.documents
            .map((document) => document != null
                ? (Conversation.fromJson(document.data)
                  ..id = document.documentID)
                : null)
            .where((c) => c != null)
            .toList()));
      } else {
        sink.addError('No conversations to load');
      }
    },
  );
  final _messageQuerySnapshotTransformer =
      StreamTransformer<QuerySnapshot, List<Message>>.fromHandlers(
    handleData: (snapshot, sink) {
      if (snapshot != null) {
        sink.add(List<Message>.from(snapshot.documents.map((document) =>
            Message.fromJson(document.data)..id = document.documentID)));
      } else {
        sink.addError('No messages to load');
      }
    },
  );
  // final _conversationUserQuerySnapshotTransformer =
  //     StreamTransformer<QuerySnapshot, List<ConversationUser>>.fromHandlers(
  //   handleData: (snapshot, sink) {
  //     if (snapshot != null) {
  //       sink.add(List<ConversationUser>.from(snapshot.documents.map(
  //           (document) => ConversationUser.fromJson(document.data)
  //             ..id = int.parse(document.documentID))));
  //     } else {
  //       sink.addError('No messages to load');
  //     }
  //   },
  // );

  final _conversationDocumentSnapshotTransformer =
      StreamTransformer<DocumentSnapshot, Conversation>.fromHandlers(
    handleData: (snapshot, sink) {
      if (snapshot != null) {
        sink.add(Conversation.fromJson(snapshot.data));
      } else {
        sink.addError('No projects to load');
      }
    },
  );

  @override
  Future<String> createConversationWithUsers(List<int> userIds) async {
    try {
      var response = await _client.post(
        '$path/CreateConversationWithUsers',
        headers: api.headers(),
        body: json.encode(userIds),
      );

      if (response.statusCode == 200 && response.body != null) {
        String body = json.decode(response.body);
        return body;
      }
    } catch (e) {
      print(e);
    }
    return null;
  }
}
