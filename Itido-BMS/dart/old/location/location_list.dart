import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/src/widgets/selectable_circular_avatar.dart';
import 'package:bms_flutter/translations.dart';
import 'package:bms_flutter/widgets.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/location_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class LocationList extends StatelessWidget {
  final Function(Location) onSelect;
  final Function(Location) onLongPress;

  const LocationList({
    Key key,
    this.onSelect,
    this.onLongPress,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final locationListBloc = BlocProvider.of<LocationListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: locationListBloc,
      builder: (context, ListState<Location> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<Location>) {
          if (state.items.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoLocations);
          }
          return ListView.separated(
            padding: EdgeInsets.only(top: 20, bottom: 200),
            itemBuilder: (BuildContext context, int index) {
              var location = state.items[index];
              return LocationTile(
                location: location,
                onSelect: onSelect,
                onLongPress: onLongPress,
                selectable: state.selectable,
                selected: locationListBloc.isSelected(location),
              );
            },
            itemCount: state.items.length,
            separatorBuilder: (BuildContext context, int index) => Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16),
              child: Divider(height: 2),
            ),
          );
        }
      },
    );
  }
}

class LocationTile extends StatelessWidget {
  final Location location;
  final bool selectable;
  final bool selected;
  final Function(Location) onSelect;
  final Function(Location) onLongPress;

  const LocationTile({
    Key key,
    @required this.location,
    this.onSelect,
    this.onLongPress,
    this.selectable,
    this.selected,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      leading: SelectableCircularAvatar(
        name: location.name,
        selectable: selectable,
        selected: selected,
      ),
      title: Text(location.displayName),
      onTap: onSelect != null ? () => onSelect(location) : null,
      onLongPress: onLongPress != null ? () => onLongPress(location) : null,
    );
  }
}
