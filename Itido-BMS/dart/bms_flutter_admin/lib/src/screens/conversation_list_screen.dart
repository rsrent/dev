import 'package:bms_dart/models.dart';
import 'package:bms_flutter_admin/src/screens/conversation_create_screen.dart';

import '../screens/conversation_screen.dart';
import 'package:bms_flutter/src/widgets/conversation_list.dart';
import 'package:bms_flutter/src/widgets/conversation_search_delegate.dart';

import 'package:bms_dart/conversation_list_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class ConversationListScreen extends StatefulWidget {
  const ConversationListScreen({Key key}) : super(key: key);

  @override
  ConversationListScreenState createState() => ConversationListScreenState();
}

class ConversationListScreenState extends State<ConversationListScreen> {
  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      builder: (context) {
        return ConversationListBloc()..dispatch(ConversationListFetchAll());
      },
      child: Builder(
        builder: (context) {
          return Scaffold(
            appBar: AppBar(
              actions: <Widget>[
                IconButton(
                  icon: Icon(Icons.search),
                  onPressed: () {
                    var _bloc = BlocProvider.of<ConversationListBloc>(context);
                    showSearch(
                        context: context,
                        delegate: ConversationSearchDelegate(_bloc,
                            (conversation) => onSelect(context, conversation)));
                  },
                ),
                IconButton(
                  icon: Icon(Icons.create),
                  onPressed: () {
                    Navigator.of(context).push(MaterialPageRoute(
                      builder: (context) => ConversationCreateScreen(),
                    ));
                  },
                ),
              ],
            ),
            body: ConversationList(
              onSelect: (conversation) => onSelect(context, conversation),
            ),
          );
        },
      ),
    );
  }

  void onSelect(BuildContext context, Conversation conversation) {
    Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => ConversationScreen(
        conversationId: conversation.id,
      ),
    ));
  }
}
