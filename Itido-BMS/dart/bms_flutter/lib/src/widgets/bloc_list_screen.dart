import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:bms_dart/list_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class BlocListScreen<B extends Bloc> extends StatefulWidget {
  final B Function(BuildContext) blocBuilder;
  final Function(B) onRefresh;
  final Widget body;
  final Widget appBar;
  final Widget Function(BuildContext, B) appBarBuilder;
  final Widget floatingActionButton;

  const BlocListScreen({
    Key key,
    this.blocBuilder,
    this.onRefresh,
    @required this.body,
    this.appBar,
    this.floatingActionButton,
    this.appBarBuilder,
  }) : super(key: key);

  @override
  _BlocListScreenState<B> createState() => _BlocListScreenState<B>();
}

class _BlocListScreenState<B extends Bloc> extends State<BlocListScreen<B>> {
  Completer loading;

  @override
  Widget build(BuildContext context) {
    if (widget.blocBuilder != null) {
      return BlocProvider<B>(
        builder: widget.blocBuilder,
        child: Builder(
          builder: _buildBlocListener,
        ),
      );
    } else {
      return _buildBlocListener(context);
    }
  }

  Widget _buildBlocListener(BuildContext context) {
    var bloc = BlocProvider.of<B>(context);
    return BlocListener(
      bloc: bloc,
      listener: (context, dynamic state) {
        if (state is Loaded || state is Failure) {
          if (!(loading?.isCompleted ?? true)) loading?.complete();
        }
      },
      child: Scaffold(
        appBar: widget.appBarBuilder != null
            ? widget.appBarBuilder(context, bloc)
            : widget.appBar,
        body: widget.onRefresh != null
            ? RefreshIndicator(
                child: widget.body,
                onRefresh: () async {
                  loading = Completer();
                  widget.onRefresh(bloc);
                  await loading.future;
                },
              )
            : widget.body,
        floatingActionButton: widget.floatingActionButton,
      ),
    );
  }
}
