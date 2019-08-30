import 'package:bms_flutter/src/components/primary_button.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/src/widgets/contract_create_update_form.dart';
import 'package:bms_dart/contract_create_update_bloc.dart';

class ContractCreateScreen extends StatelessWidget {
  final User user;
  ContractCreateScreen({@required this.user});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(Translations.of(context).titleCreateContract),
      ),
      body: BlocProvider(
        builder: (context) {
          return ContractCreateUpdateBloc(user: user)
            ..dispatch(PrepareCreate());
        },
        child: Builder(
          builder: (context) {
            var _bloc = BlocProvider.of<ContractCreateUpdateBloc>(context);
            return BlocListener(
              bloc: _bloc,
              listener: (context, ContractCreateUpdateState state) {
                if (state is CreateFailure) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content:
                          Text(Translations.of(context).infoCreationFailed),
                    ));
                } else if (state is CreateSuccessful) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content:
                          Text(Translations.of(context).infoCreationSuccessful),
                    ));
                  _bloc.dispatch(PrepareCreate());
                }
              },
              child: BlocBuilder(
                bloc: _bloc,
                builder: (context, ContractCreateUpdateState state) {
                  return bodyBuilder(context, state, _bloc);
                },
              ),
            );
          },
        ),
      ),
    );
  }

  Widget bodyBuilder(BuildContext context, ContractCreateUpdateState state,
      ContractCreateUpdateBloc bloc) {
    return SingleChildScrollView(
      child: Stack(
        children: <Widget>[
          Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: <Widget>[
              ContractCreateUpdateForm(
                isCreate: false,
              ),
              StreamBuilder<bool>(
                stream: bloc.formValid.stream,
                builder: (BuildContext context, AsyncSnapshot snapshot) {
                  return PrimaryButton(
                    onPressed: () {
                      bloc.dispatch(CreateRequested());
                    },
                    text: Translations.of(context).buttonCreate,
                    disabled: !(snapshot.data ?? false),
                  );
                },
              ),
            ],
          ),
          if (state is Loading)
            Center(
              child: CircularProgressIndicator(),
            )
        ],
      ),
    );
  }
}
