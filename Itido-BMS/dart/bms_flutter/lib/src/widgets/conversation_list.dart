import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/components/project_name.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/conversation_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'info_list_view.dart';

class ConversationList extends StatelessWidget {
  final Function(Conversation) onSelect;

  const ConversationList({Key key, this.onSelect}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final conversationListBloc = BlocProvider.of<ConversationListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: conversationListBloc,
      builder: (context, ListState<Conversation> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<Conversation>) {
          if (state.items.isEmpty) {
            return InfoListView(
                info: Translations.of(context).infoNoConversations);
          }
          return ListView.separated(
            padding: EdgeInsets.only(top: 8, bottom: 80),
            itemBuilder: (BuildContext context, int index) {
              return ConversationTile(
                conversation: state.items[index],
                onSelect: onSelect,
              );
            },
            separatorBuilder: (context, index) {
              return Divider();
            },
            itemCount: state.items.length,
          );
        }
      },
    );
  }
}

class ConversationTile extends StatelessWidget {
  final Conversation conversation;
  final Function(Conversation) onSelect;

  const ConversationTile({
    Key key,
    @required this.conversation,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    var dateTimeString =
        Translations.of(context).dateTimeString(conversation.latestUpdateTime);

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: <Widget>[
        if (conversation.firestoreConversation != null)
          Padding(
            padding: EdgeInsets.only(left: 16),
            child: ProjectName(
                project: conversation.firestoreConversation.project),
          ),
        ListTile(
          title: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: <Widget>[
              Text('${conversation.getName()}'),
              if (conversation.locked) Icon(Icons.lock),
            ],
          ),
          onTap: onSelect != null ? () => onSelect(conversation) : null,
          subtitle: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              conversation.hadLatestMessage()
                  ? Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: <Widget>[
                        Text(conversation.getLastMessage()),
                        Text(dateTimeString),
                      ],
                    )
                  : Text(Translations.of(context).infoNoMessages),
            ],
          ),
        ),
      ],
    );
  }
}
