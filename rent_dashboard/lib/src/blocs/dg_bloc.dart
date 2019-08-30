
import 'package:rxdart/rxdart.dart';
import '../resources/dg_repository.dart';

class DgBloc {
  final _dgRepository = DgRepository();
  final _dgSubject = BehaviorSubject<double>();
  get dg => _dgSubject.stream;

  int locationId;
  int customerId;
  int userId;
  DgBloc({this.locationId: 0, this.customerId: 0, this.userId: 0});

  fetch() async {
    final dgs = await _dgRepository.fetch(locationId, customerId, userId);
    _dgSubject.sink.add(dgs);
  }

  dispose() {
    _dgSubject.close();
  }
}
