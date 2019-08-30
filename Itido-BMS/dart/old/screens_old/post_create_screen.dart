import 'package:bms_flutter/src/components/primary_button.dart';
import 'package:bms_flutter/src/components/primary_button.dart';
import 'package:bms_flutter/src/widgets/loading_overlay.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_flutter/translations.dart';
import 'package:dart_packages/streamer.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_flutter/src/widgets/post_create_update_form.dart';
import 'package:bms_dart/post_create_update_bloc.dart';
import 'package:rxdart/rxdart.dart';

class PostCreateScreen extends StatelessWidget {
  final Streamer<bool> _loading = Streamer(seedValue: false);
  PostCreateScreen();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(Translations.of(context).titleCreatePost),
      ),
      body: BlocProvider(
        builder: (context) {
          return PostCreateUpdateBloc()..dispatch(PrepareCreate());
        },
        child: Builder(
          builder: (context) {
            var _bloc = BlocProvider.of<PostCreateUpdateBloc>(context);
            return BlocListener(
              bloc: _bloc,
              listener: (context, PostCreateUpdateState state) {
                _loading.update(state is Loading);

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
                builder: (context, PostCreateUpdateState state) {
                  return bodyBuilder(context, state, _bloc);
                },
              ),
            );
          },
        ),
      ),
    );
  }

  Widget bodyBuilder(BuildContext context, PostCreateUpdateState state,
      PostCreateUpdateBloc bloc) {
    return Center(
      child: Stack(
        fit: StackFit.expand,
        children: <Widget>[
          SingleChildScrollView(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: <Widget>[
                PostCreateUpdateForm(),
              ],
            ),
          ),
          if (state is Loading) Center(child: CircularProgressIndicator())
        ],
      ),
    );
  }
}
