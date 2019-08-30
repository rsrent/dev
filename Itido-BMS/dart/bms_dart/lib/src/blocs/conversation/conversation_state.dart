import 'package:bms_dart/conversation_list_bloc.dart' as prefix0;
import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

enum LoadedState { Pending, Adding, Success }

@immutable
abstract class ConversationState extends Equatable {
  ConversationState([List props = const []]) : super(props);
}

class Loading extends ConversationState {
  @override
  String toString() => 'Loading';
}

class Loaded extends ConversationState {
  final List<Message> messages;
  final List<Message> adding;
  final List<Message> failedAdding;
  final Conversation conversation;
  final LoadedState loadedState;
  Loaded({
    @required this.messages,
    @required this.adding,
    @required this.failedAdding,
    @required this.conversation,
    @required this.loadedState,
  }) : super([
          messages,
          adding,
          failedAdding,
          conversation,
          loadedState,
          DateTime.now(),
        ]);

  Loaded copyWith({
    List<Message> messages,
    List<Message> adding,
    List<Message> failedAdding,
    Conversation conversation,
    LoadedState loadedState,
  }) {
    return Loaded(
      messages: messages ?? this.messages,
      adding: adding ?? this.adding,
      failedAdding: failedAdding ?? this.failedAdding,
      conversation: conversation ?? this.conversation,
      loadedState: loadedState ?? this.loadedState,
    );
  }

  @override
  String toString() =>
      'Loaded { messages: ${messages.length}, conversation: ${conversation?.toMap()}, loadedState: $loadedState }';
}

class Failure extends ConversationState {
  @override
  String toString() => 'Failure';
}
