import 'package:bms_dart/src/blocs/location_create_update/location_create_update_errors.dart';
import 'package:bms_dart/src/models/accident_report.dart';
import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/location.dart';
import '../../models/login.dart';
import '../create_update_state_phase.dart';

//enum CreateUpdateStatePhase { Initial, InProgress, Loading, Successful, Failed }

@immutable
class LocationCreateUpdateState extends Equatable {
  final Location location;
  final LocationCreateUpdateErrors errors;
  final bool isCreate;
  final bool isValid;
  final CreateUpdateStatePhase createUpdateStatePhase;

  LocationCreateUpdateState({
    @required this.errors,
    @required this.location,
    @required this.isCreate,
    @required this.isValid,
    @required this.createUpdateStatePhase,
  }) : super([
          location.toMap(),
          isValid,
          createUpdateStatePhase,
        ]);

  factory LocationCreateUpdateState.createOrCopy(
    dynamic old, {
    Location location,
    bool isCreate,
    bool isValid,
    CreateUpdateStatePhase createUpdateStatePhase,
    Function(Location) locationChanges,
  }) {
    LocationCreateUpdateState previous;
    if (old is LocationCreateUpdateState) previous = old;

    var _location = location ?? previous?.location ?? Location();
    var _isCreate = isCreate ?? previous?.isCreate ?? false;
    var _isValid = isValid ?? previous?.isValid ?? false;
    var _createUpdateStatePhase = (createUpdateStatePhase ??
            (locationChanges != null
                ? CreateUpdateStatePhase.InProgress
                : null)) ??
        previous?.createUpdateStatePhase ??
        CreateUpdateStatePhase.Initial;
    var _errors = previous?.errors ?? LocationCreateUpdateErrors();

    if (locationChanges != null) locationChanges(_location);

    _errors.locationLoginUpdated(_location);

    _isValid = _errors.isValid(_isCreate);

    return LocationCreateUpdateState(
      errors: _errors,
      location: _location,
      isCreate: _isCreate,
      isValid: _isValid,
      createUpdateStatePhase: _createUpdateStatePhase,
    );
  }

  @override
  String toString() =>
      'LocationCreateUpdateState { location: ${location.toMap()}, createUpdateStatePhase: ${createUpdateStatePhase.toString()} }';
}
