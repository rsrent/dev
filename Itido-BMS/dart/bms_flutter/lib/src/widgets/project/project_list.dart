import 'package:bms_dart/project_list_bloc.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/src/widgets/selectable_circular_avatar.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/blocs.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/style.dart' as style;

class ProjectList extends StatelessWidget {
  final Function(Project) onSelect;
  final Function(Project) onLongPress;
  final EdgeInsets padding;

  const ProjectList({Key key, this.onSelect, this.onLongPress, this.padding})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    final projectListBloc = BlocProvider.of<ProjectListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: projectListBloc,
      builder: (context, ListState<Project> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<Project>) {
          if (state.items.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoProjects);
          }
          return ListView.separated(
            padding: padding ?? EdgeInsets.only(top: 20, bottom: 200),
            itemBuilder: (BuildContext context, int index) {
              var project = state.items[index];
              return ProjectTile(
                project: project,
                onSelect: onSelect,
                onLongPress: onLongPress,
                selectable: state.selectable,
                selected:
                    state.selectable && state.selectedItems.contains(project),
              );
            },
            itemCount: state.items.length,
            separatorBuilder: (BuildContext context, int index) => Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16),
              child: Divider(height: 2),
            ),
          );
        }
      },
    );
  }
}

// class ProjectTile extends StatelessWidget {
//   final Project project;
//   final Function(Project) onSelect;

//   const ProjectTile({
//     Key key,
//     @required this.project,
//     this.onSelect,
//   }) : super(key: key);

//   @override
//   Widget build(BuildContext context) {
//     return ListTile(
//       title: Text(project.displayName),
//       onTap: onSelect != null ? () => onSelect(project) : null,
//     );
//   }
// }

class ProjectTile extends StatelessWidget {
  final Project project;
  final bool selectable;
  final bool selected;
  final Function(Project) onSelect;
  final Function(Project) onLongPress;

  const ProjectTile({
    Key key,
    @required this.project,
    this.onSelect,
    this.onLongPress,
    @required this.selectable,
    @required this.selected,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      color: project.disabled ? style.declineRed : null,
      child: ListTile(
        leading: SelectableCircularAvatar(
          name: project.name,
          selectable: selectable,
          selected: selected,
          backgroundColor: project.disabled
              ? style.declineRed
              : project.isClient ? Colors.blue[200] : null,
        ),
        title: Text(project.name),
        onTap: onSelect != null ? () => onSelect(project) : null,
        onLongPress: onLongPress != null ? () => onLongPress(project) : null,
      ),
    );
  }
}
