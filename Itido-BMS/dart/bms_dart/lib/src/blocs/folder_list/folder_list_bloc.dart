import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/folder.dart';
import 'package:bms_dart/repositories.dart';

class FolderListBloc extends Bloc<FolderListEvent, ListState<dynamic>>
    with Refreshable {
  final FolderRepository _folderRepository =
      repositoryProvider.folderRepository();
  final DocumentRepository _documentRepository =
      repositoryProvider.documentRepository();

  final FolderListEvent Function() _refreshEven;

  FolderListBloc(this._refreshEven) {
    refresh();
  }

  @override
  ListState<dynamic> get initialState => Loading<dynamic>();

  @override
  Stream<ListState<dynamic>> mapEventToState(
    FolderListEvent event,
  ) async* {
    if (event is FolderListFetchOfFolder) {
      var folders = _folderRepository.fetchOfFolder(event.folderId);
      var documents = _documentRepository.fetchOfFolder(event.folderId);

      var foldersAndDocs = await Future.wait([folders, documents]);

      dispatch(FolderListFetched(
          folders: foldersAndDocs[0], documents: foldersAndDocs[1]));
    }
    if (event is FolderListFetched) {
      List<dynamic> items = List();
      items.addAll(event.folders);
      items.addAll(event.documents);
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is FolderListAddNew) {
      yield Loading();
      await _folderRepository
          .create(event.folderId, event.title)
          .then((folderId) => refresh());
    }
    if (event is FolderListAddNewDocument) {
      yield Loading();
      await _documentRepository
          .create(event.folderId, event.document)
          .then((folderId) => refresh());
    }
  }

  @override
  void refresh() {
    dispatch(_refreshEven());
  }
}
