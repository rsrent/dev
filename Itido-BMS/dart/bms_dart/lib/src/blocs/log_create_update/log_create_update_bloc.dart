import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:flutter/material.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class LogCreateUpdateBloc
    extends Bloc<LogCreateUpdateEvent, LogCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final LogRepository _logRepository = repositoryProvider.logRepository();

  final int logId;

  LogCreateUpdateBloc(this.logId);

  @override
  LogCreateUpdateState get initialState =>
      LogCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<LogCreateUpdateState> mapEventToState(
    LogCreateUpdateEvent event,
  ) async* {
    if (event is PrepareUpdate) {
      var log = await _logRepository.fetch(logId);
      print('preparing 1: ${currentState.createUpdateStatePhase}');
      yield LogCreateUpdateState.createOrCopy(null,
          log: log, createUpdateStatePhase: CreateUpdateStatePhase.Initial);
      print('preparing 2: ${currentState.createUpdateStatePhase}');
      yield LogCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
      print('preparing 3: ${currentState.createUpdateStatePhase}');
    }

    if (event is TitleChanged)
      yield LogCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress,
          changes: (accident) => accident.title = event.text);
    if (event is BodyChanged)
      yield LogCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress,
          changes: (accident) => accident.log = event.text);

    if (event is Commit) {
      var newState = LogCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result = await _logRepository.updateLog(logId, newState.log);
      print('reslt: $result');
      if (result) {
        yield LogCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield LogCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
