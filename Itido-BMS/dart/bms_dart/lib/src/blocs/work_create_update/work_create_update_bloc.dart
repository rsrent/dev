import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/sprog.dart';
import 'package:flutter/material.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import '../dispatch_query_result.dart';
import './bloc.dart';

class WorkCreateUpdateBloc
    extends Bloc<WorkCreateUpdateEvent, WorkCreateUpdateState>
    with DispatchQueryResult {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final WorkRepository _workRepository = repositoryProvider.workRepository();
  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  final int projectItemId;

  WorkCreateUpdateBloc({
    this.projectItemId,
    @required this.queryResultBloc,
    @required this.sprog,
  });

  @override
  WorkCreateUpdateState get initialState =>
      WorkCreateUpdateState.createOrCopy(null);

  @override
  Stream<WorkCreateUpdateState> mapEventToState(
    WorkCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      var today = DateTime.now();
      yield WorkCreateUpdateState.createOrCopy(null,
          isCreate: true,
          work: Work(
            isVisible: true,
            note: '',
            breakMins: 30,
            date: DateTime(today.year, today.month, today.day),
            startTimeMins: 8 * 60,
            endTimeMins: 12 * 60,
          ));

      yield WorkCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is PrepareUpdate) {
      yield WorkCreateUpdateState.createOrCopy(
        null,
        work: event.work,
        isCreate: false,
      );

      yield WorkCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is DateChanged)
      yield WorkCreateUpdateState.createOrCopy(
        currentState,
        changes: (w) => w.date = event.date,
      );
    if (event is NoteChanged)
      yield WorkCreateUpdateState.createOrCopy(
        currentState,
        changes: (work) => work.note = event.note,
      );
    if (event is StartTimeMinsChanged)
      yield WorkCreateUpdateState.createOrCopy(
        currentState,
        changes: (work) => work.startTimeMins = event.mins,
      );
    if (event is EndTimeMinsChanged)
      yield WorkCreateUpdateState.createOrCopy(
        currentState,
        changes: (work) => work.endTimeMins = event.mins,
      );
    if (event is BreakMinsChanged)
      yield WorkCreateUpdateState.createOrCopy(
        currentState,
        changes: (work) => work.breakMins = event.mins,
      );
    if (event is IsVisibleChanged)
      yield WorkCreateUpdateState.createOrCopy(
        currentState,
        changes: (work) => work.isVisible = event.isVisible,
      );

    if (event is Commit) {
      var newState = WorkCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      QueryResult commitResult;
      if (newState.isCreate) {
        commitResult =
            await _workRepository.createWork(newState.work, this.projectItemId);
        dispatchQueryResult(commitResult, sprog().createAttempted);
      } else {
        commitResult = await _workRepository.updateWork(newState.work);
        dispatchQueryResult(commitResult, sprog().updateAttempted);
      }

      if (commitResult.successful) {
        yield WorkCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield WorkCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
