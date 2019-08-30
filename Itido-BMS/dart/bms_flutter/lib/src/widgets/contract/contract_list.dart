import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/contract_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class ContractList extends StatelessWidget {
  final Function(Contract) onSelect;

  const ContractList({Key key, this.onSelect}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final contractListBloc = BlocProvider.of<ContractListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: contractListBloc,
      builder: (context, ListState<Contract> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<Contract>) {
          if (state.items.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoContracts);
          }
          return ListView.builder(
            padding: EdgeInsets.only(top: 8, bottom: 80),
            itemBuilder: (BuildContext context, int index) {
              return ContractTile(
                contract: state.items[index],
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

class ContractTile extends StatelessWidget {
  final Contract contract;
  final Function(Contract) onSelect;

  const ContractTile({
    Key key,
    @required this.contract,
    this.onSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    var fromDate = Translations.of(context).dateString(contract.from);
    var toDate = Translations.of(context).timeString(contract.to);

    return ListTile(
      title: Text('${contract.agreement?.name ?? ''}, ${contract.weeklyHours}'),
      subtitle: Text('$fromDate - $toDate'),
      onTap: onSelect != null ? () => onSelect(contract) : null,
    );
  }
}
