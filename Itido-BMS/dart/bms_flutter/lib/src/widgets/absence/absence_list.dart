import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/src/widgets/request_widget.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/absence_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AbsenceList extends StatelessWidget {
  final Function(Absence) onSelect;

  const AbsenceList({Key key, this.onSelect}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final absenceListBloc = BlocProvider.of<AbsenceListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: absenceListBloc,
      builder: (context, ListState<Absence> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<Absence>) {
          if (state.items.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoAbsences);
          }
          return ListView.builder(
            padding: EdgeInsets.only(top: 8, bottom: 80),
            itemBuilder: (BuildContext context, int index) {
              return AbsenceTile(
                absence: state.items[index],
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

class AbsenceTile extends StatelessWidget {
  final Absence absence;
  final Function(Absence) onSelect;

  const AbsenceTile({
    Key key,
    @required this.absence,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final absenceListBloc = BlocProvider.of<AbsenceListBloc>(context);

    var fromDate = Translations.of(context).dateString(absence.from);
    var toDate = Translations.of(context).dateString(absence.to);

    return Card(
      child: Row(
        children: <Widget>[
          Expanded(
            child: ListTile(
              title: Text((absence.absenceReason?.description ?? '') +
                  ' ${absence.isRequest ? ' (' + Translations.of(context).infoIsRequest + ')' : ''}'),
              subtitle: Text('$fromDate - $toDate'),
              onTap: onSelect != null ? () => onSelect(absence) : null,
            ),
          ),
          RequestWidget.vertical(
            request: absence.request,
            onRespond: (isApproved) {
              absenceListBloc.dispatch(AbsenceListRespond(
                id: absence.id,
                isApproved: isApproved,
              ));
            },
          ),
          // SizedBox(
          //     width: 100,
          //     child: absence.approvalState == ApprovalState.Pending
          //         ? absence.canRespondToApprovalState
          //             ? Row(
          //                 children: <Widget>[
          //                   Container(
          //                     width: 1,
          //                     height: 100,
          //                     color: Colors.grey[400],
          //                   ),
          //                   Expanded(
          //                     child: Column(
          //                       mainAxisAlignment:
          //                           MainAxisAlignment.spaceBetween,
          //                       children: <Widget>[
          //                         FlatButton(
          //                           child: Text(
          //                             Translations.of(context).buttonApprove,
          //                             style: TextStyle(color: acceptGreen),
          //                           ),
          //                           onPressed: () => absenceListBloc
          //                               .dispatch(AbsenceListRespond(
          //                             id: absence.id,
          //                             isApproved: true,
          //                           )),
          //                         ),
          //                         Container(
          //                           color: Colors.grey[400],
          //                           height: 1,
          //                         ),
          //                         FlatButton(
          //                           child: Text(
          //                             Translations.of(context).buttonDeline,
          //                             style: TextStyle(color: declineRed),
          //                           ),
          //                           onPressed: () => absenceListBloc
          //                               .dispatch(AbsenceListRespond(
          //                             id: absence.id,
          //                             isApproved: false,
          //                           )),
          //                         ),
          //                       ],
          //                     ),
          //                   ),
          //                 ],
          //               )
          //             : Center(
          //                 child: Text(Translations.of(context).infoPending))
          //         : absence.approvalState == ApprovalState.Approved
          //             ? Center(
          //                 child: Text(
          //                 Translations.of(context).infoApproved,
          //                 style: TextStyle(color: acceptGreen),
          //               ))
          //             : Center(
          //                 child: Text(
          //                 Translations.of(context).infoDeclined,
          //                 style: TextStyle(color: declineRed),
          //               ))),
        ],
      ),
    );
  }
}
