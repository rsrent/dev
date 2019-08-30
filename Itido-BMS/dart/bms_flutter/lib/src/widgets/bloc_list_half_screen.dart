import 'package:bloc/bloc.dart';
import 'package:bms_dart/blocs.dart';
import 'package:bms_dart/list_bloc.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_flutter/style.dart' as style;
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../translations.dart';
import 'bloc_list.dart';

class BlocListHalfScreen<B extends Bloc<E, S>, E, S extends ListState<M>, M>
    extends StatefulWidget {
  final B Function(BuildContext) blocBuilder;
  final void Function(B) onRefresh;
  final Widget child;
  final Widget Function(BuildContext, B, S) builder;

  final Widget floatingActionButton;
  final void Function(B, List<M>) onSelectMany;

  final void Function(B) toggleSelectable;
  final void Function(B) onSelectAll;
  final void Function(B) onClearAll;
  final void Function(String) onSearchTextUpdated;

  final bool showSearchableAppBar;
  final bool searchableAppBarIsPrimary;

  const BlocListHalfScreen({
    Key key,
    this.blocBuilder,
    this.onRefresh,
    this.builder,
    this.child,
    this.floatingActionButton,
    this.onSelectMany,
    this.onSelectAll,
    this.onClearAll,
    this.showSearchableAppBar,
    this.searchableAppBarIsPrimary,
    this.onSearchTextUpdated,
    this.toggleSelectable,
  })  : assert(child != null || builder != null),
        super(key: key);

  @override
  _BlocListHalfScreenState createState() =>
      _BlocListHalfScreenState<B, E, S, M>(showSearchableAppBar);
}

class _BlocListHalfScreenState<B extends Bloc<E, S>, E, S extends ListState<M>,
    M> extends State<BlocListHalfScreen<B, E, S, M>> {
  TextEditingController controller;

  _BlocListHalfScreenState(bool showSearchableAppBar) {
    this.controller =
        (showSearchableAppBar ?? false) ? TextEditingController() : null;
  }

  @override
  Widget build(BuildContext context) {
    var primary = widget.searchableAppBarIsPrimary ?? true;

    return BlocList(
      blocBuilder: widget.blocBuilder,
      //onRefresh: onRefresh,
      builder: (context, B bloc) {
        Selectable selectableBloc =
            (bloc is Selectable) ? (bloc as Selectable) : null;
        var toggleSelectable = selectableBloc != null
            ? () => selectableBloc.toggleSelectable()
            : widget.toggleSelectable != null
                ? () => widget.toggleSelectable(bloc)
                : null;
        var selectAll = selectableBloc != null
            ? () => selectableBloc.selectAll()
            : widget.onSelectAll != null
                ? () => widget.onSelectAll(bloc)
                : null;
        var clearAll = selectableBloc != null
            ? () => selectableBloc.clear()
            : widget.onClearAll != null ? () => widget.onClearAll(bloc) : null;

        return BlocBuilder(
          bloc: bloc,
          builder: (context, ListState<M> state) {
            var filters = (bloc is Searchable) &&
                    (bloc as Searchable).allFilters().length > 0
                ? (bloc as Searchable).filters
                : null;

            return Column(
              children: <Widget>[
                if (controller != null)
                  Padding(
                    padding: EdgeInsets.only(
                      top: primary ? 0 : 16,
                      left: primary ? 0 : 16,
                      right: primary ? 0 : 16,
                    ),
                    child: SearchableAppBar(
                      controller: controller,
                      onChanged: (text) {
                        if (bloc is Searchable)
                          (bloc as Searchable).searchTextUpdated(text);
                        else if (widget.onSearchTextUpdated != null)
                          widget.onSearchTextUpdated(text);
                      },
                      hintText: Translations.of(context).hintSearch,
                      primary: primary,
                      actions: <Widget>[
                        if (filters != null)
                          IconButton(
                            icon: Icon(
                              Icons.filter_list,
                              color: filters.length > 0
                                  ? Theme.of(context).primaryColor
                                  : null,
                            ),
                            onPressed: () => showFilters(bloc),
                          )
                      ],
                    ),
                  ),
                Expanded(
                  child: Stack(
                    children: <Widget>[
                      widget.child != null
                          ? widget.child
                          : widget.builder(context, bloc, state),
                      Positioned(
                        bottom: 8,
                        right: 8,
                        left: 8,
                        child: AnimatedTransition(
                          height: 200,
                          revealType: RevealType.SlideInFromRight,
                          revealTypeReverse: RevealType.SlideInFromRight,
                          child: state is Loaded<M> &&
                                  state.selectable &&
                                  widget.onSelectMany != null
                              ? TransitionWidget(
                                  name: 'selectable',
                                  child: Column(
                                    children: <Widget>[
                                      Expanded(child: Container()),
                                      Container(
                                        height: 70,
                                        child: Row(
                                          children: <Widget>[
                                            Expanded(child: Container()),
                                            Card(
                                              child: ButtonBar(
                                                children: <Widget>[
                                                  if (selectAll != null)
                                                    InkWell(
                                                      child: Padding(
                                                        padding:
                                                            const EdgeInsets
                                                                .all(12),
                                                        child: Text(
                                                          'Select all',
                                                          style: TextStyle(
                                                              fontWeight:
                                                                  FontWeight
                                                                      .bold,
                                                              color: Theme.of(
                                                                      context)
                                                                  .primaryColor),
                                                        ),
                                                      ),
                                                      onTap: selectAll,
                                                    ),
                                                  InkWell(
                                                    child: Padding(
                                                      padding:
                                                          const EdgeInsets.all(
                                                              12),
                                                      child: Text(
                                                        'Select (${state.selectedItems.length})',
                                                        style: TextStyle(
                                                            fontWeight:
                                                                FontWeight.bold,
                                                            color: Theme.of(
                                                                    context)
                                                                .primaryColor),
                                                      ),
                                                    ),
                                                    onTap: () {
                                                      if (widget.onSelectMany !=
                                                          null)
                                                        widget.onSelectMany(
                                                            bloc,
                                                            state
                                                                .selectedItems);
                                                    },
                                                  ),
                                                  if (clearAll != null)
                                                    InkWell(
                                                      child: Padding(
                                                        padding:
                                                            const EdgeInsets
                                                                .all(12),
                                                        child: Text(
                                                          'Clear',
                                                          style: TextStyle(
                                                              fontWeight:
                                                                  FontWeight
                                                                      .bold,
                                                              color: style
                                                                  .declineRed),
                                                        ),
                                                      ),
                                                      onTap: clearAll,
                                                    ),
                                                ],
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),
                                    ],
                                  ),
                                )
                              : TransitionWidget(
                                  name: 'floatingActionButton',
                                  child: Align(
                                    alignment: Alignment.bottomRight,
                                    child: Padding(
                                      padding: const EdgeInsets.only(
                                        right: 8,
                                        bottom: 8,
                                      ),
                                      child: Column(
                                        mainAxisAlignment:
                                            MainAxisAlignment.end,
                                        children: <Widget>[
                                          Padding(
                                            padding: const EdgeInsets.only(
                                              bottom: 8.0,
                                            ),
                                            child: toggleSelectable != null &&
                                                    widget.onSelectMany != null
                                                ? FloatingActionButton(
                                                    child: Icon(Icons.check),
                                                    onPressed: toggleSelectable,
                                                    mini: true,
                                                  )
                                                : null,
                                          ),
                                          if (widget.floatingActionButton !=
                                              null)
                                            widget.floatingActionButton
                                        ],
                                      ),
                                    ),
                                  ),
                                ),
                        ),
                      ),
                    ],
                  ),
                )
              ],
            );
          },
        );
      },
    );
  }

  void showFilters(B bloc) {
    var filterBloc = bloc as Searchable;

    showModalBottomSheet(
      context: context,
      builder: (context) {
        return BlocBuilder(
          bloc: bloc,
          builder: (context, state) {
            return Container(
              height: 400,
              child: ListView.builder(
                padding: EdgeInsets.only(top: 16, bottom: 80),
                itemCount: filterBloc.allFilters().length,
                itemBuilder: (context, index) {
                  var filter = filterBloc.allFilters()[index];

                  return ListTile(
                    leading: CircleAvatar(
                      backgroundColor: filterBloc.filters.contains(filter)
                          ? Theme.of(context).primaryColor
                          : Colors.grey[300],
                    ),
                    title: Text(filter),
                    onTap: () {
                      filterBloc.toggleFilter(filter);
                    },
                  );
                },
              ),
            );
          },
        );
      },
    );
  }
}
