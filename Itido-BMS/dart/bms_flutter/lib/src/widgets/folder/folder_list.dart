import 'package:bms_dart/list_bloc.dart';
import 'package:bms_dart/folder_list_bloc.dart';
import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class FolderList extends StatelessWidget {
  final Function(Folder) onFolderSelect;
  final Function(Folder) onFolderDelete;
  final Function(Document) onDocumentSelect;
  final Function(Document) onDocumentDelete;

  const FolderList({
    Key key,
    this.onFolderSelect,
    this.onFolderDelete,
    this.onDocumentSelect,
    this.onDocumentDelete,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final folderListBloc = BlocProvider.of<FolderListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: folderListBloc,
      builder: (context, ListState<dynamic> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          var folders = (state as Loaded).items;
          if (folders.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoFolders);
          }
          return ListView.builder(
            padding: EdgeInsets.all(0),
            itemBuilder: (BuildContext context, int index) {
              var item = folders[index];

              if (item is Folder)
                return FolderTile(
                  folder: folders[index],
                  onSelect: onFolderSelect,
                );
              if (item is Document)
                return DocumentTile(
                  document: folders[index],
                  onSelect: onDocumentSelect,
                );
            },
            itemCount: folders.length,
          );
        }
      },
    );
  }
}

class FolderTile extends StatelessWidget {
  final Folder folder;
  final Function(Folder) onSelect;

  const FolderTile({
    Key key,
    @required this.folder,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      leading: Icon(Icons.folder),
      title: Text(
        folder.title ?? '',
      ),
      onTap: onSelect != null ? () => onSelect(folder) : null,
    );
  }
}

class DocumentTile extends StatelessWidget {
  final Document document;
  final Function(Document) onSelect;

  const DocumentTile({
    Key key,
    @required this.document,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      leading: Icon(Icons.insert_drive_file),
      title: Text(
        document.title ?? '',
      ),
      onTap: onSelect != null ? () => onSelect(document) : null,
    );
  }
}
