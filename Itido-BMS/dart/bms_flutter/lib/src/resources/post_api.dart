import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class PostApi extends PostSource {
  //Client _client;

  ClientController<Post> _client;
  String path = '${api.path}/api/Post';

  PostApi({
    http.Client client,
  }) {
    _client = ClientController<Post>(
        converter: (json) => Post.fromJson(json), client: client);
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<Post>> fetchLatestPosts(int count) {
    return _client.getMany('$path/GetLatest/$count', headers: api.headers());
  }

  @override
  Future<int> createPost(Post post) {
    return _client.postId(
      '$path/Create',
      headers: api.headers(),
      body: post.toMap(),
    );
  }
}
