import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/query_result.dart';
import 'package:bms_dart/query_result_bloc.dart';
import 'package:bms_dart/sprog.dart';
import 'package:bms_dart/src/blocs/create_update_state_phase.dart';
import 'package:dart_packages/tuple.dart';
import 'package:flutter/material.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../dispatch_query_result.dart';
import './bloc.dart';

class WorkContractCreateUpdateBloc
    extends Bloc<WorkContractCreateUpdateEvent, WorkContractCreateUpdateState>
    with DispatchQueryResult {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final WorkContractRepository _workContractRepository =
      repositoryProvider.workContractRepository();
  final HolidayRepository _holidayRepository =
      repositoryProvider.holidayRepository();

  final int projectItemId;

  DateTime _firstDate;
  DateTime get firstDate => _firstDate;

  final QueryResultBloc queryResultBloc;
  QueryResultBloc getQueryResultBloc() => queryResultBloc;
  final Sprog Function() sprog;

  WorkContractCreateUpdateBloc({
    this.projectItemId,
    @required this.queryResultBloc,
    @required this.sprog,
  });

  @override
  WorkContractCreateUpdateState get initialState =>
      WorkContractCreateUpdateState.createOrCopy(null);

  @override
  Stream<WorkContractCreateUpdateState> mapEventToState(
    WorkContractCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      _firstDate = DateTime(
          DateTime.now().year, DateTime.now().month, DateTime.now().day);

      var holidays = await _holidayRepository.fetchHolidays("DK");

      var workContract = WorkContract(
        note: '',
        fromDate: firstDate,
        toDate: firstDate.add(Duration(days: 7)),
        isVisible: true,
        workDays: List.generate(
          14,
          (i) => WorkDay(
            index: i,
            breakMins: 30,
            dayOfWeek: i % 7,
            isEvenWeek: i < 7 ? 1 : 2,
            startTimeMins: 0,
            endTimeMins: 0,
          ),
        ),
        workHolidays: [],
      );

      yield WorkContractCreateUpdateState(
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress,
          isCreate: true,
          workContract: workContract,
          evenUnevenWeeks: true,
          holidays: (holidays ?? [])
              .map<Tuple2<Holiday, bool>>((h) => Tuple2<Holiday, bool>(h, true))
              .toList());
    }

    if (event is PrepareUpdate) {
      _firstDate = event.workContract.fromDate;

      var workContract =
          await _workContractRepository.fetch(event.workContract.id);

      var isEvenUnEven =
          workContract.value.workDays.any((w) => w.isEvenWeek != 0);

      var workDays = List<WorkDay>.generate(
          isEvenUnEven ? 14 : 7,
          (i) => workContract.value.workDays.firstWhere(
              (wd) => !isEvenUnEven
                  ? wd.dayOfWeek == i
                  : (i < 7 && wd.isEvenWeek == 1 && wd.dayOfWeek == i) ||
                      (i >= 7 && wd.isEvenWeek == 2 && wd.dayOfWeek == (i - 7)),
              orElse: () => WorkDay(
                    startTimeMins: 0,
                    endTimeMins: 0,
                    dayOfWeek: i % 7,
                    isEvenWeek: isEvenUnEven ? i < 7 ? 1 : 2 : 0,
                    breakMins: 30,
                    workContractId: workContract.value.id,
                  ))
            ..index = i);

      workContract.value.workDays = workDays;

      var allHolidays = await _holidayRepository.fetchHolidays("DK");

      var _holidays = allHolidays
          .map<Tuple2<Holiday, bool>>((h) => Tuple2<Holiday, bool>(
              h,
              workContract.value.workHolidays.any((wh) =>
                  wh.holidayName == h.name &&
                  wh.holidayCountryCode == h.countryCode)))
          .toList();

      yield WorkContractCreateUpdateState(
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress,
          isCreate: false,
          workContract: workContract.value,
          evenUnevenWeeks: true,
          holidays: _holidays);
    }

    if (event is FromDateChanged)
      yield WorkContractCreateUpdateState.createOrCopy(currentState,
          changes: (workContract, _) => workContract.fromDate = event.date);
    if (event is ToDateChanged)
      yield WorkContractCreateUpdateState.createOrCopy(currentState,
          changes: (workContract, _) => workContract.toDate = event.date);
    if (event is NoteChanged)
      yield WorkContractCreateUpdateState.createOrCopy(currentState,
          changes: (workContract, _) => workContract.note = event.note);
    if (event is IsVisibleChanged)
      yield WorkContractCreateUpdateState.createOrCopy(currentState,
          changes: (workContract, _) =>
              workContract.isVisible = event.isVisible);
    if (event is WorkDayStartTimeChanged)
      yield WorkContractCreateUpdateState.createOrCopy(currentState,
          changes: (workContract, _) =>
              workContract.workDays[event.index].startTimeMins = event.mins);
    if (event is WorkDayEndTimeChanged)
      yield WorkContractCreateUpdateState.createOrCopy(currentState,
          changes: (workContract, _) =>
              workContract.workDays[event.index].endTimeMins = event.mins);
    if (event is HolidayChanged)
      yield WorkContractCreateUpdateState.createOrCopy(currentState,
          changes: (_, holidays) {
        var index = holidays.indexWhere(
          (h) =>
              h.first.countryCode == event.holiday.countryCode &&
              h.first.name == event.holiday.name,
        );
        holidays[index].second = event.include;
      });

    if (event is EvenUnevenWeeksChanged) {
      yield WorkContractCreateUpdateState.createOrCopy(
        currentState,
        evenUnevenWeeks: event.evenUnevenWeeks,
        changes: (workContract, _) => workContract.workDays = List.generate(
          event.evenUnevenWeeks ? 14 : 7,
          (i) => WorkDay(
            index: i,
            breakMins: 30,
            dayOfWeek: i % 7,
            isEvenWeek: event.evenUnevenWeeks ? i < 7 ? 1 : 2 : 0,
            startTimeMins: 9 * 60,
            endTimeMins: 17 * 60,
          ),
        ),
      );
    }

    if (event is Commit) {
      var newState = WorkContractCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      QueryResult result;

      newState.workContract.workHolidays = newState.holidays
          .fold<List<WorkHoliday>>(
              [],
              (acc, tup) => tup.second
                  ? (acc
                    ..add(WorkHoliday(
                        holidayName: tup.first.name,
                        holidayCountryCode: tup.first.countryCode)))
                  : acc);

      newState.workContract.workDays = newState.workContract.workDays
          .where((w) =>
              w.startTimeMins != null &&
              w.endTimeMins != null &&
              (w.startTimeMins + w.endTimeMins > 0))
          .toList();

      if (newState.isCreate) {
        result = await _workContractRepository.createWorkContract(
            newState.workContract, this.projectItemId);
        dispatchQueryResult(result, sprog().createAttempted);
      } else {
        result = await _workContractRepository
            .updateWorkContract(newState.workContract);
        dispatchQueryResult(result, sprog().updateAttempted);
      }

      if (result.successful) {
        yield WorkContractCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield WorkContractCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
