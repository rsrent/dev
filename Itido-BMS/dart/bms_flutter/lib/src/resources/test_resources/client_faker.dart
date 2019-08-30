import 'dart:math';

import 'package:flutter/foundation.dart';

class ClientFaker<T> {
  Future<void> delayer() => Future.delayed(Duration(milliseconds: 500));
  List<T> _collection;
  Function(T, int) updateId;
  bool Function(T, T) valueToUpdate;
  Random rnd;

  ClientFaker({
    @required T Function(int) generator,
    this.updateId,
    this.valueToUpdate,
  }) {
    rnd = Random();
    _collection = List<T>.generate(20, generator);
  }

  Future<List<T>> getMany() async {
    await delayer();
    return _collection;
  }

  Future<T> getOne(Function(T) where) async {
    await delayer();
    return _collection.firstWhere(where);
  }

  Future<int> add(T t) async {
    await delayer();
    var id = 0;
    if (updateId != null) {
      id = newId;
      updateId(t, id);
    }
    _collection.add(t);
    return id;
  }

  Future<bool> update(T t) async {
    await delayer();

    if (valueToUpdate != null) {
      _collection.removeWhere((other) => valueToUpdate(other, t));
      _collection.add(t);
    }
    return true;
  }

  int get newId => rnd.nextInt(2000000);
}
