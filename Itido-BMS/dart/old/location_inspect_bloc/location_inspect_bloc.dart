import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/repositories.dart';
import '../../../models.dart';
import './bloc.dart';

class LocationInspectBloc
    extends Bloc<LocationInspectEvent, LocationInspectState> {
  final LocationRepository _locationRepository =
      repositoryProvider.locationRepository();

  final locationId;

  LocationInspectBloc(this.locationId);

  @override
  LocationInspectState get initialState => InitialLocationInspectState();

  @override
  Stream<LocationInspectState> mapEventToState(
    LocationInspectEvent event,
  ) async* {
    if (event is LocationInspectEventFetch) {
      var oldState = currentState;
      var loc =
          oldState is LoadedLocationInspectState ? oldState.location : null;

      yield LoadedLocationInspectState(location: loc, loading: true);
      _locationRepository.fetch(locationId).then((location) {
        dispatch(LocationInspectEventLoaded(location: location));
      });
    }

    if (event is LocationInspectEventLoaded) {
      if (event.location != null) {
        yield LoadedLocationInspectState(location: event.location);
      } else {
        yield ErrorLocationInspectState();
      }
    }

    if (event is EnableDisableLocation) {
      var oldState = currentState;
      if (oldState is LoadedLocationInspectState) {
        yield LoadedLocationInspectState(
            location: oldState.location, loading: true);
        if (oldState.location.disabled) {
          _locationRepository.enable(locationId).then((success) {
            dispatch(LocationInspectEventFetch());
          });
        } else {
          _locationRepository.disable(locationId).then((success) {
            dispatch(LocationInspectEventFetch());
          });
        }
      }
    }
  }
}
