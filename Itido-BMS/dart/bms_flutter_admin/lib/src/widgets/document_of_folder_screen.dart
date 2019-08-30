import 'dart:io';

import 'package:bms_dart/folder_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/components.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:bms_flutter/src/widgets/folder/widgets.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:open_file/open_file.dart';
import 'package:bms_flutter/firestore_cloud_storage_contrller.dart'
    as cloudStorage;

void documentOfFolderScreen(BuildContext context, int folderId) {
  FolderListScreen.show(
    context,
    blocBuilder: (context) => FolderListBloc(
      () => FolderListFetchOfFolder(
        folderId: folderId,
      ),
    ),
    onFolderSelect: (folder) {
      documentOfFolderScreen(context, folder.id);
    },
    onDocumentSelect: (document) async {
      if (document.file == null) {
        print('cacheName: ' + 'document-${document.id}');
        document.file =
            await cloudStorage.FirestoreCloudStorageController.downloadFile(
          document.url,
          cacheName: 'document-${document.id}',
        );

        print('DOWNLOADED!!!');
        print('document.file.path: ${document.file.path}');
      }
      if (document.file != null) OpenFile.open(document.file.path);
    },
    floatingActionButton: Builder(
      builder: (context) {
        var bloc = BlocProvider.of<FolderListBloc>(context);
        return FloatingActionButton(
          child: Icon(Icons.add),
          onPressed: () => onFolderSelected(context, folderId, bloc),
          // onPressed: () async {

          //   String filePath;
          //   filePath = await FilePicker.getFilePath(type: FileType.ANY);

          //   if (filePath != null) {
          //     var documentName = await documentCreateDialog(context);

          //     if (documentName != null) {
          //       var doc = Document(file: File(filePath), title: documentName);
          //       bloc.dispatch(FolderListAddNewDocument(
          //         document: doc,
          //         folderId: folderId,
          //       ));
          //     }
          //   } else {
          //     print('File was null');
          //   }
          // },
        );
      },
    ),
  );
}

void onFolderSelected(BuildContext context, int folderId, FolderListBloc bloc) {
  showModalBottomSheet(
      context: context,
      builder: (context) {
        return Container(
          height: 400,
          child: ListView(
            children: <Widget>[
              ListTile(
                leading: Icon(Icons.add),
                title: Text('Add folder'),
                onTap: () async {
                  var text = await showTextFieldDialog(
                    context,
                    title: 'Navn på folder',
                    hint: 'Folders navn...',
                  );
                  if (text != null)
                    bloc.dispatch(FolderListAddNew(
                      title: text,
                      folderId: folderId,
                    ));
                },
              ),
              ListTile(
                leading: Icon(Icons.add),
                title: Text('Add document'),
                onTap: () async {
                  String filePath;
                  filePath = await FilePicker.getFilePath(type: FileType.ANY);

                  if (filePath != null) {
                    var documentName = await showTextFieldDialog(
                      context,
                      title: 'Navn på dokumentet',
                      hint: 'Dokumentets navn...',
                    );

                    if (documentName != null) {
                      var doc =
                          Document(file: File(filePath), title: documentName);
                      bloc.dispatch(FolderListAddNewDocument(
                        document: doc,
                        folderId: folderId,
                      ));
                    }
                  } else {
                    print('File was null');
                  }
                },
              ),
            ],
          ),
        );
      });
}
