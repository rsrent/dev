import 'package:bms_flutter/src/widgets/bloc_list_half_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/contract_list_bloc.dart';
import 'contract_list.dart';

class ContractListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required ContractListBloc Function(BuildContext) blocBuilder,
    Function(Contract) onSelect,
    FloatingActionButton floatingActionButton,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        body: ContractListScreen(
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final ContractListBloc Function(BuildContext) blocBuilder;
  final Function(Contract) onSelect;
  final FloatingActionButton floatingActionButton;

  ContractListScreen({
    Key key,
    @required this.blocBuilder,
    this.onSelect,
    this.floatingActionButton,
  }) : super(key: key);

  @override
  _ContractListScreenState createState() => _ContractListScreenState();
}

class _ContractListScreenState extends State<ContractListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<ContractListBloc, ContractListEvent,
        ListState<Contract>, Contract>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return ContractList(
          onSelect: widget.onSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
