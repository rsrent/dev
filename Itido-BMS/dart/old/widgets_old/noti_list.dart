import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/noti_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../../translations.dart';
import 'info_list_view.dart';

class NotiList extends StatelessWidget {
  final Function(Noti) onSelect;
  final Function(Noti) onDelete;

  const NotiList({Key key, this.onSelect, this.onDelete}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final notiListBloc = BlocProvider.of<NotiListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: notiListBloc,
      builder: (context, ListState<Noti> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          var notis = (state as Loaded).items;
          if (notis.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoNotis);
          }
          return ListView.separated(
            padding: EdgeInsets.all(0),
            itemBuilder: (BuildContext context, int index) {
              return NotiTile(
                noti: notis[index],
                onSelect: onSelect,
              );
            },
            itemCount: notis.length,
            separatorBuilder: (context, index) => Divider(),
          );
        }
      },
    );
  }
}

class NotiTile extends StatelessWidget {
  final Noti noti;
  final Function(Noti) onSelect;

  const NotiTile({
    Key key,
    @required this.noti,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    print(noti.toMap());

    var dateTimeString = Translations.of(context).dateTimeString(noti.sendTime);

    return ListTile(
      title: Text(noti.title),
      subtitle: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: <Widget>[
          Text(
            noti.body,
            style: TextStyle(
                fontWeight: noti.seen ? FontWeight.normal : FontWeight.bold),
          ),
          Text(dateTimeString)
        ],
      ),
      onTap: onSelect != null ? () => onSelect(noti) : null,
    );
  }
}
