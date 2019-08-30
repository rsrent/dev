import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/absence_reason_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AbsenceReasonList extends StatelessWidget {
  final Function(AbsenceReason) onSelect;

  const AbsenceReasonList({Key key, this.onSelect}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final absenceReasonListBloc =
        BlocProvider.of<AbsenceReasonListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: absenceReasonListBloc,
      builder: (context, ListState<AbsenceReason> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<AbsenceReason>) {
          if (state.items.isEmpty) {
            return InfoListView(
                info: Translations.of(context).infoNoAbsenceReasons);
          }
          return ListView.builder(
            padding: EdgeInsets.only(top: 8, bottom: 80),
            itemBuilder: (BuildContext context, int index) {
              return AbsenceReasonTile(
                absenceReason: state.items[index],
                onSelect: onSelect,
              );
            },
            itemCount: state.items.length,
          );
        }
      },
    );
  }
}

class AbsenceReasonTile extends StatelessWidget {
  final AbsenceReason absenceReason;
  final Function(AbsenceReason) onSelect;

  const AbsenceReasonTile({
    Key key,
    @required this.absenceReason,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      title: Text('${absenceReason.description}'),
      onTap: onSelect != null ? () => onSelect(absenceReason) : null,
    );
  }
}
