import 'package:bms_flutter/src/widgets/message_list.dart';
import 'package:bms_dart/conversation_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'image_picker.dart';

class ConversationForm extends StatefulWidget {
  final String conversationId;

  const ConversationForm({Key key, @required this.conversationId})
      : super(key: key);

  @override
  ConversationFormState createState() => ConversationFormState();
}

class ConversationFormState extends State<ConversationForm>
    with TickerProviderStateMixin {
  TextEditingController _textEditingController;
  AnimationController _animationController;
  Animation _animation;

  @override
  void initState() {
    super.initState();

    _animationController =
        AnimationController(vsync: this, duration: Duration(milliseconds: 200));

    _animation = CurvedAnimation(
        parent: _animationController,
        curve: Curves.easeIn,
        reverseCurve: Curves.easeOut);

    _textEditingController = TextEditingController();
  }

  @override
  Widget build(BuildContext context) {
    var _bloc = BlocProvider.of<ConversationBloc>(context);

    return BlocListener(
        bloc: _bloc,
        listener: (BuildContext context, ConversationState state) {
          // if (state is Loaded) {
          //   if (state.loadedState == LoadedState.Success) {
          //     _textEditingController.clear();
          //     _bloc.dispatch(Cleared());
          //   }
          // }
        },
        child: _buildBody(context, _bloc));
  }

  Widget _buildBody(BuildContext context, ConversationBloc _bloc) {
    return BlocBuilder(
      bloc: _bloc,
      builder: (BuildContext context, ConversationState state) {
        return Column(
          children: <Widget>[
            Expanded(child: MessageList()),
            SafeArea(
              left: false,
              right: false,
              top: false,
              child: Container(
                child: AnimatedBuilder(
                  animation: _animationController,
                  builder: (context, snapshot) {
                    return Padding(
                      padding: const EdgeInsets.only(left: 8.0),
                      child: Row(
                        children: <Widget>[
                          ClipRect(
                            child: Align(
                              widthFactor: 1 - _animation.value,
                              child: Padding(
                                padding: const EdgeInsets.fromLTRB(0, 8, 8, 8),
                                child: Opacity(
                                  opacity: 1 - _animation.value,
                                  child: Row(
                                    children: <Widget>[
                                      IconButton(
                                        icon: Icon(
                                          Icons.camera_alt,
                                          color: Colors.grey,
                                        ),
                                        onPressed: () async {
                                          var imagefile =
                                              await displayImagePicker(context);
                                          if (imagefile != null) {
                                            _bloc.dispatch(
                                                AddImage(imageFile: imagefile));
                                          }
                                        },
                                      ),
                                      // IconButton(
                                      //   icon: Icon(
                                      //     Icons.photo_library,
                                      //     color: Colors.grey,
                                      //   ),
                                      //   onPressed: () {},
                                      // ),
                                    ],
                                  ),
                                ),
                              ),
                            ),
                          ),
                          ClipRect(
                            child: Align(
                              widthFactor: _animation.value,
                              child: Padding(
                                padding: const EdgeInsets.fromLTRB(0, 8, 8, 8),
                                child: Opacity(
                                  opacity: _animation.value,
                                  child: Row(
                                    children: <Widget>[
                                      IconButton(
                                        icon: Icon(
                                          Icons.arrow_forward_ios,
                                          color: Colors.grey,
                                        ),
                                        onPressed: () {
                                          _animationController.reverse();
                                        },
                                      ),
                                    ],
                                  ),
                                ),
                              ),
                            ),
                          ),
                          Expanded(
                            child: Column(
                              children: <Widget>[
                                Padding(
                                  padding: const EdgeInsets.only(left: 8.0),
                                  child: StreamBuilder<String>(
                                    stream: _bloc.text.stream,
                                    builder: (context, snapshot) {
                                      return TextField(
                                        controller: _textEditingController,
                                        onChanged: _bloc.text.update,
                                        minLines: 1,
                                        maxLines: _animation.value > 0 ? 4 : 1,
                                        onTap: () {
                                          _animationController.forward();
                                        },
                                        onEditingComplete: () {
                                          _animationController.reverse();
                                        },
                                        decoration:
                                            InputDecoration(filled: true),
                                      );
                                    },
                                  ),
                                ),
                              ],
                            ),
                          ),
                          IconButton(
                            icon: Icon(
                              Icons.send,
                              color: Colors.grey,
                            ),
                            onPressed: (state is Loaded &&
                                    state.loadedState == LoadedState.Pending)
                                ? () {
                                    _bloc.dispatch(Add());
                                    _textEditingController.clear();
                                  }
                                : null,
                          ),
                        ],
                      ),
                    );
                  },
                ),
              ),
            )
          ],
        );
      },
    );
  }
}
