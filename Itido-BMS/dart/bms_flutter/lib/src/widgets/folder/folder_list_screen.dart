import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/folder_list_bloc.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'folder_list.dart';

class FolderListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required FolderListBloc Function(BuildContext) blocBuilder,
    Function(Folder) onFolderSelect,
    Function(Document) onDocumentSelect,
    Widget floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: FolderListScreen(
          onFolderSelect: onFolderSelect,
          onDocumentSelect: onDocumentSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final FolderListBloc Function(BuildContext) blocBuilder;
  final Function(Folder) onFolderSelect;
  final Function(Document) onDocumentSelect;
  final Widget floatingActionButton;

  FolderListScreen({
    Key key,
    @required this.blocBuilder,
    this.onFolderSelect,
    this.onDocumentSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _FolderListScreenState createState() => _FolderListScreenState();
}

class _FolderListScreenState extends State<FolderListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<FolderListBloc, FolderListEvent,
        ListState<dynamic>, dynamic>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return FolderList(
          onFolderSelect: widget.onFolderSelect,
          onDocumentSelect: widget.onDocumentSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
