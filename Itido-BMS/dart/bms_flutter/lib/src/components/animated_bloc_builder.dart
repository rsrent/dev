import 'package:bloc/bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter/material.dart';
import 'animated_transition.dart';

typedef AnimatedBlocWidgetBuilder<S> = dynamic Function(
    BuildContext context, S state);

class AnimatedBlocBuilder<E, S> extends BlocBuilderBase<E, S> {
  final Bloc<E, S> bloc;

  /// The `builder` function which will be invoked on each widget build.
  /// The `builder` takes the [BuildContext] and current bloc state and
  /// must return a [Widget].
  /// This is analogous to the `builder` function in [StreamBuilder].
  final AnimatedBlocWidgetBuilder<S> builder;

  /// The `condition` function will be invoked on each bloc state change.
  /// The `condition` takes the previous state and current state and must return a `bool`
  /// which determines whether or not the `builder` function will be invoked.
  /// The previous state will be initialized to `currentState` when the `BlocBuilder` is initialized.
  /// `condition` is optional and if it isn't implemented, it will default to return `true`.
  final BlocBuilderCondition<S> condition;

  const AnimatedBlocBuilder({
    Key key,
    @required this.bloc,
    @required this.builder,
    this.condition,
  })  : assert(bloc != null),
        assert(builder != null),
        super(key: key, bloc: bloc);

  @override
  Widget build(BuildContext context, S state) {
    var child = builder(context, state);
    TransitionWidget widget;

    if (child == null) {
      child = AnimatedTransition.loadingTransition;
    }

    if (child is Widget) {
      var name = state.runtimeType.toString();
      // print('AnimatedBlocBuilder: $name');
      widget = TransitionWidget(
        child: child,
        name: name,
      );
    } else {
      widget = child;
      // print('AnimatedBlocBuilder: ${widget.name}');
    }

    return AnimatedTransition(
      duration: Duration(milliseconds: 500),
      revealType: RevealType.FadeIn,
      curve: Curves.easeOut,
      revealTypeReverse: RevealType.FadeIn,
      curveReverse: Curves.easeOut,
      child: widget,
    );
  }
}
