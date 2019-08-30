import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/src/models/agreement.dart';
import 'package:dart_packages/streamer.dart';
import './bloc.dart';

class AgreementCreateUpdateBloc
    extends Bloc<AgreementCreateUpdateEvent, AgreementCreateUpdateState> {
  final AgreementRepository _agreementRepository =
      repositoryProvider.agreementRepository();

  Agreement _toUpdate;

  Streamer<String> _name;
  Streamer<String> get name => _name;

  AgreementCreateUpdateBloc() {
    _name = Streamer();
  }

  @override
  AgreementCreateUpdateState get initialState => Initial();

  @override
  Stream<AgreementCreateUpdateState> mapEventToState(
    AgreementCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      _name.update(_toUpdate.name);
      yield PreparingCreate();
    }

    if (event is PrepareUpdate) {
      _toUpdate = event.agreement;
      _name.update(_toUpdate.name);
      yield PreparingUpdate(agreement: event.agreement);
    }

    if (event is CreateRequested) {
      yield Loading();
      var result = await _agreementRepository.createAgreement(Agreement(
        name: name.value,
      ));
      if (result != null) {
        yield CreateSuccessful(id: result);
      } else {
        yield CreateFailure();
      }
    }
    if (event is UpdateRequested) {
      yield Loading();
      var result = await _agreementRepository.updateAgreement(Agreement(
        id: _toUpdate.id,
        name: name.value,
      ));
      if (result != null) {
        yield UpdateSuccessful();
      } else {
        yield UpdateFailure();
      }
    }
  }
}
