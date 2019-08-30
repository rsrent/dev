import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/repositories/agreement_repository.dart';
import '../refreshable.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class AgreementListBloc extends Bloc<AgreementListEvent, ListState<Agreement>>
    with Refreshable {
  final AgreementRepository _agreementRepository =
      repositoryProvider.agreementRepository();

  AgreementListBloc(this._refreshEvent) {
    refresh();
  }
  final AgreementListEvent Function() _refreshEvent;
  void refresh() => dispatch(_refreshEvent());

  @override
  ListState<Agreement> get initialState => Loading();

  @override
  Stream<ListState<Agreement>> mapEventToState(
    AgreementListEvent event,
  ) async* {
    if (event is AgreementListFetch) {
      final items = await _agreementRepository.fetchAllAgreements();
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }
  }
}
