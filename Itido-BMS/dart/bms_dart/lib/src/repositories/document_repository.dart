import 'dart:async';
import '../models/document.dart';
import 'source.dart';

abstract class DocumentSource extends Source {
  Future<Document> fetch(int documentId);
  Future<List<Document>> fetchOfFolder(int folderId);
  Future<int> create(int folderId, Document document);
  Future<bool> update(int folderId, Document document);
}

class DocumentRepository extends DocumentSource {
  final List<DocumentSource> sources;

  DocumentRepository(this.sources);

  Future<Document> fetch(int documentId) async {
    var values;
    for (var source in sources) {
      values = await source.fetch(documentId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<List<Document>> fetchOfFolder(int folderId) async {
    var values;
    for (var source in sources) {
      values = await source.fetchOfFolder(folderId);
      if (values != null) {
        break;
      }
    }
    return values;
  }

  Future<int> create(int folderId, Document document) =>
      sources[0].create(folderId, document);
  Future<bool> update(int folderId, Document document) =>
      sources[0].update(folderId, document);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
