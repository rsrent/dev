import 'dart:async';

import 'package:bms_flutter/src/widgets/conversation_form.dart';
import 'package:bms_dart/conversation_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class ConversationScreen extends StatefulWidget {
  final String conversationId;

  const ConversationScreen({Key key, @required this.conversationId})
      : super(key: key);

  @override
  ConversationScreenState createState() => ConversationScreenState();
}

class ConversationScreenState extends State<ConversationScreen> {
  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      builder: (context) {
        return ConversationBloc(widget.conversationId)..dispatch(Fetch(false));
      },
      child: Scaffold(
        appBar: AppBar(),
        body: ConversationForm(
          conversationId: widget.conversationId,
        ),
      ),
    );
  }
}
