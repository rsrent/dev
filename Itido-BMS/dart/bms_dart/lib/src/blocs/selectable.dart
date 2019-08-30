abstract class Selectable<M> {
  void clear();
  void selectAll();
  void toggleSelectable();

  bool equal(M m1, M m2);
  bool isSelected(M m);
}
