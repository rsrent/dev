import 'package:bms_dart/noti_list_bloc.dart';
import 'package:bms_flutter/src/widgets/bloc_list_half_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';

import 'noti_list.dart';

class NotiListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required NotiListBloc Function(BuildContext) blocBuilder,
    Function(Noti) onSelect,
    FloatingActionButton floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: NotiListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final NotiListBloc Function(BuildContext) blocBuilder;
  final Function(Noti) onSelect;
  final FloatingActionButton floatingActionButton;

  NotiListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _NotiListScreenState createState() => _NotiListScreenState();
}

class _NotiListScreenState extends State<NotiListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<NotiListBloc, NotiListEvent, ListState<Noti>,
        Noti>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return NotiList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
