import 'package:flutter/material.dart';
import 'customer_bloc.dart';
export 'customer_bloc.dart';

class CustomerProvider extends InheritedWidget {
  final CustomerBloc customerBloc = CustomerBloc();

  CustomerProvider({Key key, Widget child}) : super(key: key, child: child);

  bool updateShouldNotify(_) => true;

  static CustomerBloc of(BuildContext context) =>
      (context.inheritFromWidgetOfExactType(CustomerProvider)
              as CustomerProvider)
          ?.customerBloc;
}
