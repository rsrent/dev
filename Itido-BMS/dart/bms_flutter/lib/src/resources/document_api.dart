import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'package:firebase_storage/firebase_storage.dart';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';
import 'package:mime/mime.dart' as mime;

class DocumentApi extends DocumentSource {
  //Client _client;

  ClientController<Document> _client;
  String path = '${api.path}/api/Document';

  DocumentApi({
    http.Client client,
  }) {
    _client = ClientController<Document>(
        converter: (json) => Document.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  static String timestamp() => DateTime.now().millisecondsSinceEpoch.toString();
  static String get getFileName => timestamp();

  StorageReference getFileRef(int folderId) {
    return FirebaseStorage.instance
        .ref()
        .child('0/folder-$folderId/${timestamp()}');
  }

  @override
  Future<bool> update(int folderId, Document folder) {}

  @override
  Future<Document> fetch(int documentId) {}

  @override
  Future<int> create(int folderId, Document document) async {
    var ref = getFileRef(folderId);

    var contentType = mime.lookupMimeType(document.file.path);

    print('contentType: $contentType');

    final StorageUploadTask uploadTask = ref.putFile(
      document.file,
      StorageMetadata(
        contentType: contentType,
      ),
    );

    final StorageTaskSnapshot downloadUrl = (await uploadTask.onComplete);
    final String url = (await downloadUrl.ref.getDownloadURL());

    print('url: $url');

    document.file = null;
    document.url = url;

    //return 0;

    return _client.postId(
      '$path/AddDocumentToFolder/$folderId',
      body: document.toMap(),
    );
  }

  @override
  Future<List<Document>> fetchOfFolder(int folderId) {
    return _client.getMany(
      '$path/GetDocumentsOfFolder/$folderId',
    );
  }
}
