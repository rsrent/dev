import 'package:equatable/equatable.dart';
import 'package:meta/meta.dart';
import '../../models/noti.dart';

@immutable
abstract class NotiListEvent extends Equatable {
  NotiListEvent([List props = const []]) : super(props);
}

class NotiListFetchLatest extends NotiListEvent {
  final int count;
  NotiListFetchLatest({@required this.count}) : super([count]);
  @override
  String toString() => 'NotiListFetchLatest';
}

class NotiListFetched extends NotiListEvent {
  final List<Noti> conversations;

  NotiListFetched({@required this.conversations}) : super([conversations]);
  @override
  String toString() =>
      'NotiListFetched { conversations: ${conversations.length} }';
}

class NotiSeen extends NotiListEvent {
  final Noti noti;
  NotiSeen({@required this.noti}) : super([noti]);
  @override
  String toString() => 'NotiSeen { noti: $noti }';
}
