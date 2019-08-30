import 'package:flutter/material.dart';

showPopupMenu(
  BuildContext context,
  String title,
  List<PopupItem> items,
) {
  showDialog(
      context: context,
      builder: (BuildContext context) {
        return new SimpleDialog(
          title: Center(child: Text(title)),
          children: []..addAll(
                items.where((i) => i != null).map<Widget>((PopupItem item) {
              return item.build(context);
            })),
        );
      });
}

class PopupItem {
  String title;
  Function onTap;
  TextStyle style;
  PopupItem({this.title, this.onTap, this.style});

  Widget build(BuildContext context) {
    return SimpleDialogOption(
      onPressed: () {
        Navigator.pop(context);
        onTap();
      },
      child: Text(
        title,
        style: style,
      ),
    );
  }
}
