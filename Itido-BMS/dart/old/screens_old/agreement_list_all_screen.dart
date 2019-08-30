import '../screens/agreement_create_screen.dart';
import 'package:bms_dart/agreement_list_bloc.dart';
import 'package:flutter/material.dart';
import 'package:bms_flutter/widgets.dart';

import 'agreement_update_screen.dart';

class AgreementListAllScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return BlocListScreen(
      blocBuilder: (context) =>
          AgreementListBloc()..dispatch(AgreementListFetch()),
      onRefresh: (bloc) => bloc.dispatch(AgreementListFetch()),
      body: AgreementList(
        onFolderSelect: (agreement) {
          Navigator.of(context).push(MaterialPageRoute(
            builder: (context) => AgreementUpdateScreen(
              agreementToUpdate: agreement,
            ),
          ));
        },
      ),
      appBar: AppBar(
        title: Text('TEST'),
      ),
      floatingActionButton: FloatingActionButton(
        child: Icon(Icons.add),
        onPressed: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (context) => AgreementCreateScreen(),
            ),
          );
        },
      ),
    );
  }
}
