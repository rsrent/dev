import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/conversation_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../translations.dart';
import 'image_viewer_widget.dart';
import 'info_list_view.dart';

class MessageList extends StatelessWidget {
  final Function(Message) onSelect;

  const MessageList({Key key, this.onSelect}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final conversationBloc = BlocProvider.of<ConversationBloc>(context);
    return AnimatedBlocBuilder(
      bloc: conversationBloc,
      builder: (context, ConversationState state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          if (state.messages.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoMessages);
          }

          var allMessages = []..addAll(state.adding)..addAll(state.messages);

          print('state.adding: ${state.adding}');
          print('state.messages: ${state.messages}');

          return ListView.builder(
            reverse: true,
            padding: EdgeInsets.only(top: 8, bottom: 8),
            itemBuilder: (BuildContext context, int index) {
              if (index == allMessages.length) {
                return InkWell(
                  child: Container(
                    height: 100,
                    child: Center(
                      child: Row(
                        mainAxisSize: MainAxisSize.min,
                        children: <Widget>[
                          Icon(
                            Icons.refresh,
                            color: Theme.of(context).primaryColor,
                          ),
                          Padding(
                            padding: const EdgeInsets.only(left: 16),
                            child: Text(
                              'Load more',
                              style: TextStyle(
                                color: Theme.of(context).primaryColor,
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  onTap: () {
                    conversationBloc.dispatch(Fetch(true));
                  },
                );
              }
              return MessageTile(
                message: allMessages[index],
                onSelect: onSelect,
                loading: state.adding.contains(allMessages[index]),
              );
            },
            itemCount: allMessages.length + 1,
          );
        }
      },
    );
  }
}

class MessageTile extends StatelessWidget {
  final Message message;
  final Function(Message) onSelect;
  final bool loading;

  const MessageTile({
    Key key,
    @required this.message,
    this.onSelect,
    this.loading = false,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final conversationBloc = BlocProvider.of<ConversationBloc>(context);
    var sentByUser = conversationBloc.sentByUser(message);
    var showSender = conversationBloc.showSender(message);
    var showSendTime = conversationBloc.showSendTime(message);
    var showSendDate = conversationBloc.showSendDate(message);
    var usersSeenThis = loading ? [] : conversationBloc.usersSeenThis(message);
    var senderName = conversationBloc.senderName(message);

    var sendDate = Translations.of(context).dateString(message.senderTime);
    var sendTime = Translations.of(context).timeString(message.senderTime);

    return Column(
      children: <Widget>[
        if (showSendDate || showSendTime)
          Text(
              '${showSendDate ? sendDate : ''} ${showSendTime ? sendTime : ''}'),
        Row(
          children: <Widget>[
            if (sentByUser)
              Expanded(
                child: Container(),
                flex: 1,
              ),
            if (!sentByUser)
              Padding(
                child: showSender
                    ? CircleAvatar(
                        radius: 20,
                        child: Text(senderName),
                      )
                    : SizedBox(
                        width: 40,
                      ),
                padding: EdgeInsets.only(left: 8),
              ),
            Expanded(
              child: Align(
                alignment:
                    sentByUser ? Alignment.centerRight : Alignment.centerLeft,
                child: message.imageFile != null
                    ? showImage(context, message.imageFile)
                    : Container(
                        margin: const EdgeInsets.all(8),
                        padding: const EdgeInsets.all(8),
                        child: Text(
                          message.text,
                          style: TextStyle(
                              fontSize: 16,
                              color: sentByUser ? Colors.white : null),
                        ),
                        //  message.url != null
                        //     ? ImageViewerWidget(ImageFile(url: message.url))
                        //     : Text(
                        //         message.text,
                        //         style: TextStyle(
                        //             fontSize: 16,
                        //             color: sentByUser ? Colors.white : null),
                        //       ),
                        decoration: BoxDecoration(
                          borderRadius: BorderRadius.all(Radius.circular(8)),
                          color: sentByUser ? Colors.blue : Colors.grey[300],
                        ),
                      ),
              ),
              flex: 3,
            ),
            if (loading)
              Container(
                margin: EdgeInsets.only(right: 16),
                child: CircularProgressIndicator(),
                width: 20,
                height: 20,
              ),
            if (!sentByUser)
              Expanded(
                child: Container(),
                flex: 1,
              ),
          ],
        ),
        Row(
          children: <Widget>[
            Expanded(
              child: Container(),
            ),
            for (int i = 0; i < usersSeenThis.length; i++)
              Padding(
                padding: const EdgeInsets.all(2),
                child: CircleAvatar(
                  maxRadius: 8,
                  child: Text(
                    usersSeenThis[i].firstName?.substring(0, 1) ?? '?',
                    style: TextStyle(fontSize: 8),
                  ),
                ),
              ),
          ],
        )
      ],
    );

    // if (conversationBloc.sentByUser(message)) {
    //   return Align(
    //     alignment: Alignment.centerLeft,
    //     widthFactor: 0.7,
    //     child: Container(
    //       color: Colors.lightBlue,
    //       child: Text(message.text),
    //     ),
    //   );
    // } else {
    //   return Align(
    //     alignment: Alignment.centerRight,
    //     widthFactor: 0.7,
    //     child: Container(
    //       child: Text(message.text),
    //     ),
    //   );
    // }

    // return ListTile(
    //   title: Text('${message.text}'),
    //   onTap: onSelect != null ? () => onSelect(message) : null,
    // );
  }

  Widget showImage(BuildContext context, ImageFile imageFile) {
    return GestureDetector(
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: ImageViewerWidget(imageFile),
      ),
      onTap: () {
        Navigator.of(context).push(MaterialPageRoute(builder: (context) {
          return Scaffold(
            appBar: AppBar(),
            body: Center(child: ImageViewerWidget(imageFile)),
          );
        }));
      },
    );
  }
}
