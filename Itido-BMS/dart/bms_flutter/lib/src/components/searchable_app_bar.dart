import 'package:flutter/material.dart';

class SearchableAppBar extends StatelessWidget implements PreferredSizeWidget {
  final TextEditingController controller;
  final List<Widget> actions;
  final String hintText;
  final Function(String) onChanged;
  final Function(String) onSubmitted;
  final bool primary;

  final List<String> filters;

  SearchableAppBar({
    Key key,
    @required this.controller,
    this.actions = const [],
    @required this.hintText,
    this.onSubmitted,
    @required this.onChanged,
    this.primary,
    this.filters,
  })  : preferredSize = Size.fromHeight(kToolbarHeight),
        super(key: key);

  @override
  Widget build(BuildContext context) {
    ThemeData theme = Theme.of(context);
    theme = theme.copyWith(
      primaryColor: Colors.white,
      primaryIconTheme: theme.primaryIconTheme.copyWith(color: Colors.grey),
      primaryColorBrightness: Brightness.light,
      primaryTextTheme: theme.textTheme,
    );

    return AppBar(
      automaticallyImplyLeading: (primary ?? true),
      primary: (primary ?? true),
      backgroundColor: theme.primaryColor,
      iconTheme: theme.primaryIconTheme,
      textTheme: theme.primaryTextTheme,
      brightness: theme.primaryColorBrightness,
      title: TextField(
        controller: controller,
        style: theme.textTheme.title,
        textInputAction: TextInputAction.search,
        onSubmitted: onSubmitted,
        onChanged: onChanged,
        decoration: InputDecoration(
          border: InputBorder.none,
          hintText: hintText,
          hintStyle: theme.inputDecorationTheme.hintStyle,
        ),
      ),
      // title: Row(
      //   children: <Widget>[
      //     Expanded(
      //       child: TextField(
      //         controller: controller,
      //         style: theme.textTheme.title,
      //         textInputAction: TextInputAction.search,
      //         onSubmitted: onSubmitted,
      //         onChanged: onChanged,
      //         decoration: InputDecoration(
      //           border: InputBorder.none,
      //           hintText: hintText,
      //           hintStyle: theme.inputDecorationTheme.hintStyle,
      //         ),
      //       ),
      //     ),
      //     ConstrainedBox(
      //       constraints: BoxConstraints(maxWidth: 160),
      //       child: Wrap(
      //         children: <Widget>[
      //           Chip(label: Text('aktiv')),
      //           Chip(label: Text('aktiv')),
      //           Chip(label: Text('aktiv')),
      //           Chip(label: Text('aktiv')),
      //         ],
      //       ),
      //     ),
      //   ],
      // ),
      actions: []
        ..addAll(actions ?? [])
        ..add(IconButton(
          icon: Icon(Icons.clear),
          onPressed: () {
            controller.clear();
            onChanged('');
          },
        )),
    );
  }

  @override
  final Size preferredSize;
}
