import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/src/widgets/selectable_circular_avatar.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/customer_list_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class CustomerList extends StatelessWidget {
  final Function(Customer) onSelect;
  final Function(Customer) onLongPress;
  final EdgeInsets padding;
  const CustomerList({
    Key key,
    this.onSelect,
    this.onLongPress,
    this.padding,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final customerListBloc = BlocProvider.of<CustomerListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: customerListBloc,
      builder: (context, ListState<Customer> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded<Customer>) {
          if (state.items.isEmpty) {
            return InfoListView(info: Translations.of(context).infoNoCustomers);
          }
          return ListView.separated(
            padding: padding ?? EdgeInsets.only(top: 20, bottom: 200),
            itemBuilder: (BuildContext context, int index) {
              var customer = state.items[index];
              return CustomerTile(
                customer: customer,
                onSelect: onSelect,
                onLongPress: onLongPress,
                selectable: state.selectable,
                selected:
                    state.selectable && state.selectedItems.contains(customer),
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

class CustomerTile extends StatelessWidget {
  final Customer customer;
  final bool selectable;
  final bool selected;
  final Function(Customer) onSelect;
  final Function(Customer) onLongPress;

  const CustomerTile({
    Key key,
    @required this.customer,
    this.onSelect,
    this.selectable,
    this.selected,
    this.onLongPress,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ListTile(
      leading: SelectableCircularAvatar(
        name: customer.displayName,
        selectable: selectable,
        selected: selected,
      ),
      title: Text(customer.displayName),
      onTap: onSelect != null ? () => onSelect(customer) : null,
      onLongPress: onLongPress != null ? () => onLongPress(customer) : null,
    );
  }
}
