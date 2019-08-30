import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class ConversationListBloc
    extends Bloc<ConversationListEvent, ListState<Conversation>> {
  final ConversationRepository _conversationRepository =
      repositoryProvider.conversationRepository();

  StreamSubscription<List<Conversation>> _streamSubscription;
  List<FirestoreConversation> _firestoreConversations;

  @override
  ListState<Conversation> get initialState => Loading<Conversation>();

  @override
  Stream<ListState<Conversation>> mapEventToState(
    ConversationListEvent event,
  ) async* {
    if (event is ConversationListFetchAll) {
      var result = await _conversationRepository.fetchFirestoreConversations();
      print('Got conversations');
      print(result);
      if (result is Ok<List<FirestoreConversation>>) {
        _firestoreConversations = result.value;
      }
      try {
        _streamSubscription?.cancel();
        _streamSubscription = _conversationRepository
            .fetchConversations()
            .listen((_conversations) {
          dispatch(ConversationListFetched(conversations: _conversations));
        });
      } catch (_) {
        yield Failure<Conversation>();
      }
    }

    if (event is ConversationListFetched) {
      var items = event.conversations;
      items.sort(
          (c1, c2) => (c2.latestUpdateTime).compareTo((c1.latestUpdateTime)));
      items.forEach((item) => item.firestoreConversation =
          _firestoreConversations.firstWhere((f) => f.conversationId == item.id,
              orElse: () => null));
      yield Loaded<Conversation>(items: items, refreshTime: DateTime.now());
    }
  }

  @override
  void dispose() {
    _streamSubscription?.cancel();
    super.dispose();
  }
}
