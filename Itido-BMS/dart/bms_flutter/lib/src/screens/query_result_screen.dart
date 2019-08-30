import 'package:bms_flutter/components.dart';
import 'package:bms_flutter/src/components/show_snack_text.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bms_dart/query_result_bloc.dart';
export 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_flutter/style.dart' as style;

class QueryResultScreen extends StatefulWidget {
  final List<QueryResultBloc> blocs;
  final String unauthorizedAccessTitle;
  final String unauthorizedAccessBody;
  final String defaultErrorTitle;
  final Widget child;
  const QueryResultScreen({
    Key key,
    @required this.blocs,
    @required this.child,
    this.unauthorizedAccessTitle,
    this.unauthorizedAccessBody,
    this.defaultErrorTitle,
  }) : super(key: key);
  @override
  _QueryResultScreenState createState() => _QueryResultScreenState();
}

class _QueryResultScreenState extends State<QueryResultScreen> {
  static const successIcon = Icon(
    Icons.check,
    color: style.acceptGreen,
    size: 70,
  );
  static const errorIcon = Icon(
    Icons.clear,
    color: style.declineRed,
    size: 70,
  );
  @override
  Widget build(BuildContext context) {
    Widget previous = widget.child;

    for (int i = 0; i < widget.blocs.length; i++) {
      previous = buildBlocListener(
        context,
        widget.blocs[i],
        previous,
      );
    }
    return previous;
/*
    return BlocListener(
      bloc: widget.bloc,
      listener: (context, QueryResultState state) {
        var text = '';
        Icon icon;

        if (state is SuccessfulQueryState) {
          text = "Success";
          icon = successIcon;
        }
        if (state is UnauthorizedQueryState) {
          text = "Unauthorized";
          icon = errorIcon;
        }
        if (state is ErrorQueryState) {
          text = "Error";
          icon = errorIcon;
        }
        if (state is CreateSuccessfulQueryState) {
          text = Translations.of(context).infoCreationSuccessful;
          icon = successIcon;
        }
        if (state is CreateFailedQueryState) {
          text = Translations.of(context).infoCreationFailed;
          icon = errorIcon;
        }
        if (state is UpdateSuccessfulQueryState) {
          text = Translations.of(context).infoUpdateSuccessful;
          icon = successIcon;
        }
        if (state is UpdateFailedQueryState) {
          text = Translations.of(context).infoUpdateFailed;
          icon = errorIcon;
        }

        showDialog(
          context: context,
          builder: (context) => Dialog(
            child: InkWell(
              child: Container(
                padding: EdgeInsets.all(20),
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.all(Radius.circular(16)),
                ),
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: <Widget>[
                    Text(
                      text,
                      style: TextStyle(
                        fontSize: 30,
                      ),
                    ),
                    Space(height: 8),
                    icon,
                  ],
                ),
              ),
              onTap: () {
                Navigator.of(context).pop();
              },
            ),
            backgroundColor: Colors.transparent,
          ),
        );
      },
      child: widget.child,
    );
    */
  }

  Widget buildBlocListener(
      BuildContext context, QueryResultBloc bloc, Widget child) {
    return BlocListener(
      bloc: bloc,
      listener: (context, QueryResultState state) {
        var title = '';
        String message;
        Icon icon;

        if (state is SuccessfulQueryState) {
          title = state.translations?.successTitle ?? 'Success';
          message = state.translations?.successMessage;
          icon = successIcon;
        }
        if (state is UnauthorizedQueryState) {
          title = 'Unauthorized';
          message = 'Du har ikke adgang til den forsøgte funktion';
          icon = Icon(Icons.block, size: 60, color: style.declineRed);
        }
        if (state is ErrorQueryState) {
          title = state.translations?.errorTitle ?? 'Error';
          message = state.translations?.errorMessage;
          icon = errorIcon;
        }

        showDialog(
          context: context,
          builder: (context) => Dialog(
            child: InkWell(
              child: Container(
                padding: EdgeInsets.all(20),
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.all(Radius.circular(16)),
                ),
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: <Widget>[
                    Text(
                      title,
                      style: TextStyle(
                        fontSize: 24,
                      ),
                    ),
                    Space(height: 8),
                    Text(message),
                    icon,
                  ],
                ),
              ),
              onTap: () {
                Navigator.of(context).pop();
              },
            ),
            backgroundColor: Colors.transparent,
          ),
        );
      },
      child: child,
    );
  }
}
