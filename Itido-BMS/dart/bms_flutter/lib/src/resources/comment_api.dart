import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class CommentApi extends CommentSource {
  ClientController<Comment> _client;
  String path = '${api.path}/api/Comment';

  CommentApi({
    http.Client client,
  }) {
    _client = ClientController<Comment>(
        converter: (json) => Comment.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<bool> update(int commentId, Comment comment) {
    return _client.put(
      '$path/Update/$commentId',
      body: comment.toMap(),
    );
  }

  @override
  Future<Comment> fetch(int commentId) {
    return _client.get('$path/Get/$commentId');
  }
}
