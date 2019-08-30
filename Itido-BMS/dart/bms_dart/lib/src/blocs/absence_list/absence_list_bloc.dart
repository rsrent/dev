import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/sprog.dart';
import 'package:bms_dart/src/blocs/dispatch_query_result.dart';
import 'package:bms_dart/src/models/approval_state.dart';
import 'package:flutter/foundation.dart';
import '../refreshable.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class AbsenceListBloc extends Bloc<AbsenceListEvent, ListState<Absence>>
    with Refreshable, DispatchQueryResult {
  final AbsenceRepository _absenceRepository =
      repositoryProvider.absenceRepository();
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  AbsenceListBloc(
    this._refreshEvent, {
    @required this.queryResultBloc,
    @required this.sprog,
  }) {
    refresh();
  }

  @override
  ListState<Absence> get initialState => Loading<Absence>();

  @override
  Stream<ListState<Absence>> mapEventToState(
    AbsenceListEvent event,
  ) async* {
    // if (event is AbsenceListFetchAll) {
    //   // yield* mapEventToRefreshing(event);
    //   final items = await _absenceRepository.fetchAllAbsences();
    //   if (items != null)
    //     yield Loaded(items: items, refreshTime: DateTime.now());
    //   else
    //     yield Failure();
    // }
    if (event is AbsenceListFetchOfUser) {
      // yield* mapEventToRefreshing(event);

      final result = await _absenceRepository.fetchAbsencesOfUser(event.userId);

      if (result.successful)
        yield Loaded(
            items: result.value.reversed.toList(), refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is AbsenceListFetchOfSignedInUser) {
      // yield* mapEventToRefreshing(event);

      final result = await _absenceRepository
          .fetchAbsencesOfUser(_authenticationRepository.getUserId());
      if (result.successful)
        yield Loaded(
            items: result.value.reversed.toList(), refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is AbsenceListRespond) {
      final listState = currentState;
      var items = List<Absence>();
      if (listState is Loaded<Absence>) {
        items = listState.items;
        var item =
            items.firstWhere((i) => i.id == event.id, orElse: () => null);
        if (item != null && item.request.canRespondToApprovalState) {
          item.request.canRespondToApprovalState = false;
        }
        yield Loaded<Absence>(items: items, refreshTime: DateTime.now());
        _absenceRepository
            .replyToAbsence(event.id, event.isApproved)
            .then((result) {
          dispatchQueryResult(result, sprog().absenceReplied);
          dispatch(AbsenceListResponded(
              id: event.id,
              isApproved: event.isApproved,
              success: result.successful));
        });
      }
    }

    if (event is AbsenceListResponded) {
      final listState = currentState;
      var items = List<Absence>();
      if (listState is Loaded<Absence>) {
        items = listState.items;
        var item =
            items.firstWhere((i) => i.id == event.id, orElse: () => null);
        if (item != null) {
          if (event.success) {
            item.request.approvalState = event.isApproved
                ? ApprovalState.Approved
                : ApprovalState.Denied;
          } else {
            item.request.canRespondToApprovalState = true;
          }
          yield Loaded<Absence>(items: items, refreshTime: DateTime.now());
        }
      }
    }
  }

  final AbsenceListEvent Function() _refreshEvent;

  @override
  void refresh() => dispatch(_refreshEvent());

  // Stream<ListState<Absence>> mapEventToRefreshing(
  //     AbsenceListEvent event) async* {
  //   if (currentState is Loaded) {
  //     yield Refreshing(items: (currentState as Loaded).items);
  //   }
  // }
}
