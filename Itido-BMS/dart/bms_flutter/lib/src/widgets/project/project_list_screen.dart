import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/project_list_bloc.dart';

import 'project_list.dart';

class ProjectListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required ProjectListBloc Function(BuildContext) blocBuilder,
    Function(Project) onSelect,
    Function(ProjectListBloc, List<Project>) onManySelected,
    Widget floatingActionButton,
    bool showSearchableAppBar = true,
    bool searchableAppBarIsPrimary = true,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        body: ProjectListScreen(
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

  final ProjectListBloc Function(BuildContext) blocBuilder;
  final Function(Project) onSelect;
  final Function(ProjectListBloc, List<Project>) onManySelected;
  final Widget floatingActionButton;

  final bool searchableAppBarIsPrimary;
  final bool showSearchableAppBar;

  ProjectListScreen({
    Key key,
    @required this.blocBuilder,
    this.onManySelected,
    this.onSelect,
    this.floatingActionButton,
    this.showSearchableAppBar = true,
    this.searchableAppBarIsPrimary = false,
  }) : super(key: key);

  @override
  _ProjectListScreenState createState() => _ProjectListScreenState();
}

class _ProjectListScreenState extends State<ProjectListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<ProjectListBloc, ProjectListEvent,
        ListState<Project>, Project>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return ProjectList(
          onLongPress: widget.onManySelected != null
              ? (project) {
                  bloc.dispatch(ProjectLongPressed(project: project));
                }
              : null,
          onSelect: (project) {
            var _state = bloc.currentState;
            if (_state is Loaded<Project> && _state.selectable) {
              bloc.dispatch(ProjectLongPressed(project: project));
            } else {
              if (widget.onSelect != null) widget.onSelect(project);
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
