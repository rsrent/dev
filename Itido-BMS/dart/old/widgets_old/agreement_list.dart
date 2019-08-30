import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/agreement_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../../translations.dart';
import 'info_list_view.dart';

class AgreementList extends StatelessWidget {
  final Function(Agreement) onSelect;

  const AgreementList({Key key, this.onSelect}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final agreementListBloc = BlocProvider.of<AgreementListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: agreementListBloc,
      builder: (context, ListState<Agreement> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<Agreement>) {
          if (state.items.isEmpty) {
            return InfoListView(
                info: Translations.of(context).infoNoAgreements);
          }
          return ListView.builder(
            padding: EdgeInsets.only(top: 8, bottom: 80),
            itemBuilder: (BuildContext context, int index) {
              return AgreementTile(
                agreement: state.items[index],
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

class AgreementTile extends StatelessWidget {
  final Agreement agreement;
  final Function(Agreement) onSelect;

  const AgreementTile({
    Key key,
    @required this.agreement,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      title: Text(agreement.name),
      onTap: onSelect != null ? () => onSelect(agreement) : null,
    );
  }
}
