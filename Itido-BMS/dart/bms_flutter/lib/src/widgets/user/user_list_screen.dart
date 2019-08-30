import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/blocs.dart';

import 'user_list.dart';

class UserListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required UserListBloc Function(BuildContext) blocBuilder,
    Function(User) onSelect,
    Function(UserListBloc, List<User>) onManySelected,
    Widget floatingActionButton,
    bool showSearchableAppBar = true,
    bool searchableAppBarIsPrimary = true,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        body: UserListScreen(
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

  final UserListBloc Function(BuildContext) blocBuilder;
  final Function(User) onSelect;
  final Function(UserListBloc, List<User>) onManySelected;
  final Widget floatingActionButton;

  final bool searchableAppBarIsPrimary;
  final bool showSearchableAppBar;

  UserListScreen({
    Key key,
    @required this.blocBuilder,
    this.onManySelected,
    this.onSelect,
    this.floatingActionButton,
    this.showSearchableAppBar = true,
    this.searchableAppBarIsPrimary = false,
  }) : super(key: key);

  @override
  _UserListScreenState createState() => _UserListScreenState();
}

class _UserListScreenState extends State<UserListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<UserListBloc, UserListEvent, ListState<User>,
        User>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return UserList(
          onLongPress: widget.onManySelected != null
              ? (user) {
                  bloc.dispatch(UserLongPressed(user: user));
                }
              : null,
          onSelect: (user) {
            var _state = bloc.currentState;
            if (_state is Loaded<User> && _state.selectable) {
              bloc.dispatch(UserLongPressed(user: user));
            } else {
              if (widget.onSelect != null) widget.onSelect(user);
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
