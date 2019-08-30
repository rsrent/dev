import 'package:bms_dart/models.dart';
import 'package:bms_dart/conversation_list_bloc.dart';
import 'package:bms_flutter/src/language/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class ConversationSearchDelegate extends SearchDelegate {
  final ConversationListBloc bloc;
  final Function(Conversation) onSelect;

  ConversationSearchDelegate(this.bloc, this.onSelect);

  @override
  List<Widget> buildActions(BuildContext context) {
    return [
      IconButton(
        icon: Icon(Icons.clear),
        onPressed: () {
          query = '';
        },
      ),
    ];
  }

  @override
  Widget buildLeading(BuildContext context) {
    return IconButton(
      icon: Icon(Icons.arrow_back),
      onPressed: () {
        close(context, null);
      },
    );
  }

  @override
  Widget buildResults(BuildContext context) {
    return showHits(context);
  }

  @override
  Widget buildSuggestions(BuildContext context) {
    return showHits(context);
  }

  Widget showHits(BuildContext context) {
    //

    return BlocBuilder(
      bloc: bloc,
      builder: (BuildContext context, ListState<Conversation> state) {
        if (state is Loaded<Conversation>) {
          var lowercaseQuery = query.toLowerCase();
          var searchHits = state.items
              .where((c) => c
                  .getSearchStrings()
                  .any((s) => s.toLowerCase().contains(lowercaseQuery)))
              .toList();

          if (searchHits.length > 0) {
            return ListView.builder(
              itemCount: searchHits.length,
              itemBuilder: (context, index) {
                var result = searchHits[index];
                return ListTile(
                  title: Text(result.getName()),
                  onTap: () => onSelect(result),
                );
              },
            );
          }
        }
        return Center(child: Text(Translations.of(context).infoNoResults));
      },
    );
  }
}
