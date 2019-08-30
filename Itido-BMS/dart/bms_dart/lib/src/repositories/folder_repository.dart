import 'dart:async';
import '../models/folder.dart';
import 'source.dart';

abstract class FolderSource extends Source {
  Future<Folder> fetch(int folderId);
  Future<List<Folder>> fetchOfProjectItem(int projectItemId);
  Future<List<Folder>> fetchOfFolder(int folderId);
  Future<int> create(int folderId, String title);
  Future<bool> update(int folderId, Folder folder);
}

class FolderRepository extends FolderSource {
  final List<FolderSource> sources;

  FolderRepository(this.sources);

  Future<Folder> fetch(int folderId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(folderId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<Folder>> fetchOfFolder(int folderId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfFolder(folderId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<Folder>> fetchOfProjectItem(int projectItemId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfProjectItem(projectItemId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<int> create(int folderId, String title) =>
      sources[0].create(folderId, title);
  Future<bool> update(int folderId, Folder folder) =>
      sources[0].update(folderId, folder);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
