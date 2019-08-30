import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/src/widgets/selectable_circular_avatar.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/blocs.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/style.dart' as style;

class UserList extends StatelessWidget {
  final Function(User) onSelect;
  final Function(User) onLongPress;
  final EdgeInsets padding;

  const UserList({Key key, this.onSelect, this.onLongPress, this.padding})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    final userListBloc = BlocProvider.of<UserListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: userListBloc,
      builder: (context, ListState<User> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<User>) {
          if (state.items.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoUsers);
          }
          return ListView.separated(
            padding: padding ?? EdgeInsets.only(top: 20, bottom: 200),
            itemBuilder: (BuildContext context, int index) {
              var user = state.items[index];
              return UserTile(
                user: user,
                onSelect: onSelect,
                onLongPress: onLongPress,
                selectable: state.selectable,
                selected:
                    state.selectable && state.selectedItems.contains(user),
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

// class UserTile extends StatelessWidget {
//   final User user;
//   final Function(User) onSelect;

//   const UserTile({
//     Key key,
//     @required this.user,
//     this.onSelect,
//   }) : super(key: key);

//   @override
//   Widget build(BuildContext context) {
//     return ListTile(
//       title: Text(user.displayName),
//       onTap: onSelect != null ? () => onSelect(user) : null,
//     );
//   }
// }

class UserTile extends StatelessWidget {
  final User user;
  final bool selectable;
  final bool selected;
  final Function(User) onSelect;
  final Function(User) onLongPress;

  const UserTile({
    Key key,
    @required this.user,
    this.onSelect,
    this.onLongPress,
    @required this.selectable,
    @required this.selected,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      // color: user.disabled
      //     ? style.declineRed
      //     : user.isClientUser ? Colors.green : null,
      child: ListTile(
        leading: SelectableCircularAvatar(
          name: user.displayName,
          selectable: selectable,
          selected: selected,
          backgroundColor: user.disabled
              ? style.declineRed
              : user.client != null ? Colors.blue[200] : null,
        ),
        title: Text(user.displayName),
        subtitle: Text(user.userRole +
            (user.client != null ? ' - ${user.client.name}' : '') +
            (user.customerName != null ? ' - ${user.customerName}' : '')),
        onTap: onSelect != null ? () => onSelect(user) : null,
        onLongPress: onLongPress != null ? () => onLongPress(user) : null,
        trailing: (user.hasAbsence ?? false)
            ? Icon(Icons.sentiment_dissatisfied, color: style.declineRed)
            : (user.hasAbsenceRequest ?? false)
                ? Icon(Icons.sentiment_neutral, color: style.pendingYellow)
                : user.hasProject != null && !user.hasProject
                    ? Icon(Icons.business, color: style.declineRed)
                    : null,
      ),
    );
  }
}
