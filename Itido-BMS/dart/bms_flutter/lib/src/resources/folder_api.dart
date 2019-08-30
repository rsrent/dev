import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class FolderApi extends FolderSource {
  //Client _client;

  ClientController<Folder> _client;
  String path = '${api.path}/api/Document';

  FolderApi({
    http.Client client,
  }) {
    _client = ClientController<Folder>(
        converter: (json) => Folder.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<bool> update(int folderId, Folder folder) {
    return _client.put(
      '$path/$folderId',
      body: folder.toMap(),
    );
  }

  @override
  Future<Folder> fetch(int folderId) {
    return _client.get('$path/$folderId');
  }

  @override
  Future<int> create(int folderId, String title) {
    return _client.postId(
      '$path/AddFolderToFolder/$folderId/$title',
    );
  }

  @override
  Future<List<Folder>> fetchOfProjectItem(int projectItemId) {
    return _client.getMany('$path/GetOfProjectItem/$projectItemId');
  }

  @override
  Future<List<Folder>> fetchOfFolder(int folderId) {
    return _client.getMany('$path/GetOfFolder/$folderId');
  }
}
