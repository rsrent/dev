abstract class Searchable<M> {
  List<M> loaded = [];
  String searchText = '';
  List<String> filters = [];

  void searchTextUpdated(String searchText);
  List<M> filtered(String filter, List<String> filters);

  List<String> allFilters();
  List<String> initialFilters();

  void toggleFilter(String filter) {
    if (filters.contains(filter)) {
      filters.remove(filter);
    } else {
      filters.add(filter);
    }
    searchTextUpdated(searchText);
  }
}
