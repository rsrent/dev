import 'package:rxdart/rxdart.dart';
import '../models/data.dart';
import '../models/news.dart';
import '../resources/data_repository.dart';

class DataBloc {
  final _dataRepository = DataRepository();
  final _dataSubject = BehaviorSubject<CompleteData>();
  final _newsSubject = BehaviorSubject<List<News>>();
  final _newNewsSubject = PublishSubject<bool>();

  Observable<CompleteData> get data => _dataSubject.stream;
  Observable<List<News>> get news => _newsSubject.stream;
  Observable<bool> get newNews => _newNewsSubject.stream;

  //final DataBlocScope scope;
  final int customerId;
  final int userId;
  final int locationId;
  DataBloc({this.customerId: 0, this.userId: 0, this.locationId: 0});

  int latestId = 0;

  fetchData() async {
    final data =
        await _dataRepository.fetchData(customerId, userId, locationId);
    data.customerId = customerId;
    data.userId = userId;
    _dataSubject.sink.add(data);
  }

  fetchNews() async {
    final news =
        await _dataRepository.fetchNews(customerId, userId, locationId);

    if (news.length > 0) {
      if (latestId != news.first.id) {
        print("New news!!");
        _newNewsSubject.sink.add(true);
      }
      latestId = news.first.id;
    }

    _newsSubject.sink.add(news);
  }

  latestNewsSeen() {
    _newNewsSubject.sink.add(false);
  }

  dispose() {
    _dataSubject.close();
    _newsSubject.close();
    _newNewsSubject.close();
  }
}

enum DataBlocScope { Show, Hide }
