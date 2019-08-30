import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/log.dart';

@immutable
abstract class LogListEvent extends Equatable {
  LogListEvent([List props = const []]) : super(props);
}

class LogListFetchOfProjectItem extends LogListEvent {
  final int projectItemId;
  LogListFetchOfProjectItem({@required this.projectItemId})
      : super([projectItemId]);
  @override
  String toString() => 'LogListFetchOfProjectItem';
}

class LogListFetched extends LogListEvent {
  final List<Log> logs;

  LogListFetched({@required this.logs}) : super([logs]);
  @override
  String toString() => 'LogListFetched { logs: ${logs.length} }';
}

class LogListAddNew extends LogListEvent {
  final int projectItemId;
  LogListAddNew({@required this.projectItemId}) : super([projectItemId]);
  @override
  String toString() => 'LogListAddNew';
}
