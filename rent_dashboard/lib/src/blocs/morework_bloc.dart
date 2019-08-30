import 'package:rxdart/rxdart.dart';
import '../models/morework.dart';
import '../resources/morework_repository.dart';

class MoreWorkBloc extends MoreWorkSource {
  final _moreworkRepository = MoreWorkRepository();
  final _moreworkSubject = BehaviorSubject<List<MoreWork>>();
  get moreworks => _moreworkSubject.stream;
  
  fetchMoreWorks() async {
    final moreworks = await _moreworkRepository.fetchMoreWorks();
    _moreworkSubject.sink.add(moreworks);
  }

  dispose()
  {
    _moreworkSubject.close();
  }
}