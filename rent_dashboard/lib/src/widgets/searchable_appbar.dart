import 'package:flutter/material.dart';
import '../blocs/sortable_bloc.dart';

/*
class SearchableAppBar extends StatelessWidget {
  final String title;
  final SortableBloc bloc;
  SearchableAppBar({this.title, this.bloc});

  @override
  Widget build(BuildContext context) {
    
} */

AppBar searchableAppBar({String title, SortableBloc bloc}) {
  return AppBar(
    title: TextField(
      onChanged: bloc.filterBy,
      decoration: InputDecoration(
        hintText: '$title',
        hintStyle: TextStyle(color: Colors.white),
        icon: Icon(
          Icons.search,
          color: Colors.white,
        ),
        fillColor: Colors.white,
      ),
    ),
  );
}
