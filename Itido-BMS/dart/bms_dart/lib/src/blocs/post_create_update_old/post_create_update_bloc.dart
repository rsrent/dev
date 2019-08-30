import 'dart:async';
import 'package:bloc/bloc.dart';
import 'package:dart_packages/streamer.dart';
import './bloc.dart';
import '../../../models.dart';
import '../../../repositories.dart';

class PostCreateUpdateBloc
    extends Bloc<PostCreateUpdateEvent, PostCreateUpdateState> {
  final PostRepository _postRepository = repositoryProvider.postRepository();

  Streamer<String> _title = Streamer();
  Streamer<String> get title => _title;
  Streamer<String> _body = Streamer();
  Streamer<String> get body => _body;
  Streamer<List<PostCondition>> _conditions = Streamer(seedValue: [
    PostCondition(
        postConditionType: PostConditionType.HasRole,
        postConditionValue: "User")
      ..postConditionSubject = "User",
  ]);
  Streamer<List<PostCondition>> get conditions => _conditions;

  @override
  PostCreateUpdateState get initialState => Initial();

  @override
  Stream<PostCreateUpdateState> mapEventToState(
    PostCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      yield Loading();
      var locs = await _locationRepository.fetchAllLocations();
      _locations = locs;
      var custs = await _customerRepository.fetchAllCustomers();
      _customers = custs;
      yield PreparingCreate();
    }

    if (event is AddCondition) {
      _conditions.update(_conditions.value
        ..add(PostCondition(postConditionType: event.postConditionType)));
    }
    if (event is RemoveCondition) {
      _conditions.update(_conditions.value..remove(event.postCondition));
    }

    if (event is CreateRequested) {
      yield Loading();

      var post =
          Post(title: title.value, body: body.value, sendTime: DateTime.now());

      var userRoleCondition = _conditions.value.firstWhere(
          (c) => c.postConditionType == PostConditionType.HasRole,
          orElse: () => null);
      if (userRoleCondition != null) {
        post.userRole = userRoleCondition.postConditionValue;
      }
      var locationCondition = _conditions.value.firstWhere(
          (c) => c.postConditionType == PostConditionType.AtLocation,
          orElse: () => null);
      if (locationCondition != null) {
        post.locationId = int.parse(locationCondition.postConditionValue);
      }
      var customerCondition = _conditions.value.firstWhere(
          (c) =>
              c.postConditionType ==
              PostConditionType.AtAnyLocationUnderCustomer,
          orElse: () => null);
      if (customerCondition != null) {
        post.customerId = int.parse(customerCondition.postConditionValue);
      }

      var result = await _postRepository.createPost(post);
      if (result != null) {
        yield CreateSuccessful();
      } else {
        yield CreateFailure();
      }
    }
  }
}
