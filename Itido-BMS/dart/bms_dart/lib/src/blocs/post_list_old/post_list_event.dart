import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/post.dart';

@immutable
abstract class PostListEvent extends Equatable {
  PostListEvent([List props = const []]) : super(props);
}

class PostListFetch extends PostListEvent {
  final bool more;
  PostListFetch({@required this.more}) : super([more]);
  @override
  String toString() => 'PostListFetch';
}

class PostListFetched extends PostListEvent {
  final List<Post> conversations;

  PostListFetched({@required this.conversations}) : super([conversations]);
  @override
  String toString() =>
      'PostListFetched { conversations: ${conversations.length} }';
}
