import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:bms_dart/models.dart';
import '../refreshable.dart';
import './bloc.dart';
import 'package:bms_dart/repositories.dart';

class ContractListBloc extends Bloc<ContractListEvent, ListState<Contract>>
    with Refreshable {
  final ContractRepository _contractRepository =
      repositoryProvider.contractRepository();

  ContractListBloc(this._refreshEvent) {
    refresh();
  }

  @override
  ListState<Contract> get initialState => Loading<Contract>();

  @override
  Stream<ListState<Contract>> mapEventToState(
    ContractListEvent event,
  ) async* {
    if (event is ContractListFetchOfUser) {
      // yield* mapEventToRefreshing(event);
      final items =
          await _contractRepository.fetchContractsOfUser(event.userId);
      if (items != null)
        yield Loaded(items: items, refreshTime: DateTime.now());
      else
        yield Failure();
    }
  }

  // Stream<ListState<Contract>> mapEventToRefreshing(
  //     ContractListEvent event) async* {
  //   if (currentState is Loaded) {
  //     yield Refreshing(items: (currentState as Loaded).items);
  //   }
  // }

  final ContractListEvent Function() _refreshEvent;

  @override
  void refresh() => dispatch(_refreshEvent());
}
