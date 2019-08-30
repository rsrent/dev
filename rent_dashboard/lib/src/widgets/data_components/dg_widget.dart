import 'package:flutter/material.dart';
import '../../models/data.dart';

class DgWidget extends StatelessWidget {
  CompleteData simpleData;
  //final double dg;
  DgWidget(this.simpleData);

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: [
          _buildElement('${(simpleData.dg*100).toStringAsFixed(2)}%', 'Dg'),
          _buildDivider(),
          Text(
            'Lokationer med mangler:',
            textAlign: TextAlign.center,
          ),
          Row(
            mainAxisSize: MainAxisSize.max,
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              _buildElement('${simpleData.locationsWithoutServiceLeader}',
                  'Serviceleder'),
              _buildElement('${simpleData.locationsWithoutStaff}', 'Personale'),
              _buildElement('${simpleData.locationsWithoutTasks}', 'Opgaver'),
            ],
          ),
          _buildDivider(),
          Text(
            'Ikke afsluttede:',
            textAlign: TextAlign.center,
          ),
          Row(
            mainAxisSize: MainAxisSize.max,
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              _buildElement(
                  '${simpleData.incompleteReports}', 'Kvalitetsreporter'),
              _buildElement('${simpleData.incompleteMorework}', 'Merarbejde'),
            ],
          ),
          _buildDivider(),
          Text(
            'Rapporter:',
            textAlign: TextAlign.center,
          ),
          Row(
            mainAxisSize: MainAxisSize.max,
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              _buildElement(
                  '${simpleData.reportsYearly.toStringAsFixed(1)}', 'Årligt'),
              _buildElement(
                  '${(simpleData.reportsYearly / 4.0).toStringAsFixed(1)}',
                  'Pr kvatal'),
              _buildElement(
                  '${(simpleData.reportsYearly / 252.0).toStringAsFixed(1)}',
                  'Pr dag'),
            ],
          ),
          /*
          Row(
            children: [
              
            ],
          ),*/
        ],
      ),
    );
  }

  Widget _buildDivider() => Divider(
        height: 6.0,
      );

  Widget _buildElement(title, subtitle) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.center,
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(
          '$title',
          style: TextStyle(fontSize: 24.0, color: Colors.black),
          textAlign: TextAlign.center,
        ),
        Text(
          '$subtitle',
          style: TextStyle(fontSize: 16.0, color: Colors.grey),
          textAlign: TextAlign.center,
          maxLines: 2,
        ),
      ],
    );
  }
}
