enum IsOneOrTheOther { One, Other }

class OneOrTheOther<A, B> {
  final A one;
  final B other;
  final IsOneOrTheOther oneOrTheOther;

  OneOrTheOther.one(this.one)
      : this.other = null,
        this.oneOrTheOther = IsOneOrTheOther.One;
  OneOrTheOther.other(this.other)
      : this.one = null,
        this.oneOrTheOther = IsOneOrTheOther.Other;
}
