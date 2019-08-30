DateTime toDateTime(String text) {
  if (text == null) return null;

  var splits = text.split('.');

  var lastLength = splits.last.length;

  if (lastLength == 6 || lastLength == 7) {
    text = text.substring(0, text.length - (lastLength - 4));
  }

  DateTime result;
  try {
    result = DateTime.tryParse(text);
  } catch (e) {
    print('toDateTime Error: $e');
  }
  return result;
}
