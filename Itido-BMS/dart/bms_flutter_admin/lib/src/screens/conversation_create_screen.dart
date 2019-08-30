import 'package:bms_dart/conversation_create_bloc.dart';
import 'package:flutter/material.dart';
import 'package:bms_flutter/src/widgets/conversation_create_form.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'conversation_screen.dart';

class ConversationCreateScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      builder: (context) {
        return ConversationCreateBloc()..dispatch(Prepare());
      },
      child: Builder(
        builder: (context) {
          var bloc = BlocProvider.of<ConversationCreateBloc>(context);
          return BlocListener(
              bloc: bloc,
              listener: (BuildContext context, ConversationCreateState state) {
                if (state is ConversationCreated) {
                  var id = state.id;
                  Navigator.of(context).pop();
                  Navigator.of(context).push(MaterialPageRoute(
                    builder: (context) => ConversationScreen(
                      conversationId: id,
                    ),
                  ));
                }
              },
              child: Scaffold(
                appBar: AppBar(),
                body: ConversationCreateForm(),
              ));
        },
      ),
    );
  }
}
