import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/log.dart';
import 'package:bms_dart/repositories.dart';

class LogListBloc extends Bloc<LogListEvent, ListState<Log>> with Refreshable {
  final LogRepository _logRepository = repositoryProvider.logRepository();

  final LogListEvent Function() _refreshEven;

  LogListBloc(this._refreshEven) {
    refresh();
  }

  @override
  ListState<Log> get initialState => Loading<Log>();

  @override
  Stream<ListState<Log>> mapEventToState(
    LogListEvent event,
  ) async* {
    if (event is LogListFetchOfProjectItem) {
      _logRepository
          .fetchOfProjectItem(event.projectItemId)
          .then((logs) => dispatch(LogListFetched(logs: logs)));
    }
    if (event is LogListFetched) {
      final items = event.logs;
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is LogListAddNew) {
      yield Loading();
      await _logRepository.createLog(event.projectItemId).then((logId) =>
          dispatch(
              LogListFetchOfProjectItem(projectItemId: event.projectItemId)));
    }
  }

  @override
  void refresh() {
    dispatch(_refreshEven());
  }
}
