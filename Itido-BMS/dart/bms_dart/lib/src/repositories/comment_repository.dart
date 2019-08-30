import 'dart:async';
import '../models/comment.dart';
import 'source.dart';

abstract class CommentSource extends Source {
  Future<Comment> fetch(int commentId);
  //Future<List<Comment>> fetchOfProjectItem(int projectItemId);
  //Future<int> createComment(int projectItemId);
  Future<bool> update(int commentId, Comment comment);
}

class CommentRepository extends CommentSource {
  final List<CommentSource> sources;

  CommentRepository(this.sources);

  Future<Comment> fetch(int commentId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(commentId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  // Future<List<Comment>> fetchOfProjectItem(int projectItemId) async {
  //   var values;
  //   for (var source in sources) {
  //     values = await source.fetchOfProjectItem(projectItemId);
  //     if (values != null) {
  //       break;
  //     }
  //   }
  //   return values;
  // }

  // Future<int> createComment(int projectItemId) =>
  //     sources[0].createComment(projectItemId);
  Future<bool> update(int commentId, Comment comment) =>
      sources[0].update(commentId, comment);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
