import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/contract_list_bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
// import 'package:bms_flutter_admin/src/screens/user_list_screen.dart';
import 'package:dart_packages/tuple.dart';
import 'package:flutter/material.dart';
import 'package:bms_flutter/style.dart' as style;

import 'package:bms_flutter/src/widgets/user/widgets.dart';
import 'package:bms_flutter/src/widgets/contract/widgets.dart';

Future<Tuple2<User, Contract>> selectUserContract(
    BuildContext context, UserListEvent fetchEvent) async {
  User _user;
  // var contract = await UserListScreen.pushUserListScreen(
  //   context,
  //   Translations.of(context).hintSearchUsers,
  //   fetchEvent,
  //   (user) async {
  //     if (user is User) {
  //       _user = user;
  //       var contract = await showContracts(context, user);
  //       Navigator.of(context).pop(contract);
  //     }
  //   },
  // );

  // var bloc = UserListBloc(() => fetchEvent);
  // var controller = TextEditingController();

  var contract = await UserListScreen.show(
    context,
    blocBuilder: (context) => UserListBloc(() => fetchEvent),
    onSelect: (user) async {
      if (user is User) {
        _user = user;
        var contract = await showContracts(context, user);
        if (contract != null) Navigator.of(context).pop(contract);
      }
    },
    searchableAppBarIsPrimary: true,
  );

  // var contract = await Navigator.of(context).push(
  //   MaterialPageRoute(
  //     builder: (context) => Scaffold(
  //       appBar: SearchableAppBar(
  //         controller: controller,
  //         onChanged: (text) =>
  //             bloc.dispatch(SearchTextUpdated(searchText: text)),
  //         hintText: Translations.of(context).hintSearchUsers,
  //       ),
  //       body: UserListAdmin(
  //         blocBuilder: (context) => bloc,
  //         onSelect: (user) async {
  //           if (user is User) {
  //             _user = user;
  //             var contract = await showContracts(context, user);
  //             Navigator.of(context).pop(contract);
  //           }
  //         },
  //       ),
  //     ),
  //   ),
  // );

  if (contract is Contract) {
    return Tuple2(_user, contract);
  }
  return null;
}

Future<Contract> showContracts(BuildContext context, User user) async {
  var result = await showModalBottomSheet<Contract>(
    context: context,
    builder: (context) {
      return Column(
        children: <Widget>[
          ListTile(
            title: Text(Translations.of(context).hintSelectContract),
          ),
          Expanded(
            child: ContractListScreen(
              blocBuilder: (context) => ContractListBloc(
                  () => ContractListFetchOfUser(userId: user.id)),
              onSelect: (contract) => Navigator.of(context).pop(contract),
            ),
          ),
          ListTile(
            leading: Icon(Icons.arrow_back),
            title: Text(
              Translations.of(context).buttonBack,
              style: TextStyle(color: style.declineRed),
            ),
            onTap: () {
              Navigator.of(context).pop();
            },
          ),
        ],
      );
    },
  );
  if (result is Contract) return result;
  return null;
}
