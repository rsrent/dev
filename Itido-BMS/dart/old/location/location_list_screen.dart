import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/location_list_bloc.dart';

import 'location_list.dart';

class LocationListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required LocationListBloc Function(BuildContext) blocBuilder,
    Function(Location) onSelect,
    Function(LocationListBloc, List<Location>) onManySelected,
    Widget floatingActionButton,
    bool showSearchableAppBar = true,
    bool searchableAppBarIsPrimary = true,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        body: LocationListScreen(
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

  final LocationListBloc Function(BuildContext) blocBuilder;
  final Function(Location) onSelect;
  final Function(LocationListBloc, List<Location>) onManySelected;
  final Widget floatingActionButton;

  final bool searchableAppBarIsPrimary;
  final bool showSearchableAppBar;

  LocationListScreen({
    Key key,
    @required this.blocBuilder,
    this.onManySelected,
    this.onSelect,
    this.floatingActionButton,
    this.showSearchableAppBar = true,
    this.searchableAppBarIsPrimary = false,
  }) : super(key: key);

  @override
  _LocationListScreenState createState() => _LocationListScreenState();
}

class _LocationListScreenState extends State<LocationListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<LocationListBloc, LocationListEvent,
        ListState<Location>, Location>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return LocationList(
          onLongPress: widget.onManySelected != null
              ? (location) {
                  bloc.dispatch(LocationLongPressed(location: location));
                }
              : null,
          onSelect: (location) {
            var _state = bloc.currentState;
            if (_state is Loaded<Location> && _state.selectable) {
              bloc.dispatch(LocationLongPressed(location: location));
            } else {
              if (widget.onSelect != null) widget.onSelect(location);
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
