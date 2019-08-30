import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/sprog.dart';
import 'package:bms_dart/src/blocs/dispatch_query_result.dart';
import 'package:bms_dart/src/models/approval_state.dart';
import '../refreshable.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class AccidentReportListBloc
    extends Bloc<AccidentReportListEvent, ListState<AccidentReport>>
    with Refreshable, DispatchQueryResult {
  final AccidentReportRepository _accidentReportRepository =
      repositoryProvider.accidentReportRepository();
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  AccidentReportListBloc(this._refreshEvent,
      {this.queryResultBloc, this.sprog}) {
    refresh();
  }

  @override
  ListState<AccidentReport> get initialState => Loading<AccidentReport>();

  @override
  Stream<ListState<AccidentReport>> mapEventToState(
    AccidentReportListEvent event,
  ) async* {
    if (event is AccidentReportListFetchOfUser) {
      var items = await _accidentReportRepository
          .fetchAccidentReportsOfUser(event.userId);
      if (items != null) {
        items = items.reversed.toList();
        yield Loaded(items: items, refreshTime: DateTime.now());
      } else
        yield Failure();
    }

    if (event is AccidentReportListFetchOfSignedInUser) {
      // yield* mapEventToRefreshing(event);

      var items = await _accidentReportRepository
          .fetchAccidentReportsOfUser(_authenticationRepository.getUserId());
      if (items != null) {
        items = items.reversed.toList();
        yield Loaded(items: items, refreshTime: DateTime.now());
      } else
        yield Failure();
    }

    if (event is AccidentReportListRespond) {
      final listState = currentState;
      var items = List<AccidentReport>();
      if (listState is Loaded<AccidentReport>) {
        items = listState.items;
        var item =
            items.firstWhere((i) => i.id == event.id, orElse: () => null);
        if (item != null && item.request.canRespondToApprovalState)
          item.request.canRespondToApprovalState = false;
        yield Loaded<AccidentReport>(items: items, refreshTime: DateTime.now());
        _accidentReportRepository
            .replyToAccidentReport(event.id, event.isApproved)
            .then((result) {
          dispatch(AccidentReportListResponded(
              id: event.id, isApproved: event.isApproved, success: result));
        });
      }
    }

    if (event is AccidentReportListResponded) {
      final listState = currentState;
      var items = List<AccidentReport>();
      if (listState is Loaded<AccidentReport>) {
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
          yield Loaded<AccidentReport>(
              items: items, refreshTime: DateTime.now());
        }
      }
    }
  }

  final AccidentReportListEvent Function() _refreshEvent;

  @override
  void refresh() => dispatch(_refreshEvent());
}
