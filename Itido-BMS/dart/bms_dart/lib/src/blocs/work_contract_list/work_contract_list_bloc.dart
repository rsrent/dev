import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/sprog.dart';
import 'package:flutter/material.dart';
import '../dispatch_query_result.dart';
import '../refreshable.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class WorkContractListBloc
    extends Bloc<WorkContractListEvent, ListState<WorkContract>>
    with Refreshable, DispatchQueryResult {
  final WorkContractRepository _workContractRepository =
      repositoryProvider.workContractRepository();

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  WorkContractListBloc(
    this._refreshEvent, {
    @required this.queryResultBloc,
    @required this.sprog,
  }) {
    refresh();
  }

  @override
  ListState<WorkContract> get initialState => Loading<WorkContract>();

  @override
  Stream<ListState<WorkContract>> mapEventToState(
    WorkContractListEvent event,
  ) async* {
    if (event is WorkContractListFetchOfUser) {
      try {
        _workContractRepository
            .fetchWorkContractsOfUser(event.userId)
            .then((result) {
          dispatch(WorkContractListFetched(items: result.value));
        });
      } catch (_) {
        yield Failure<WorkContract>();
      }
    }
    if (event is WorkContractListFetchOfProjectItem) {
      try {
        _workContractRepository
            .fetchWorkContractsOfProjectItem(event.proejctItemId)
            .then((result) {
          dispatch(WorkContractListFetched(items: result.value));
        });
      } catch (_) {
        yield Failure<WorkContract>();
      }
    }

    if (event is WorkContractListFetched) {
      var items = event.items;
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }

    if (event is WorkContractListAddUserContract) {
      _workContractRepository
          .addContract(event.workContract.id, event.contract.id)
          .then((result) {
        var oldState = currentState;
        dispatchQueryResult(result, sprog().addUser);
        if (oldState is Loaded<WorkContract> && result.successful) {
          var workContract =
              oldState.items.firstWhere((w) => w.id == event.workContract.id);
          workContract.contract = event.contract..user = event.user;

          dispatch(WorkContractListFetched(items: oldState.items));
        }
      });
    }

    if (event is WorkContractListRemoveUser) {
      _workContractRepository
          .removeContract(event.workContractId)
          .then((result) {
        var oldState = currentState;
        dispatchQueryResult(result, sprog().addUser);
        if (oldState is Loaded<WorkContract> && result.successful) {
          var workContract =
              oldState.items.firstWhere((w) => w.id == event.workContractId);
          workContract.contract = null;
          dispatch(WorkContractListFetched(items: oldState.items));
        }
      });
    }
  }

  final WorkContractListEvent Function() _refreshEvent;
  @override
  void refresh() => dispatch(_refreshEvent());
}
