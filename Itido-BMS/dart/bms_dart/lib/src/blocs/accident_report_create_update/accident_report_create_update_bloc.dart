import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:flutter/material.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class AccidentReportCreateUpdateBloc extends Bloc<
    AccidentReportCreateUpdateEvent, AccidentReportCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final AccidentReportRepository _accidentReportRepository =
      repositoryProvider.accidentReportRepository();

  final AccidentReportType accidentReportType;
  final int userId;

  AccidentReportCreateUpdateBloc({this.accidentReportType, this.userId});

  @override
  AccidentReportCreateUpdateState get initialState =>
      AccidentReportCreateUpdateState.createOrCopy(null);

  @override
  Stream<AccidentReportCreateUpdateState> mapEventToState(
    AccidentReportCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      yield AccidentReportCreateUpdateState.createOrCopy(
        null,
        isCreate: true,
        accidentReport: AccidentReport(
          accidentReportType: accidentReportType,
          dateTime: DateTime.now(),
          place: '',
          description: '',
          actionTaken: '',
          absenceDurationDays: 0,
        ),
      );

      yield AccidentReportCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    // if (event is PrepareUpdate) {
    //   yield AccidentReportCreateUpdateState.createOrCopy(null,
    //       accidentReport: event.accidentReport, isCreate: false);

    //   yield AccidentReportCreateUpdateState.createOrCopy(currentState,
    //       createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    // }

    if (event is DateTimeChanged)
      yield AccidentReportCreateUpdateState.createOrCopy(currentState,
          changes: (accident) => accident.dateTime = event.dateTime);
    if (event is PlaceChanged)
      yield AccidentReportCreateUpdateState.createOrCopy(currentState,
          changes: (accident) => accident.place = event.text);
    if (event is DescriptionChanged)
      yield AccidentReportCreateUpdateState.createOrCopy(currentState,
          changes: (accident) => accident.description = event.text);
    if (event is ActionTakenChanged)
      yield AccidentReportCreateUpdateState.createOrCopy(currentState,
          changes: (accident) => accident.actionTaken = event.text);
    if (event is AbsenceDurationChanged)
      yield AccidentReportCreateUpdateState.createOrCopy(currentState,
          changes: (accident) => accident.absenceDurationDays = event.days);

    if (event is Commit) {
      var newState = AccidentReportCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result;
      if (newState.isCreate) {
        result = (await _accidentReportRepository.create(
                newState.accidentReport, userId)) !=
            null;
      } else {}
      if (result) {
        yield AccidentReportCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield AccidentReportCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
