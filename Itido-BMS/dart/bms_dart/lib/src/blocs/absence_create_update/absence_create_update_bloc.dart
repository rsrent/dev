import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/sprog.dart';
import 'package:bms_dart/src/blocs/dispatch_query_result.dart';
import 'package:flutter/foundation.dart';
import './bloc.dart';

class AbsenceCreateUpdateBloc
    extends Bloc<AbsenceCreateUpdateEvent, AbsenceCreateUpdateState>
    with DispatchQueryResult {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final AbsenceReasonRepository _absenceReasonRepository =
      repositoryProvider.absenceReasonRepository();
  final AbsenceRepository _absenceRepository =
      repositoryProvider.absenceRepository();

  int _userId;

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  AbsenceCreateUpdateBloc({
    int userId,
    @required this.sprog,
    @required this.queryResultBloc,
  }) {
    this._userId = userId;
  }

  @override
  AbsenceCreateUpdateState get initialState =>
      AbsenceCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<AbsenceCreateUpdateState> mapEventToState(
    AbsenceCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      var loadedAbsenceReason =
          await _absenceReasonRepository.fetchAllAbsenceReasons();
      yield AbsenceCreateUpdateState.createOrCopy(
        null,
        requester: _authenticationRepository.getUser(),
        userId: _userId,
        absence: Absence(
          from: DateTime.now(),
          to: DateTime.now().add(Duration(days: 2)),
          comment: '',
        ),
        allAbsenceReasons: loadedAbsenceReason,
        selectedAbsenceReason: null,
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: true,
      );
      yield AbsenceCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is PrepareUpdate) {
      var loadedAbsenceReason =
          await _absenceReasonRepository.fetchAllAbsenceReasons();
      var absence = await _absenceRepository.fetch(event.absence.id);
      yield AbsenceCreateUpdateState.createOrCopy(
        null,
        requester: _authenticationRepository.getUser(),
        userId: _userId,
        absence: absence.value,
        allAbsenceReasons: loadedAbsenceReason,
        selectedAbsenceReason: loadedAbsenceReason.firstWhere(
            (a) => a.id == event.absence.absenceReason.id,
            orElse: () => null),
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: false,
      );
      yield AbsenceCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is AbsenceReasonChanged)
      yield AbsenceCreateUpdateState.createOrCopy(currentState,
          selectedAbsenceReason: event.absenceReason);
    if (event is FromChanged)
      yield AbsenceCreateUpdateState.createOrCopy(currentState,
          absenceChanges: (absence) => absence.from = event.dateTime);
    if (event is ToChanged)
      yield AbsenceCreateUpdateState.createOrCopy(currentState,
          absenceChanges: (absence) => absence.to = event.dateTime);
    if (event is CommentChanged)
      yield AbsenceCreateUpdateState.createOrCopy(currentState,
          absenceChanges: (absence) => absence.comment = event.text);

    if (event is Commit) {
      var newState = AbsenceCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      QueryResult result;
      if (newState.isCreate) {
        result = await _absenceRepository.createAbsence(newState.absence,
            _userId, newState.selectedAbsenceReason.id, event.asRequest);
        dispatchQueryResult(
            result,
            event.asRequest
                ? sprog().absenceRequested
                : sprog().absenceCreated);
      } else {
        result = await _absenceRepository.updateAbsence(newState.absence);
        dispatchQueryResult(result, sprog().updateAttempted);
      }
      if (result.successful) {
        yield AbsenceCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield AbsenceCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
