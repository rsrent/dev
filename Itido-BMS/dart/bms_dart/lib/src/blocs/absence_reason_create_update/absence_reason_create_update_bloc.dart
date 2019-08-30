import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/repositories.dart';
import 'package:bms_dart/src/models/absence_reason.dart';
import 'package:dart_packages/streamer.dart';
import './bloc.dart';
import 'package:rxdart/rxdart.dart';

class AbsenceReasonCreateUpdateBloc extends Bloc<AbsenceReasonCreateUpdateEvent,
    AbsenceReasonCreateUpdateState> {
  final AbsenceReasonRepository _absenceReasonRepository =
      repositoryProvider.absenceReasonRepository();

  AbsenceReason _toUpdate;

  Streamer<String> _description;
  Streamer<String> get description => _description;

  Streamer<bool> _canUserRequest;
  Streamer<bool> get canUserRequest => _canUserRequest;
  Streamer<bool> _canUserCreate;
  Streamer<bool> get canUserCreate => _canUserCreate;
  Streamer<bool> _canManagerRequest;
  Streamer<bool> get canManagerRequest => _canManagerRequest;
  Streamer<bool> _canManagerCreate;
  Streamer<bool> get canManagerCreate => _canManagerCreate;

  Streamer<bool> _formValid;
  Streamer<bool> get formValid => _formValid;

  AbsenceReasonCreateUpdateBloc() {
    _description = Streamer();
    _canUserRequest = Streamer();
    _canUserCreate = Streamer();
    _canManagerRequest = Streamer();
    _canManagerCreate = Streamer();

    this._formValid = Streamer(
      seedValue: false,
      source: Observable.combineLatest5<String, bool, bool, bool, bool, bool>(
          _description.stream,
          _canUserRequest.stream,
          _canUserCreate.stream,
          _canManagerRequest.stream,
          _canManagerCreate.stream,
          (d, c, j, u, a) => (d.length > 0)),
    );
  }

  @override
  AbsenceReasonCreateUpdateState get initialState => Initial();

  @override
  Stream<AbsenceReasonCreateUpdateState> mapEventToState(
    AbsenceReasonCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      _description.update('');
      _canUserRequest.update(false);
      _canUserCreate.update(false);
      _canManagerRequest.update(false);
      _canManagerCreate.update(false);
      yield PreparingCreate();
    }

    if (event is PrepareUpdate) {
      _toUpdate = event.absenceReason;
      _description.update(event.absenceReason.description);
      _canUserRequest.update(_toUpdate.canUserRequest);
      _canUserCreate.update(_toUpdate.canUserCreate);
      _canManagerRequest.update(_toUpdate.canManagerRequest);
      _canManagerCreate.update(_toUpdate.canManagerCreate);
      yield PreparingUpdate(absenceReason: event.absenceReason);
    }

    if (event is CreateRequested) {
      yield Loading();
      var result =
          await _absenceReasonRepository.createAbsenceReason(AbsenceReason(
        description: description.value,
        canUserRequest: _canUserRequest.value,
        canUserCreate: _canUserCreate.value,
        canManagerRequest: _canManagerRequest.value,
        canManagerCreate: _canManagerCreate.value,
      ));
      if (result != null) {
        yield CreateSuccessful(id: result);
      } else {
        yield CreateFailure();
      }
    }
    if (event is UpdateRequested) {
      yield Loading();
      var result =
          await _absenceReasonRepository.updateAbsenceReason(AbsenceReason(
        id: _toUpdate.id,
        description: description.value,
        canUserRequest: _canUserRequest.value,
        canUserCreate: _canUserCreate.value,
        canManagerRequest: _canManagerRequest.value,
        canManagerCreate: _canManagerCreate.value,
      ));
      if (result != null) {
        yield UpdateSuccessful();
      } else {
        yield UpdateFailure();
      }
    }
  }
}
