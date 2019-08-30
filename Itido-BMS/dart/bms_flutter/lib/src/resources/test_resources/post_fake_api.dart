import 'package:bms_dart/models.dart';
import 'dart:async';
import 'package:bms_dart/repositories.dart';
import 'client_faker.dart';

class PostFakeApi extends PostSource {
  ClientFaker _faker = ClientFaker<Post>(
      generator: (i) => Post(
            title: 'Besked nr $i',
            body:
                'Her er den egentlige besked så nu. Hvad der skal stå ved jeg ikke....',
            sendTime: DateTime.now(),
          )..sender = User(firstName: 'Tobias Bang (600)'));

  @override
  void dispose() {}

  @override
  Future<int> createPost(Post post) {
    return _faker.add(post);
  }

  @override
  Future<List<Post>> fetchLatestPosts(int count) => _faker.getMany();
}
