import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class ConversationEvent extends Equatable {
  ConversationEvent([List props = const []]) : super(props);
}

class Fetch extends ConversationEvent {
  final bool more;

  Fetch(this.more) : super([more]);
  @override
  String toString() => 'Fetch { more: $more }';
}

class FetchedMessages extends ConversationEvent {
  final List<Message> messages;

  FetchedMessages({@required this.messages}) : super([messages]);
  @override
  String toString() => 'Fetched { messages: ${messages.length} }';
}

class FetchedConversation extends ConversationEvent {
  final Conversation conversation;

  FetchedConversation({@required this.conversation}) : super([conversation]);
  @override
  String toString() => 'Fetched { conversation: ${conversation} }';
}

class Add extends ConversationEvent {
  @override
  String toString() => 'Add';
}

class AddImage extends ConversationEvent {
  final ImageFile imageFile;

  AddImage({@required this.imageFile}) : super([imageFile]);

  @override
  String toString() => 'AddImage { ${imageFile.file.path} }';
}

// class Added extends ConversationEvent {
//   final bool success;
//   Added({@required this.success}) : super([success]);
//   @override
//   String toString() => 'Added { success: $success }';
// }

// class Cleared extends ConversationEvent {
//   @override
//   String toString() => 'Cleared';
// }
