import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/repositories/absence_reason_repository.dart';
import '../refreshable.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class AbsenceReasonListBloc
    extends Bloc<AbsenceReasonListEvent, ListState<AbsenceReason>>
    with Refreshable {
  final AbsenceReasonRepository _absenceReasonRepository =
      repositoryProvider.absenceReasonRepository();

  AbsenceReasonListBloc(this._refreshEvent) {
    refresh();
  }

  @override
  ListState<AbsenceReason> get initialState => Loading();

  @override
  Stream<ListState<AbsenceReason>> mapEventToState(
    AbsenceReasonListEvent event,
  ) async* {
    if (event is AbsenceReasonListFetch) {
      final items = await _absenceReasonRepository.fetchAllAbsenceReasons();
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }
  }

  final AbsenceReasonListEvent Function() _refreshEvent;

  @override
  void refresh() => dispatch(_refreshEvent());
}
