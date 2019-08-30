import 'dart:async';
import 'package:bloc/bloc.dart';
import '../refreshable.dart';
import './bloc.dart';
import '../../models/noti.dart';
import 'package:bms_dart/repositories.dart';

class NotiListBloc extends Bloc<NotiListEvent, ListState<Noti>>
    with Refreshable {
  final NotiRepository _notiRepository = repositoryProvider.notiRepository();

  @override
  ListState<Noti> get initialState => Loading<Noti>();

  NotiListBloc(this._refreshEvent) {
    refresh();
  }
  final NotiListEvent Function() _refreshEvent;
  void refresh() => dispatch(_refreshEvent());

  @override
  Stream<ListState<Noti>> mapEventToState(
    NotiListEvent event,
  ) async* {
    if (event is NotiListFetchLatest) {
      final items = await _notiRepository.fetchLatestNotis(event.count);
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is NotiSeen) {
      event.noti.seen = true;
      _notiRepository.setNotiSeen(event.noti.id);
      var old = currentState;
      if (old is Loaded<Noti>) {
        yield Loaded(items: old.items, refreshTime: DateTime.now());
      }
    }
  }
}
