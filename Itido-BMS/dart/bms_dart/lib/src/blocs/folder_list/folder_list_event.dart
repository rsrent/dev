import 'package:bms_dart/models.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/folder.dart';

@immutable
abstract class FolderListEvent extends Equatable {
  FolderListEvent([List props = const []]) : super(props);
}

class FolderListFetchOfFolder extends FolderListEvent {
  final int folderId;
  FolderListFetchOfFolder({@required this.folderId}) : super([folderId]);
  @override
  String toString() => 'FolderListFetchOfFolder';
}

class FolderListFetched extends FolderListEvent {
  final List<Folder> folders;
  final List<Document> documents;

  FolderListFetched({@required this.folders, @required this.documents})
      : super([folders, documents]);
  @override
  String toString() =>
      'FolderListFetched { folders: ${folders.length}, documents: ${documents.length} }';
}

class FolderListAddNew extends FolderListEvent {
  final int folderId;
  final String title;
  FolderListAddNew({@required this.folderId, @required this.title})
      : super([folderId, title]);
  @override
  String toString() => 'FolderListAddNew';
}

class FolderListAddNewDocument extends FolderListEvent {
  final int folderId;
  final Document document;
  FolderListAddNewDocument({@required this.folderId, @required this.document})
      : super([folderId, document.toMap()]);
  @override
  String toString() => 'FolderListAddNewDocument';
}
