import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class ConversationListEvent extends Equatable {
  ConversationListEvent([List props = const []]) : super(props);
}

class ConversationListFetchAll extends ConversationListEvent {
  @override
  String toString() => 'ConversationListFetchAll';
}

class ConversationListFetched extends ConversationListEvent {
  final List<Conversation> conversations;

  ConversationListFetched({@required this.conversations})
      : super([conversations]);
  @override
  String toString() =>
      'ConversationListFetched { conversations: ${conversations.length} }';
}
