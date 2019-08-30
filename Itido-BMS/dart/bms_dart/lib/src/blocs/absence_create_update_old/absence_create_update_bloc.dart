import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:dart_packages/streamer.dart';
import 'package:flutter/foundation.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import './bloc.dart';
import 'package:rxdart/rxdart.dart';
import 'package:dart_packages/date_time_operations.dart' as dtOps;

class AbsenceCreateUpdateBloc
    extends Bloc<AbsenceCreateUpdateEvent, AbsenceCreateUpdateState> {
  final AbsenceReasonRepository _absenceReasonRepository =
      repositoryProvider.absenceReasonRepository();
  final AbsenceRepository _absenceRepository =
      repositoryProvider.absenceRepository();
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();

  int _userId;
  Absence _toUpdate;

  Streamer<AbsenceReason> _selectedAbsenceReason;
  Streamer<AbsenceReason> get selectedAbsenceReason => _selectedAbsenceReason;
  Streamer<List<AbsenceReason>> _allAbsenceReasons;
  Streamer<List<AbsenceReason>> get allAbsenceReasons => _allAbsenceReasons;
  Streamer<DateTime> _from;
  Streamer<DateTime> get from => _from;
  Streamer<DateTime> _to;
  Streamer<DateTime> get to => _to;
  Streamer<String> _comment;
  Streamer<String> get comment => _comment;

  Streamer<bool> _formValid;
  Streamer<bool> get formValid => _formValid;

  Streamer<String> _userRole;
  Streamer<bool> _canUserCreate;
  Streamer<bool> get canUserCreate => _canUserCreate;
  Streamer<bool> _canUserRequest;
  Streamer<bool> get canUserRequest => _canUserRequest;

  AbsenceCreateUpdateBloc({
    int userId,
  }) {
    this._userId = userId;
    this._selectedAbsenceReason = Streamer();
    this._allAbsenceReasons = Streamer();
    this._from = Streamer(seedValue: dtOps.toDate(DateTime.now()));
    this._to = Streamer(seedValue: dtOps.toDate(DateTime.now()));
    this._comment = Streamer();
    this._formValid = Streamer(
      seedValue: false,
      source: Observable.combineLatest3<dynamic, DateTime, DateTime, bool>(
          selectedAbsenceReason.stream,
          from.stream,
          to.stream,
          (u, p, o) => (u != null)).transform(transformerTest),
    );

    this._userRole = Streamer(seedValue: '');

    this._canUserCreate = Streamer(
        seedValue: false,
        source: Observable.combineLatest2<String, AbsenceReason, bool>(
            _userRole.stream, _selectedAbsenceReason.stream, (role, reason) {
          var t = reason != null &&
              (role == 'User' && reason.canUserCreate ||
                  (role == 'Manager' || role == 'Admin') &&
                      reason.canManagerCreate);
          print('userRole $role');
          print('reason ${reason?.toMap()}');
          return t;
        }));
    this._canUserRequest = Streamer(
        seedValue: false,
        source: Observable.combineLatest2<String, AbsenceReason, bool>(
            _userRole.stream,
            _selectedAbsenceReason.stream,
            (role, reason) =>
                reason != null &&
                (role == 'User' && reason.canUserRequest ||
                    (role == 'Manager' || role == 'Admin') &&
                        reason.canManagerRequest)));
  }

  @override
  AbsenceCreateUpdateState get initialState => Initial();

  @override
  Stream<AbsenceCreateUpdateState> mapEventToState(
    AbsenceCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      var loadedAbsenceReasons =
          await _absenceReasonRepository.fetchAllAbsenceReasons();
      _allAbsenceReasons.update(loadedAbsenceReasons);
      _userRole.update(_authenticationRepository.getUserRole());
      _selectedAbsenceReason.update(null);
      _from.update(dtOps.toDate(DateTime.now()));
      _to.update(dtOps.toDate(DateTime.now().add(Duration(days: 30))));
      _comment.update('');
      yield PreparingCreate();
    }

    if (event is PrepareUpdate) {
      _toUpdate = event.absence;
      var loadedAbsenceReasons =
          await _absenceReasonRepository.fetchAllAbsenceReasons();
      _allAbsenceReasons.update(loadedAbsenceReasons);
      _userRole.update(_authenticationRepository.getUserRole());
      _selectedAbsenceReason.update(loadedAbsenceReasons.firstWhere(
          (a) => a.id == event.absence.absenceReason.id,
          orElse: () => null));
      _from.update(event.absence.from);
      _to.update(event.absence.to);
      _comment.update(event.absence.comment);
      yield PreparingUpdate(absence: event.absence);
    }

    if (event is CreateRequested) {
      yield Loading();

      var isRequest = event.asRequest;

      print('Hallo');

      var result = await _absenceRepository.createAbsence(
          Absence(
            from: _from.value,
            to: _to.value,
            comment: _comment.value,
          ),
          _userId,
          _selectedAbsenceReason.value.id,
          isRequest);
      if (result != null) {
        yield CreateSuccessful();
      } else {
        yield CreateFailure();
      }
    }
    if (event is UpdateRequested) {
      yield Loading();
      var result = await _absenceRepository.updateAbsence(Absence(
        id: _toUpdate.id,
        from: dtOps.toDate(_from.value),
        to: dtOps.toDate(_to.value),
        comment: _comment.value,
      ));
      if (result != null) {
        yield UpdateSuccessful();
      } else {
        yield UpdateFailure();
      }
    }
  }

  final StreamTransformer transformerTest =
      StreamTransformer<bool, bool>.fromHandlers(
          handleError: (error, stackTrace, sink) {
    sink.add(false);
  });
}
