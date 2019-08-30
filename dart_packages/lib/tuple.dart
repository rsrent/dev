class Tuple2<A, B> {
  A first;
  B second;

  Tuple2(this.first, this.second);

  @override
  String toString() => '($first, $second)';
}

class Tuple3<A, B, C> {
  A first;
  B second;
  C third;

  Tuple3(this.first, this.second, this.third);

  @override
  String toString() => '($first, $second, $third)';
}

class Tuple4<A, B, C, D> {
  A first;
  B second;
  C third;
  D fourth;

  Tuple4(this.first, this.second, this.third, this.fourth);

  @override
  String toString() => '($first, $second, $third, $fourth)';
}
