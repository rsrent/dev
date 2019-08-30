import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/post.dart';
import 'package:bms_dart/repositories.dart';

class PostListBloc extends Bloc<PostListEvent, ListState<Post>>
    with Refreshable {
  final PostRepository _postRepository = repositoryProvider.postRepository();

  @override
  ListState<Post> get initialState => Loading<Post>();

  PostListBloc(this._refreshEvent) {
    refresh();
  }
  final PostListEvent Function() _refreshEvent;
  void refresh() => dispatch(_refreshEvent());

  int _fetchCount = 10;

  @override
  Stream<ListState<Post>> mapEventToState(
    PostListEvent event,
  ) async* {
    if (event is PostListFetch) {
      if (event.more) {
        _fetchCount += 10;
      }

      final items = await _postRepository.fetchLatestPosts(_fetchCount);
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }
  }
}
