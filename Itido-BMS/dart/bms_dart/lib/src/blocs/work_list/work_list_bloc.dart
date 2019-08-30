import 'dart:async';
import 'package:bloc/bloc.dart';

import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/sprog.dart';
import 'package:bms_dart/src/blocs/dispatch_query_result.dart';
import 'package:flutter/material.dart';
import '../refreshable.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class WorkListBloc extends Bloc<WorkListEvent, ListState<Work>>
    with Refreshable, DispatchQueryResult {
  final WorkRepository _workRepository = repositoryProvider.workRepository();
  final WorkRegistrationRepository _workRegistrationRepository =
      repositoryProvider.workRegistrationRepository();

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  WorkListBloc(
    this._refreshEvent, {
    @required this.queryResultBloc,
    @required this.sprog,
  }) {
    refresh();
  }

  DateTime from = DateTime.now().subtract(Duration(days: 2));
  DateTime to = DateTime.now().add(Duration(days: 30));
  bool withDate = true;

  @override
  ListState<Work> get initialState => Loading<Work>();

  @override
  Stream<ListState<Work>> mapEventToState(
    WorkListEvent event,
  ) async* {
    if (event is WorkListFetchAll) {
      try {
        _workRepository
            .fetchWorks(
          from: withDate ? from : null,
          to: withDate ? to : null,
        )
            .then((worksResult) {
          if (worksResult is Ok<List<Work>>) {
            dispatch(WorkListFetched(items: worksResult.value));
          }
        });
      } catch (_) {
        yield Failure<Work>();
      }
    }
    if (event is WorkListFetchOfUser) {
      try {
        _workRepository
            .fetchWorksOfUser(
          userId: event.userId,
          from: withDate ? from : null,
          to: withDate ? to : null,
        )
            .then((worksResult) {
          if (worksResult is Ok<List<Work>>) {
            dispatch(WorkListFetched(items: worksResult.value));
          }
        });
      } catch (_) {
        yield Failure<Work>();
      }
    }
    if (event is WorkListFetchOfSignedInUser) {
      try {
        _workRepository.fetchWorksOfSignedInUser().then((worksResult) {
          if (worksResult is Ok<List<Work>>) {
            dispatch(WorkListFetched(items: worksResult.value));
          }
        });
      } catch (_) {
        yield Failure<Work>();
      }
    }

    if (event is WorkListFetchOfProjectItem) {
      try {
        print('fetchWorksOfProjectItem');
        _workRepository
            .fetchWorksOfProjectItem(
          projectItemId: event.projectItemId,
          from: withDate ? from : null,
          to: withDate ? to : null,
        )
            .then((worksResult) {
          if (worksResult is Ok<List<Work>>) {
            dispatch(WorkListFetched(items: worksResult.value));
          }
        });
      } catch (_) {
        yield Failure<Work>();
      }
    }

    if (event is WorkListFetched) {
      var items = event.items;
      if (items != null) {
        items.sort((w1, w2) => w1.date.compareTo(w2.date));
        yield Loaded(items: items, refreshTime: DateTime.now());
      } else
        yield Failure();
    }

    if (event is WorkListRegister) {
      _workRegistrationRepository
          .register(event.workId,
              startTimeMins: event.startTimeMins,
              endTimeMins: event.endTimeMins)
          .then((result) {
        var oldState = currentState;
        dispatchQueryResult(result, sprog().registered);
        if (oldState is Loaded<Work> && result.successful) {
          var work = oldState.items.firstWhere((w) => w.id == event.workId);
          work.workRegistration = WorkRegistration(
            startTimeMins: event.startTimeMins,
            endTimeMins: event.endTimeMins,
          );
          dispatch(WorkListFetched(items: oldState.items));
        }
      });
    }

    if (event is WorkListAddUserContract) {
      _workRepository
          .addContract(event.work.id, event.contract.id)
          .then((result) {
        var oldState = currentState;
        dispatchQueryResult(result, sprog().addUser);
        if (oldState is Loaded<Work> && result.successful) {
          var work = oldState.items.firstWhere((w) => w.id == event.work.id);
          work.contract = event.contract..user = event.user;

          dispatch(WorkListFetched(items: oldState.items));
        }
      });
    }

    if (event is WorkListReplaceUserContract) {
      _workRepository.replace(event.work.id, event.contract.id).then((result) {
        var oldState = currentState;
        dispatchQueryResult(result, sprog().addUser);
        if (oldState is Loaded<Work> && result.successful) {
          var work = oldState.items.firstWhere((w) => w.id == event.work.id);
          work.workReplacement = (WorkReplacement()..contract = event.contract)
            ..contract.user = event.user;

          dispatch(WorkListFetched(items: oldState.items));
        }
      });
    }
    if (event is WorkListInviteUserContract) {
      _workRepository
          .inviteContractToWork(event.work.id, event.contract.id)
          .then((result) {
        dispatchQueryResult(result, sprog().inviteUser);
        var oldState = currentState;
        if (oldState is Loaded<Work> && result.successful) {
          refresh();
        }
      });
    }
    if (event is WorkListReplyToInvite) {
      _workRepository
          .replyToWorkInvite(event.work.id, event.answer)
          .then((result) {
        dispatchQueryResult(result, sprog().inviteReplied);
        var oldState = currentState;
        if (oldState is Loaded<Work> && result.successful) {
          refresh();
        }
      });
    }

    if (event is WorkListRemoveUser) {
      _workRepository.removeContract(event.workId).then((result) {
        dispatchQueryResult(result, sprog().removeUser);
        var oldState = currentState;
        if (oldState is Loaded<Work> && result.successful) {
          var work = oldState.items.firstWhere((w) => w.id == event.workId);
          work.contract = null;
          dispatch(WorkListFetched(items: oldState.items));
        }
      });
    }

    if (event is WorkListRemoveReplacer) {
      _workRepository.removeReplacer(event.workId).then((result) {
        dispatchQueryResult(result, sprog().removeUser);
        var oldState = currentState;
        if (oldState is Loaded<Work> && result.successful) {
          var work = oldState.items.firstWhere((w) => w.id == event.workId);
          work.workReplacement.contract = null;
          dispatch(WorkListFetched(items: oldState.items));
        }
      });
    }
  }

  final WorkListEvent Function() _refreshEvent;
  @override
  void refresh() => dispatch(_refreshEvent());
}
