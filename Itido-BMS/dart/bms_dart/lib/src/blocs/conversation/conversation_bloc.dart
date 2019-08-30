import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/src/models/conversation.dart';
import 'package:bms_dart/src/models/conversation_user.dart';
import 'package:bms_dart/src/models/message.dart';
import 'package:dart_packages/streamer.dart';
import './bloc.dart';

class ConversationBloc extends Bloc<ConversationEvent, ConversationState> {
  final String conversationId;
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final ConversationRepository _conversationRepository =
      repositoryProvider.conversationRepository();

  StreamSubscription<List<Message>> _messageSubscription;
  StreamSubscription<Conversation> _conversationUserSubscription;

  Streamer<String> _text = Streamer(seedValue: '');
  Streamer<String> get text => _text;

  int _messagesLength = 10;

  ConversationBloc(this.conversationId);

  @override
  ConversationState get initialState => Loading();

  @override
  void onTransition(
      Transition<ConversationEvent, ConversationState> transition) {
    var nextState = transition.nextState;
    if (nextState is Loaded) {
      _setMessageSeen(nextState.messages, nextState.conversation);
    }

    super.onTransition(transition);
  }

  @override
  Stream<ConversationState> mapEventToState(
    ConversationEvent event,
  ) async* {
    if (event is Fetch) {
      try {
        if (event.more) {
          _messagesLength += 10;
        }
        _messageSubscription?.cancel();
        _messageSubscription = _conversationRepository
            .fetchMessages(conversationId, _messagesLength)
            .listen((_messaged) {
          dispatch(FetchedMessages(messages: _messaged));
        });

        _conversationUserSubscription?.cancel();
        _conversationUserSubscription = _conversationRepository
            .fetchConversation(conversationId)
            .listen((_conversationUsers) {
          dispatch(FetchedConversation(conversation: _conversationUsers));
        });
      } catch (_) {
        yield Failure();
      }
    }

    if (event is FetchedMessages) {
      var messages = event.messages
        ..sort((m1, m2) => m2.senderTime.compareTo(m1.senderTime));
      var oldState = currentState;

      if (oldState is Loaded) {
        yield oldState.copyWith(
            messages: event.messages,
            adding: oldState.adding
              ..removeWhere((a) => event.messages.any((m) =>
                  m.text == a.text && m.imageFile?.url == a.imageFile?.url)));
      } else {
        yield Loaded(
            messages: messages,
            adding: [],
            failedAdding: [],
            conversation: null,
            loadedState: LoadedState.Pending);
      }
    }
    if (event is FetchedConversation) {
      var conversation = event.conversation;
      var oldState = currentState;

      if (oldState is Loaded) {
        yield oldState.copyWith(conversation: event.conversation);
      } else {
        yield Loaded(
            messages: [],
            adding: [],
            failedAdding: [],
            conversation: conversation,
            loadedState: LoadedState.Pending);
      }
    }

    if (event is Add && text.value.length > 0) {
      var newMessage = Message(
        text: text.value,
        senderId: _authenticationRepository.getUserId(),
        senderTime: DateTime.now(),
      );

      text.update('');

      var oldState = currentState;
      if (oldState is Loaded) {
        yield oldState.copyWith(
          loadedState: LoadedState.Pending,
          adding: oldState.adding..add(newMessage),
        );
      } else {
        yield Loaded(
          messages: [],
          adding: [newMessage],
          failedAdding: [],
          conversation: null,
          loadedState: LoadedState.Pending,
        );
      }

      _conversationRepository.sendMessage(conversationId, newMessage);
      //     .then((success) {
      //   dispatch(Added(success: success));
      // });
    }

    if (event is AddImage) {
      var newMessage = Message(
        senderId: _authenticationRepository.getUserId(),
        senderTime: DateTime.now(),
      );

      _conversationRepository.sendMessage(conversationId, newMessage,
          imageFile: event.imageFile);
    }

    // if (event is Added) {
    //   var oldState = currentState;
    //   var newLoadedState =
    //       event.success ? LoadedState.Success : LoadedState.Pending;

    //   if (newLoadedState == LoadedState.Success) {
    //     text.update('');
    //   }

    //   if (oldState is Loaded) {
    //     yield oldState.copyWith(loadedState: newLoadedState);
    //   } else {
    //     yield Loaded(
    //       adding: [],
    //       failedAdding: [],
    //       messages: [],
    //       conversation: null,
    //       loadedState: newLoadedState,
    //     );
    //   }
    // }

    // if (event is Cleared) {
    //   var oldState = currentState;
    //   if (oldState is Loaded) {
    //     yield oldState.copyWith(loadedState: LoadedState.Pending);
    //   } else {
    //     yield Loaded(
    //       adding: [],
    //       failedAdding: [],
    //       messages: [],
    //       conversation: null,
    //       loadedState: LoadedState.Pending,
    //     );
    //   }
    // }
  }

  void _setMessageSeen(List<Message> messages, Conversation conversation) {
    if (messages.length > 0) {
      var latestMessageId = messages.first.id;
      var thisConversationUser = conversation?.users?.firstWhere(
          (tcu) => tcu.id == _authenticationRepository.getUserId(),
          orElse: () => null);
      if (thisConversationUser == null ||
          thisConversationUser.latestMessageSeenId != latestMessageId) {
        _conversationRepository.setMessageSeen(conversationId, latestMessageId);
      }
    }
  }

  Message _previousMessage(Message message) {
    var state = currentState;
    if (state is Loaded) {
      if (state.messages.length < 2) return null;
      var index = state.messages.indexOf(message);
      if (index == state.messages.length - 1) return null;
      return state.messages[index + 1];
    }
    return null;
  }

  Message _nextMessage(Message message) {
    var state = currentState;
    if (state is Loaded) {
      if (state.messages.length < 2) return null;
      var index = state.messages.indexOf(message);
      if (index == 0) return null;
      return state.messages[index - 1];
    }
    return null;
  }

  String senderName(Message message) {
    var state = currentState;
    if (state is Loaded) {
      var sender = state.conversation?.users
          ?.firstWhere((cu) => cu.id == message.senderId, orElse: () => null);

      return sender?.firstName?.substring(0, 1) ?? '?';
    }
    return '?';
  }

  bool sentByUser(Message message) =>
      _authenticationRepository.getUserId() == message.senderId;

  bool showSender(Message message) {
    if (sentByUser(message)) return false;
    var next = _nextMessage(message);
    if (next != null && next.senderId == message.senderId) return false;
    return true;
  }

  bool showSendTime(Message message) {
    var previous = _previousMessage(message);
    if (previous != null &&
        previous.senderTime.difference(message.senderTime).abs() <
            Duration(minutes: 10)) return false;
    return true;
  }

  bool showSendDate(Message message) {
    var previous = _previousMessage(message);
    if (previous != null &&
        (previous.senderTime.day == message.senderTime.day &&
            previous.senderTime.month == message.senderTime.month &&
            previous.senderTime.year == message.senderTime.year)) return false;
    return true;
  }

  List<ConversationUser> usersSeenThis(Message message) {
    //print('cu.latestMessageSeenId: ${cu.latestMessageSeenId}');

    var state = currentState;
    if (state is Loaded) {
      return (state.conversation?.users ?? [])
          .where((cu) =>
              cu.latestMessageSeenId == message.id &&
              cu.id != _authenticationRepository.getUserId())
          .toList();
    }
    return [];
  }

  @override
  void dispose() {
    _messageSubscription?.cancel();
    _conversationUserSubscription?.cancel();
    super.dispose();
  }
}
