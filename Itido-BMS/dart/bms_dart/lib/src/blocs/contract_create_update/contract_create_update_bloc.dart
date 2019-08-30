import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/repositories.dart';
import 'package:dart_packages/streamer.dart';
import 'package:flutter/foundation.dart';
import './bloc.dart';
import 'package:rxdart/rxdart.dart';

class ContractCreateUpdateBloc
    extends Bloc<ContractCreateUpdateEvent, ContractCreateUpdateState> {
  final AgreementRepository _agreementRepository =
      repositoryProvider.agreementRepository();
  final ContractRepository _contractRepository =
      repositoryProvider.contractRepository();

  int _userId;
  // Contract _toUpdate;

  // Streamer<Agreement> _selectedAgreement;
  // Streamer<Agreement> get selectedAgreement => _selectedAgreement;
  // Streamer<List<Agreement>> _allAgreements;
  // Streamer<List<Agreement>> get allAgreements => _allAgreements;
  // Streamer<DateTime> _from;
  // Streamer<DateTime> get from => _from;
  // Streamer<DateTime> _to;
  // Streamer<DateTime> get to => _to;
  // Streamer<String> _weeklyHours;
  // Streamer<String> get weeklyHours => _weeklyHours;
  // Streamer<bool> _formValid;
  // Streamer<bool> get formValid => _formValid;

  ContractCreateUpdateBloc({
    int userId,
  }) {
    this._userId = userId;
    // this._selectedAgreement = Streamer();
    // this._allAgreements = Streamer();
    // this._from = Streamer(seedValue: DateTime.now());
    // this._to = Streamer(seedValue: DateTime.now());
    // this._weeklyHours = Streamer(streamTransformer: transformer);
    // this._formValid = Streamer(
    //   seedValue: false,
    //   source:
    //       Observable.combineLatest4<dynamic, DateTime, DateTime, String, bool>(
    //               selectedAgreement.stream,
    //               from.stream,
    //               to.stream,
    //               weeklyHours.stream,
    //               (u, p, o, w) => (u != null && w.length > 0))
    //           .transform(transformerTest),
    // );
  }

  @override
  ContractCreateUpdateState get initialState =>
      ContractCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<ContractCreateUpdateState> mapEventToState(
    ContractCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      var loadedAgreements = await _agreementRepository.fetchAllAgreements();
      yield ContractCreateUpdateState.createOrCopy(
        null,
        contract: Contract(
          from: DateTime.now(),
          to: DateTime.now().add(Duration(days: 30)),
          weeklyHours: 20,
        ),
        allAgreements: loadedAgreements,
        selectedAgreement: null,
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: true,
      );
      yield ContractCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);

      // _allAgreements.update(loadedAgreements);
      // _selectedAgreement.update(null);
      // _from.update(DateTime.now());
      // _to.update(DateTime.now().add(Duration(days: 30)));
      // _weeklyHours.update('');
      // yield PreparingCreate();
    }

    if (event is PrepareUpdate) {
      var loadedAgreements = await _agreementRepository.fetchAllAgreements();
      var contract = await _contractRepository.fetchContract(event.contract.id);
      yield ContractCreateUpdateState.createOrCopy(
        null,
        contract: contract,
        allAgreements: loadedAgreements,
        selectedAgreement: loadedAgreements.firstWhere(
            (a) => a.id == event.contract.agreement.id,
            orElse: () => null),
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: false,
      );
      yield ContractCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    // if (event is PrepareUpdate) {
    //   _toUpdate = event.contract;
    //   var loadedAgreements = await _agreementRepository.fetchAllAgreements();
    //   _allAgreements.update(loadedAgreements);
    //   _selectedAgreement.update(loadedAgreements.firstWhere(
    //       (a) => a.id == event.contract.agreement.id,
    //       orElse: () => null));
    //   _from.update(event.contract.from);
    //   _to.update(event.contract.to);
    //   _weeklyHours.update('${event.contract.weeklyHours}');
    //   yield PreparingUpdate(contract: event.contract);
    // }

    if (event is AgreementChanged)
      yield ContractCreateUpdateState.createOrCopy(currentState,
          selectedAgreement: event.agreement);
    if (event is FromChanged)
      yield ContractCreateUpdateState.createOrCopy(currentState,
          contractChanges: (contract) => contract.from = event.dateTime);
    if (event is ToChanged)
      yield ContractCreateUpdateState.createOrCopy(currentState,
          contractChanges: (contract) => contract.to = event.dateTime);
    if (event is WeeklyHoursChanged)
      yield ContractCreateUpdateState.createOrCopy(currentState,
          contractChanges: (contract) => contract.weeklyHours =
              event.text != '' ? double.parse(event.text) : null);

    if (event is Commit) {
      var newState = ContractCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result;
      if (newState.isCreate) {
        result = (await _contractRepository.createContract(
                newState.contract, _userId, newState.selectedAgreement.id)) !=
            null;
      } else {
        result = await _contractRepository.updateContract(
          newState.contract,
        );
      }
      if (result) {
        yield ContractCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield ContractCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }

    // if (event is CreateRequested) {
    //   yield Loading();
    //   var result = await _contractRepository.createContract(
    //       Contract(
    //         from: _from.value,
    //         to: _to.value,
    //         weeklyHours: double.parse(_weeklyHours.value),
    //         //agreementName: _selectedAgreement.value.name,
    //       ),
    //       _userId,
    //       _selectedAgreement.value.id);
    //   if (result != null) {
    //     yield CreateSuccessful();
    //   } else {
    //     yield CreateFailure();
    //   }
    // }
    // if (event is UpdateRequested) {
    //   yield Loading();
    //   var result = await _contractRepository.updateContract(Contract(
    //     from: _from.value,
    //     to: _to.value,
    //     weeklyHours: double.parse(_weeklyHours.value),
    //   ));
    //   if (result != null) {
    //     yield UpdateSuccessful();
    //   } else {
    //     yield UpdateFailure();
    //   }
    // }
  }

  final StreamTransformer transformer =
      StreamTransformer<String, String>.fromHandlers(handleData: (val, sink) {
    if (val.length == 0 || double.tryParse(val) != null) {
      sink.add(val);
    } else {
      sink.addError('Ikke et korrekt antal timer');
    }
  });

  final StreamTransformer transformerTest =
      StreamTransformer<bool, bool>.fromHandlers(
          handleError: (error, stackTrace, sink) {
    sink.add(false);
  });
}
