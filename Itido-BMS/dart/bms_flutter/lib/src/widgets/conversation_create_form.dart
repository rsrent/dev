import 'package:bms_dart/conversation_create_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class ConversationCreateForm extends StatefulWidget {
  @override
  _ConversationCreateFormState createState() => _ConversationCreateFormState();
}

class _ConversationCreateFormState extends State<ConversationCreateForm> {
  TextEditingController _controller = TextEditingController(text: '');

  @override
  Widget build(BuildContext context) {
    var bloc = BlocProvider.of<ConversationCreateBloc>(context);

    return BlocBuilder(
      bloc: bloc,
      builder: (BuildContext context, ConversationCreateState state) {
        if (state is ConversationInCreation) {
          return Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              Padding(
                padding: const EdgeInsets.only(left: 8, right: 8),
                child: Row(
                  children: <Widget>[
                    Expanded(
                      child: Wrap(
                          spacing: 4,
                          children: state.selectedUsers
                              .map<Widget>(
                                (u) => Chip(
                                  label: Text(u.displayName),
                                  onDeleted: () =>
                                      bloc.dispatch(UserRemoved(user: u)),
                                ),
                              )
                              .toList()),
                    ),
                    IconButton(
                      icon: Icon(Icons.send),
                      onPressed: () => bloc.dispatch(CreateConversation()),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.all(8),
                child: TextField(
                  controller: _controller,
                  onChanged: (v) =>
                      bloc.dispatch(SearchTextUpdated(searchText: v)),
                ),
              ),
              Expanded(
                child: ListView.builder(
                  itemCount: state.searchedUsers.length,
                  itemBuilder: (context, index) {
                    var u = state.searchedUsers[index];
                    return ListTile(
                      title: Text(u.displayName),
                      onTap: () {
                        _controller.clear();
                        bloc.dispatch(UserSelected(user: u));
                      },
                    );
                  },
                ),
              ),
            ],
          );
        }

        return Center(child: CircularProgressIndicator());
      },
    );
  }
}
