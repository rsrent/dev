import 'package:bms_dart/agreement_list_bloc.dart';
import 'package:bms_flutter/src/widgets/bloc_list_half_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'agreement_list.dart';

class AgreementListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required AgreementListBloc Function(BuildContext) blocBuilder,
    Function(Agreement) onSelect,
    FloatingActionButton floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(),
        body: AgreementListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final AgreementListBloc Function(BuildContext) blocBuilder;
  final Function(Agreement) onSelect;
  final FloatingActionButton floatingActionButton;

  AgreementListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _AgreementListScreenState createState() => _AgreementListScreenState();
}

class _AgreementListScreenState extends State<AgreementListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<AgreementListBloc, AgreementListEvent,
        ListState<Agreement>, Agreement>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return AgreementList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
