import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/list_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class BlocList<B extends Bloc> extends StatefulWidget {
  final B Function(BuildContext) blocBuilder;
  final Function(B) onRefresh;
  final Widget child;
  final Widget Function(BuildContext, B) builder;

  const BlocList({
    Key key,
    this.blocBuilder,
    this.onRefresh,
    this.child,
    this.builder,
  })  : assert(child != null || builder != null),
        super(key: key);

  @override
  _BlocListState<B> createState() => _BlocListState<B>();
}

class _BlocListState<B extends Bloc> extends State<BlocList<B>> {
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
      child: (bloc is Refreshable) || widget.onRefresh != null
          ? RefreshIndicator(
              displacement: 120,
              child: _buildChild(bloc),
              onRefresh: () async {
                loading = Completer();
                if (bloc is Refreshable)
                  (bloc as Refreshable).refresh();
                else if (widget.onRefresh != null) widget.onRefresh(bloc);

                await loading.future;
              },
            )
          : _buildChild(bloc),
    );
  }

  Widget _buildChild(B bloc) {
    return widget.child != null ? widget.child : widget.builder(context, bloc);
  }
}
