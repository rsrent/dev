import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';

@immutable
abstract class ListState<T> extends Equatable {
  ListState([List props = const []]) : super(props);
}

class Loading<T> extends ListState<T> {
  @override
  String toString() => 'Loading';
}

class Loaded<T> extends ListState<T> {
  final List<T> items;
  final DateTime refreshTime;
  final List<T> selectedItems;
  final bool selectable;

  Loaded({
    @required this.items,
    @required this.refreshTime,
    this.selectedItems,
    this.selectable = false,
  }) : super([
          items,
          refreshTime,
          selectedItems,
          selectable,
        ]);

  @override
  String toString() => 'Loaded { items: ${items.length} }';
}

// class Refreshing<T> extends ListState<T> {
//   final List<T> items;

//   Refreshing({@required this.items}) : super([items]);

//   @override
//   String toString() => 'Refreshing { items: ${items.length} }';
// }

class Failure<T> extends ListState<T> {
  @override
  String toString() => 'Failure';
}
