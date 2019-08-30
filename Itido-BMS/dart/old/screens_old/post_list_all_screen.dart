import 'package:bms_flutter/widgets.dart';
import 'package:bms_dart/post_list_bloc.dart';
import 'package:flutter/material.dart';
import 'post_create_screen.dart';

class PostListAllScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return BlocListScreen(
      blocBuilder: (context) =>
          PostListBloc()..dispatch(PostListFetch(more: false)),
      onRefresh: (bloc) => bloc.dispatch(PostListFetch(more: false)),
      body: PostList(),
      appBar: AppBar(
        title: Text('TEST'),
      ),
      floatingActionButton: FloatingActionButton(
        child: Icon(Icons.add),
        onPressed: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (context) => PostCreateScreen(),
            ),
          );
        },
      ),
    );
  }
}
