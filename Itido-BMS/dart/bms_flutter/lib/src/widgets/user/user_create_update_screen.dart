import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/user_create_update_bloc.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'user_create_update_form.dart';

class UserCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    User user,
    int customerId,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => UserCreateUpdateScreen(
        userToUpdate: user,
        customerId: customerId,
      ),
    ));
  }

  final User userToUpdate;
  final int customerId;
  final bool isCreate;

  const UserCreateUpdateScreen({
    Key key,
    this.userToUpdate,
    this.customerId,
  })  : isCreate = userToUpdate == null,
        super(key: key);

  @override
  _UserCreateUpdateScreenState createState() => _UserCreateUpdateScreenState();
}

class _UserCreateUpdateScreenState extends State<UserCreateUpdateScreen> {
  bool updated = false;
  var queryResultBloc = QueryResultBloc();

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async => false,
      child: Scaffold(
        appBar: AppBar(
          leading: IconButton(
            icon: Icon(Icons.arrow_back),
            onPressed: () {
              Navigator.of(context).pop(updated);
            },
          ),
          title: Text(
            widget.isCreate
                ? Translations.of(context).titleCreateUser
                : Translations.of(context).titleUpdateUser,
          ),
        ),
        body: QueryResultScreen(
          blocs: [queryResultBloc],
          child: BlocProvider(
            builder: (context) {
              if (widget.isCreate)
                return UserCreateUpdateBloc(
                  customerId: widget.customerId,
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                )..dispatch(PrepareCreate());
              else
                return UserCreateUpdateBloc(
                  queryResultBloc: queryResultBloc,
                  sprog: () => Translations.of(context),
                )..dispatch(PrepareUpdate(user: widget.userToUpdate));
            },
            child: Builder(
              builder: (context) {
                var _bloc = BlocProvider.of<UserCreateUpdateBloc>(context);

                return BlocListener(
                  bloc: _bloc,
                  listener: (context, UserCreateUpdateState state) {
                    print('${state.createUpdateStatePhase}');
                    if (state.createUpdateStatePhase ==
                        CreateUpdateStatePhase.Failed) {
                      showSnackText(
                          context,
                          widget.isCreate
                              ? Translations.of(context).infoCreationFailed
                              : Translations.of(context).infoUpdateFailed);
                    }
                    if (state.createUpdateStatePhase ==
                        CreateUpdateStatePhase.Successful) {
                      updated = true;
                      showSnackText(
                          context,
                          widget.isCreate
                              ? Translations.of(context).infoCreationSuccessful
                              : Translations.of(context).infoUpdateSuccessful);
                    }
                  },
                  child: UserCreateUpdateForm(),
                );
              },
            ),
          ),
        ),
      ),
    );
  }
}
