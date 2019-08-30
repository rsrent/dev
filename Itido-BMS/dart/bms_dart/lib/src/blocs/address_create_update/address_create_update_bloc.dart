import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:flutter/material.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class AddressCreateUpdateBloc
    extends Bloc<AddressCreateUpdateEvent, AddressCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final AddressRepository _addressRepository =
      repositoryProvider.addressRepository();

  final int addressId;

  AddressCreateUpdateBloc(this.addressId);

  @override
  AddressCreateUpdateState get initialState =>
      AddressCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<AddressCreateUpdateState> mapEventToState(
    AddressCreateUpdateEvent event,
  ) async* {
    if (event is PrepareUpdate) {
      var address = await _addressRepository.fetch(addressId);
      yield AddressCreateUpdateState.createOrCopy(null,
          address: address,
          createUpdateStatePhase: CreateUpdateStatePhase.Initial);
      yield AddressCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is AddressNameChanged)
      yield AddressCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress,
          changes: (address) => address.addressName = event.text);
    if (event is PositionChanged)
      yield AddressCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress,
          changes: (address) {
        address.lat = event.lat;
        address.lon = event.lon;
      });

    if (event is Commit) {
      var newState = AddressCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result =
          await _addressRepository.update(addressId, newState.address);
      if (result) {
        yield AddressCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield AddressCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
