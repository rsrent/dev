import 'dart:async';
import '../models/post.dart';
import 'source.dart';

abstract class PostSource extends Source {
  Future<List<Post>> fetchLatestPosts(int count);
  Future<int> createPost(Post post);
}

class PostRepository extends PostSource {
  final List<PostSource> sources;

  PostRepository(this.sources);

  Future<List<Post>> fetchLatestPosts(int count) async {
    var values;
    for (var source in sources) {
      values = await source.fetchLatestPosts(count);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<int> createPost(Post post) => sources[0].createPost(post);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
