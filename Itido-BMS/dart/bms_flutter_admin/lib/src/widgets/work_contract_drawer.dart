import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/work_contract_list_bloc.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_flutter/style.dart' as style;
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/widgets.dart';
import 'select_user_and_contract.dart';
import 'package:bms_flutter/src/widgets/work_contract/widgets.dart';

class WorkContractDrawer extends StatefulWidget {
  static void show(BuildContext context, WorkContract workContract,
      WorkContractListBloc bloc) {
    //var bloc = BlocProvider.of<WorkContractListBloc>(context);
    showModalBottomSheet(
      context: context,
      builder: (context) {
        return Container(
          height: 400,
          child: WorkContractDrawer(
            bloc: bloc,
            workContract: workContract,
          ),
        );
      },
    );
  }

  final WorkContract workContract;
  final WorkContractListBloc bloc;

  const WorkContractDrawer({Key key, this.workContract, this.bloc})
      : super(key: key);
  @override
  _WorkContractDrawerState createState() => _WorkContractDrawerState();
}

class _WorkContractDrawerState extends State<WorkContractDrawer> {
  @override
  Widget build(BuildContext context) {
    var workContract = widget.workContract;
    var bloc = widget.bloc;

    return BlocListener(
        bloc: bloc,
        listener: (BuildContext context, state) {
          setState(() {});
        },
        child: ListView(
          children: <Widget>[
            IgnorePointer(child: WorkContractTile(workContract: workContract)),
            ListTile(
              title: Text(Translations.of(context).labelComment),
              subtitle: Text(workContract.note ?? ''),
            ),
            ListTile(
              leading: Icon(Icons.edit),
              title: Text(Translations.of(context).titleUpdateWork),
              onTap: () {
                return Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) {
                      return WorkContractCreateUpdateScreen(
                          workContractToUpdate: workContract);
                    },
                  ),
                );
              },
            ),
            if (workContract.isOwned)
              ListTile(
                leading: Icon(Icons.remove_circle),
                title: Text(Translations.of(context).optionRemoveWorkUser),
                onTap: () => bloc.dispatch(WorkContractListRemoveUser(
                    workContractId: workContract.id)),
              ),
            if (!workContract.isOwned)
              ListTile(
                leading: Icon(Icons.person),
                title: Text(Translations.of(context).optionFindWorkOwner),
                onTap: () async {
                  //print('workContract.locationId: ${workContract.location.id}');
                  var userContract = await selectUserContract(context,
                      FetchOfProject(projectId: workContract.project.id));

                  if (userContract != null) {
                    bloc.dispatch(WorkContractListAddUserContract(
                      workContract: workContract,
                      user: userContract.first,
                      contract: userContract.second,
                    ));
                  }
                },
              ),
            ListTile(
              leading: Icon(Icons.keyboard_arrow_left),
              title: Text(Translations.of(context).buttonBack),
              onTap: () => Navigator.of(context).pop(),
            ),
          ],
        ));
  }
}
