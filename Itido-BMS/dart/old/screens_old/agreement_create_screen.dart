import 'package:bms_flutter/src/components/primary_button.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/src/widgets/agreement_create_update_form.dart';
import 'package:bms_dart/agreement_create_update_bloc.dart';

class AgreementCreateScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(Translations.of(context).titleCreateAgreement),
      ),
      body: BlocProvider(
        builder: (context) {
          return AgreementCreateUpdateBloc();
        },
        child: Builder(
          builder: (context) {
            var _bloc = BlocProvider.of<AgreementCreateUpdateBloc>(context);
            return BlocListener(
              bloc: _bloc,
              listener: (context, AgreementCreateUpdateState state) {
                if (state is CreateFailure) {
                  Scaffold.of(context)
                    ..removeCurrentSnackBar()
                    ..showSnackBar(SnackBar(
                      content:
                          Text(Translations.of(context).infoCreationFailed),
                    ));
                }
              },
              child: BlocBuilder(
                bloc: _bloc,
                builder: (context, AgreementCreateUpdateState state) {
                  return SingleChildScrollView(
                    child: Stack(
                      children: <Widget>[
                        Column(
                          crossAxisAlignment: CrossAxisAlignment.stretch,
                          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                          children: <Widget>[
                            AgreementCreateUpdateForm(),
                            PrimaryButton(
                              text: Translations.of(context).buttonCreate,
                              onPressed: () {
                                _bloc.dispatch(CreateRequested());
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
                },
              ),
            );
          },
        ),
      ),
    );
  }
}
