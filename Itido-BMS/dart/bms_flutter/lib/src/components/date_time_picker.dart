import 'package:flutter/material.dart';
import 'input_dropdown.dart';
import '../../translations.dart';

class DateTimePicker extends StatelessWidget {
  const DateTimePicker({
    Key key,
    this.labelText,
    this.selectedDate,
    this.selectedTime,
    this.selectDate,
    this.selectTime,
    this.firstDate,
    this.lastDate,
  }) : super(key: key);

  final String labelText;
  final DateTime selectedDate;
  final TimeOfDay selectedTime;
  final ValueChanged<DateTime> selectDate;
  final ValueChanged<TimeOfDay> selectTime;
  final DateTime firstDate;
  final DateTime lastDate;

  Future<void> _selectDate(BuildContext context) async {
    print(Translations.of(context).dateString(selectedDate));
    print(Translations.of(context).dateString(firstDate));

    final DateTime picked = await showDatePicker(
      context: context,
      initialDate: selectedDate,
      firstDate: firstDate ?? DateTime(2015, 8),
      lastDate: lastDate ?? DateTime(2101),
    );
    if (picked != null && picked != selectedDate) selectDate(picked);
  }

  Future<void> _selectTime(BuildContext context) async {
    final TimeOfDay picked = await showTimePicker(
      context: context,
      initialTime: selectedTime,
    );
    if (picked != null && picked != selectedTime) selectTime(picked);
  }

  @override
  Widget build(BuildContext context) {
    final TextStyle valueStyle = Theme.of(context).textTheme.title;
    var dateString = Translations.of(context).dateString(selectedDate);
    return Row(
      crossAxisAlignment: CrossAxisAlignment.end,
      children: <Widget>[
        if (selectDate != null)
          Expanded(
            flex: 4,
            child: ValueInputDropdown(
              labelText: labelText,
              valueText: dateString,
              valueStyle: valueStyle,
              onPressed: () {
                _selectDate(context);
              },
            ),
          ),
        if (selectTime != null && selectDate != null)
          const SizedBox(width: 12.0),
        if (selectTime != null)
          Expanded(
            flex: 3,
            child: ValueInputDropdown(
              labelText: selectDate == null ? labelText : null,
              valueText:
                  selectedTime != null ? selectedTime.format(context) : '-',
              valueStyle: valueStyle,
              onPressed: () {
                _selectTime(context);
              },
            ),
          ),
      ],
    );
  }
}
