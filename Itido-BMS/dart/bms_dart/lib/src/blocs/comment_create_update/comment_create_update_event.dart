import 'package:bms_dart/src/models/comment.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:meta/meta.dart';

import '../../models/accident_report.dart';

@immutable
abstract class CommentCreateUpdateEvent extends Equatable {
  CommentCreateUpdateEvent([List props = const []]) : super(props);
}

class PrepareUpdate extends CommentCreateUpdateEvent {
  final Comment comment;
  PrepareUpdate({@required this.comment}) : super([comment]);
  @override
  String toString() => 'PrepareUpdate { comment: $comment }';
}

class TitleChanged extends CommentCreateUpdateEvent {
  final String text;
  TitleChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'TitleChanged { text: $text }';
}

class BodyChanged extends CommentCreateUpdateEvent {
  final String text;
  BodyChanged({@required this.text}) : super([text]);
  @override
  String toString() => 'BodyChanged { text: $text }';
}

class Commit extends CommentCreateUpdateEvent {
  @override
  String toString() => 'Commit';
}
