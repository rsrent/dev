import 'dart:async';
import 'package:bloc/bloc.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class LocationCreateUpdateBloc
    extends Bloc<LocationCreateUpdateEvent, LocationCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final LocationRepository _locationRepository =
      repositoryProvider.locationRepository();

  final int customerId;

  LocationCreateUpdateBloc({this.customerId});

  @override
  LocationCreateUpdateState get initialState =>
      LocationCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<LocationCreateUpdateState> mapEventToState(
    LocationCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      yield LocationCreateUpdateState.createOrCopy(
        null,
        location: Location(
          disabled: false,
        ),
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: true,
      );
      yield LocationCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }
    if (event is PrepareUpdate) {
      var location = await _locationRepository.fetch(event.location.id);
      yield LocationCreateUpdateState.createOrCopy(
        null,
        location: location,
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: false,
      );
      yield LocationCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is NameChanged)
      yield LocationCreateUpdateState.createOrCopy(currentState,
          locationChanges: (location) => location.name = event.text);
    if (event is AddressChanged)
      yield LocationCreateUpdateState.createOrCopy(currentState,
          locationChanges: (location) => location.address = event.text);
    if (event is CommentChanged)
      yield LocationCreateUpdateState.createOrCopy(currentState,
          locationChanges: (location) => location.comment = event.text);
    if (event is PhoneChanged)
      yield LocationCreateUpdateState.createOrCopy(currentState,
          locationChanges: (location) => location.phone = event.text);
    if (event is CommentChanged)
      yield LocationCreateUpdateState.createOrCopy(currentState,
          locationChanges: (location) => location.comment = event.text);
    if (event is ProjectNumberChanged)
      yield LocationCreateUpdateState.createOrCopy(currentState,
          locationChanges: (location) => location.projectNumber =
              event.text != '' ? int.parse(event.text) : null);

    if (event is Commit) {
      var newState = LocationCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result;
      if (newState.isCreate) {
        result =
            (await _locationRepository.create(newState.location, customerId)) !=
                null;
      } else {
        result = await _locationRepository.update(
            newState.location, newState.location.id);
      }
      print('reslt: $result');
      if (result) {
        yield LocationCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield LocationCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
