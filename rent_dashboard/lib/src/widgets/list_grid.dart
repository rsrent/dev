import 'package:flutter/material.dart';

typedef Widget GetTile(int index);

class ListGrid extends StatelessWidget {
  final bool hasData;
  final int length;
  final GetTile getTile;
  final double ratio;

  ListGrid({this.hasData: false, this.length, this.getTile, this.ratio: 2.3});

  @override
  Widget build(BuildContext context) {
    if (hasData == null || !hasData)
      return Center(child: CircularProgressIndicator());

    if (MediaQuery.of(context).size.width > 1000.0) {
      return GridView.builder(
        gridDelegate: SliverGridDelegateWithMaxCrossAxisExtent(
            maxCrossAxisExtent: 600.0,
            childAspectRatio:
                ratio), //SliverGridDelegateWithFixedCrossAxisCount( crossAxisCount: MediaQuery.of(context).size.width > 800.0 ? 2 : 1, childAspectRatio: 2.0),
        itemCount: length,
        itemBuilder: (context, index) {
          return getTile(index);
        },
      );
    } else {
      return ListView.builder(
        itemCount: length,
        itemBuilder: (context, index) {
          return getTile(index);
          //return ;
        },
      );
    }
  }
}
