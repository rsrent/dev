import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import 'package:bms_dart/src/repositories/customer_repository.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';
import '../refreshable.dart';
import '../searchable.dart';
import '../selectable.dart';

class CustomerListBloc extends Bloc<CustomerListEvent, ListState<Customer>>
    with Refreshable, Searchable, Selectable<Customer> {
  final CustomerRepository _customerRepository =
      repositoryProvider.customerRepository();

  CustomerListBloc(this._refreshEvent) {
    refresh();
  }

  @override
  ListState<Customer> get initialState => Loading();

  @override
  Stream<ListState<Customer>> mapEventToState(
    CustomerListEvent event,
  ) async* {
    if (event is CustomerListFetch) {
      _customerRepository.fetchAllCustomers().then(
          (customers) => dispatch(CustomersFetched(customers: customers)));
    }

    if (event is SearchTextUpdated) {
      searchText = event.searchText;
      dispatch(CustomersFetched(customers: loaded));
    }

    if (event is CustomersFetched) {
      loaded = event.customers;
      if (event.customers != null) {
        var items = filtered(searchText, filters);
        var oldState = currentState;
        if (oldState is Loaded<Customer>) {
          yield Loaded<Customer>(
            items: items,
            refreshTime: DateTime.now(),
            selectable: oldState.selectable,
            selectedItems: oldState.selectedItems,
          );
        } else {
          yield Loaded(items: items, refreshTime: DateTime.now());
        }
      } else {
        yield Failure();
      }
    }
  }

  @override
  List<Customer> filtered(String text, List<String> filters) {
    var _filter = text.toLowerCase();
    return loaded.where((u) => u.name.toLowerCase().contains(_filter)).toList();
  }

  @override
  List<String> allFilters() => [];

  @override
  List<String> initialFilters() => [];

  final CustomerListEvent Function() _refreshEvent;

  @override
  void refresh() => dispatch(_refreshEvent());

  @override
  void searchTextUpdated(String text) =>
      dispatch(SearchTextUpdated(searchText: text));

  @override
  void clear() {
    dispatch(ClearSelected());
  }

  @override
  void selectAll() {
    dispatch(SelectAll());
  }

  @override
  void toggleSelectable() {
    dispatch(ToggleSelectable());
  }

  @override
  bool equal(m1, m2) => m1.id == m2.id;

  @override
  bool isSelected(Customer m) {
    var state = currentState;
    if (state is Loaded<Customer>) {
      return state.selectable && state.selectedItems.any((si) => equal(si, m));
    }
    return false;
  }
}
