import 'package:flutter/material.dart';
import '../models/data.dart';

class StaticData extends StatelessWidget {
  final CompleteData data;
  StaticData(this.data);
  @override
  Widget build(BuildContext context) {
    return Column(
      children: <Widget>[
        Padding(
          padding: const EdgeInsets.only(bottom: 8.0),
          child: Text(
            'Lokations info',
            style: TextStyle(fontSize: 24.0),
          ),
        ),

          textRow('Antal lokationer', '${data.locationsTotal}'),
          clickableRow('Lokationer uden plan', '${data.locationsWithoutTasks}', context, onClickLocationsWithoutPlan),
          clickableRow('Lokationer uden personale', '${data.locationsWithoutStaff}', context, onClickLocationsWithoutStaff),
          clickableRow('Lokationer uden service leder', '${data.locationsWithoutServiceLeader}', context, onClickLocationsWithoutServiceLeader),
          textRow('Uafsluttede kvalitetsreporter', '${data.incompleteReports}'),
          textRow('Uafsluttede merarbejde', '${data.incompleteMorework}'),
          textRow('Reporter årligt', '${data.reportsYearly.toStringAsFixed(1)}'),
          textRow('Reporter pr kvatal', '${(data.reportsYearly / 4.0).toStringAsFixed(1)}'),
          textRow('Reporter dagligt', '${(data.reportsYearly / 252.0).toStringAsFixed(1)}'),

      ],
    );
  }

  clickableRow(name, value, context, onClick) {
    return FlatButton(
      padding: EdgeInsets.all(0.0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(name),
          Text(value),
        ],
      ),
      onPressed: () {
        onClick(context);
      },
    );
  }

  textRow(name, value) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(name),
        Text(value),
      ],
    );
  }

  onClickLocationsWithoutPlan(context) {
    Navigator.pushNamed(context, 'locationsWithoutPlan/${data.customerId}/${data.userId}');
  }
  onClickLocationsWithoutStaff(context) {
    Navigator.pushNamed(context, 'locationsWithoutStaff/${data.customerId}/${data.userId}');
  }
  onClickLocationsWithoutServiceLeader(context) {
    Navigator.pushNamed(context, 'locationsWithoutServiceLeader/${data.customerId}/0');
  }
}
