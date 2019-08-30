import 'package:rxdart/rxdart.dart';
import 'dart:async';

typedef void TransformerFunction<T>(T val, EventSink<T> sink);

class Streamer<T> {
  Function(T) onUpdate;
  Function(T, T) onUpdateWithOld;

  StreamTransformer streamTransformer;
  TransformerFunction<T> transformerFunction;

  Stream source;

  Streamer({
    T seedValue,
    this.onUpdate,
    this.onUpdateWithOld,
    this.streamTransformer,
    this.transformerFunction,
    this.source,
  }) {
    if (seedValue != null)
      _streamController = BehaviorSubject<T>.seeded(seedValue);
    else
      _streamController = BehaviorSubject<T>();

    if (source != null) {
      source.pipe(_streamController);
    }

    if (onUpdate == null) {
      onUpdate = (v) {};
    }
    if (onUpdateWithOld == null) {
      onUpdateWithOld = (v, o) {};
    }

    if (transformerFunction != null) {
      streamTransformer =
          StreamTransformer<T, T>.fromHandlers(handleData: (val, sink) {
        transformerFunction(val, sink);
      });
    }
  }

  BehaviorSubject<T> _streamController;
  Observable<T> get stream => streamTransformer != null
      ? _streamController.stream.transform(streamTransformer)
      : _streamController.stream;
  Function(T) get update => (v) {
        onUpdate(v);
        var old = value;
        _streamController.sink.add(v);
        onUpdateWithOld(v, old);
      };
  Function(T) get addError => (v) {
        _streamController.sink.addError(v);
      };
  T get value => _streamController.value;

  dispose() {
    _streamController.close();
  }

  close() {
    _streamController.close();
  }

  bool isOpen() => !_streamController.isClosed;

  void notifyListeners() => update(value);
}

Stream<List<T>> getStreamFromStreams<T>(List<Stream<List<T>>> souces) {
  if (souces.length == 0) {
    return Stream.fromIterable([List<T>()]);
  }
  if (souces.length == 1) {
    return souces[0].asBroadcastStream();
  }
  return Observable.combineLatestList(souces).transform<List<T>>(
      StreamTransformer.fromHandlers(handleData: (sss, sink) {
    List<T> finalList = [];
    sss.forEach((ss) {
      finalList.addAll(ss);
    });
    sink.add(finalList);
  })).asBroadcastStream();
}
