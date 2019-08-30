import 'package:bms_dart/contract_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:bms_flutter_admin/src/screens/contract_create_screen.dart';
import 'package:bms_flutter_admin/src/screens/contract_update_screen.dart';
import 'package:flutter/material.dart';

class ContractListAdmin {
  static Widget getContractListOfUser(BuildContext context, User user) {
    return BlocListHalfScreen<ContractListBloc, ContractListEvent,
        ListState<Contract>, Contract>(
      child: ContractList(
        onFolderSelect: (contract) {
          Navigator.of(context).push(MaterialPageRoute(
            builder: (context) => ContractUpdateScreen(
              user: user,
              contractToUpdate: contract,
            ),
          ));
        },
      ),
      floatingActionButton: FloatingActionButton(
        heroTag: null,
        child: Icon(Icons.add),
        onPressed: () {
          Navigator.of(context).push(MaterialPageRoute(builder: (context) {
            return ContractCreateScreen(user: user);
          }));
        },
      ),
    );
  }
}
