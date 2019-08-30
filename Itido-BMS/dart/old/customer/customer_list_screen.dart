import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/customer_list_bloc.dart';

import 'customer_list.dart';

class CustomerListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required CustomerListBloc Function(BuildContext) blocBuilder,
    Function(Customer) onSelect,
    Function(CustomerListBloc, List<Customer>) onManySelected,
    FloatingActionButton floatingActionButton,
    bool showSearchableAppBar = true,
    bool searchableAppBarIsPrimary = true,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        body: CustomerListScreen(
          onManySelected: onManySelected,
          onSelect: onSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
          showSearchableAppBar: showSearchableAppBar,
          searchableAppBarIsPrimary: searchableAppBarIsPrimary,
        ),
      ),
    ));
  }

  final CustomerListBloc Function(BuildContext) blocBuilder;
  final Function(Customer) onSelect;
  final Function(CustomerListBloc, List<Customer>) onManySelected;
  final FloatingActionButton floatingActionButton;

  final bool searchableAppBarIsPrimary;
  final bool showSearchableAppBar;

  CustomerListScreen({
    Key key,
    @required this.blocBuilder,
    this.onManySelected,
    this.onSelect,
    this.floatingActionButton,
    this.showSearchableAppBar = true,
    this.searchableAppBarIsPrimary = false,
  }) : super(key: key);

  @override
  _CustomerListScreenState createState() => _CustomerListScreenState();
}

class _CustomerListScreenState extends State<CustomerListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<CustomerListBloc, CustomerListEvent,
        ListState<Customer>, Customer>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return CustomerList(
          onLongPress: widget.onManySelected != null
              ? (customer) {
                  bloc.dispatch(CustomerLongPressed(customer: customer));
                }
              : null,
          onSelect: (customer) {
            var _state = bloc.currentState;
            if (_state is Loaded<Customer> && _state.selectable) {
              bloc.dispatch(CustomerLongPressed(customer: customer));
            } else {
              if (widget.onSelect != null) widget.onSelect(customer);
            }
          },
        );
      },
      floatingActionButton: widget.floatingActionButton,
      onSelectMany: widget.onManySelected,
      searchableAppBarIsPrimary: widget.searchableAppBarIsPrimary,
      showSearchableAppBar: widget.showSearchableAppBar,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
