import 'package:bms_dart/create_update_state_phase.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/location_create_update_bloc.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class LocationCreateUpdateScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    Location location,
    int customerId,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => LocationCreateUpdateScreen(
        locationToUpdate: location,
        customerId: customerId,
      ),
    ));
  }

  final Location locationToUpdate;
  final int customerId;
  final bool isCreate;

  const LocationCreateUpdateScreen(
      {Key key, this.locationToUpdate, this.customerId})
      : isCreate = locationToUpdate == null,
        super(key: key);

  @override
  _LocationCreateUpdateScreenState createState() =>
      _LocationCreateUpdateScreenState();
}

class _LocationCreateUpdateScreenState
    extends State<LocationCreateUpdateScreen> {
  bool updated = false;

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async => false,
      child: Scaffold(
        appBar: AppBar(
          leading: IconButton(
            icon: Icon(Icons.arrow_back),
            onPressed: () {
              Navigator.of(context).pop(updated);
            },
          ),
          title: Text(
            widget.isCreate
                ? Translations.of(context).titleCreateLocation
                : Translations.of(context).titleUpdateLocation,
          ),
        ),
        body: BlocProvider(
          builder: (context) {
            if (widget.isCreate)
              return LocationCreateUpdateBloc(customerId: widget.customerId)
                ..dispatch(PrepareCreate());
            else
              return LocationCreateUpdateBloc()
                ..dispatch(
                    PrepareUpdate(location: this.widget.locationToUpdate));
          },
          child: Builder(
            builder: (context) {
              var _bloc = BlocProvider.of<LocationCreateUpdateBloc>(context);

              return BlocListener(
                bloc: _bloc,
                listener: (context, LocationCreateUpdateState state) {
                  print('${state.createUpdateStatePhase}');
                  if (state.createUpdateStatePhase ==
                      CreateUpdateStatePhase.Failed) {
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationFailed
                            : Translations.of(context).infoUpdateFailed);
                  }
                  if (state.createUpdateStatePhase ==
                      CreateUpdateStatePhase.Successful) {
                    updated = true;
                    showSnackText(
                        context,
                        widget.isCreate
                            ? Translations.of(context).infoCreationSuccessful
                            : Translations.of(context).infoUpdateSuccessful);
                  }
                },
                child: LocationCreateUpdateForm(),
              );
            },
          ),
        ),
      ),
    );
  }
}
