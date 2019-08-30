import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/post_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../../translations.dart';
import 'info_list_view.dart';

class PostList extends StatelessWidget {
  final Function(Post) onSelect;
  final Function(Post) onDelete;

  const PostList({Key key, this.onSelect, this.onDelete}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final postListBloc = BlocProvider.of<PostListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: postListBloc,
      builder: (context, ListState<Post> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          var posts = (state as Loaded).items;
          if (posts.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoPosts);
          }
          return ListView.builder(
            padding: EdgeInsets.all(0),
            itemBuilder: (BuildContext context, int index) {
              if (index == posts.length) {
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
                    postListBloc.dispatch(PostListFetch(more: true));
                  },
                );
              }
              return PostTile(
                post: posts[index],
                onSelect: onSelect,
              );
            },
            itemCount: posts.length + 1,
          );
        }
      },
    );
  }
}

class PostTile extends StatelessWidget {
  final Post post;
  final Function(Post) onSelect;

  const PostTile({
    Key key,
    @required this.post,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    var dateTimeString = Translations.of(context).dateTimeString(post.sendTime);
    return Container(
      margin: const EdgeInsets.all(8),
      padding: const EdgeInsets.only(top: 8, bottom: 8),
      color: Color(0xFFF1F9FF),
      child: ListTile(
        isThreeLine: true,
        title: Text(
          post.title,
          style: TextStyle(fontSize: 20),
        ),
        subtitle: Padding(
          padding: const EdgeInsets.only(top: 8),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              Text(
                post.body,
                // style: TextStyle(fontSize: 16),
              ),
              Divider(
                height: 24,
                color: Colors.transparent,
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: <Widget>[
                  Text(post.sender.displayName),
                  Text(dateTimeString),
                ],
              ),
            ],
          ),
        ),
        onTap: onSelect != null ? () => onSelect(post) : null,
      ),
    );
  }
}
