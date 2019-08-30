import 'package:bms_dart/quality_report_item_list_bloc.dart';
import 'package:bms_flutter/src/components/animated_bloc_builder.dart';
import 'package:bms_flutter/src/widgets/info_list_view.dart';
import 'package:bms_flutter/src/widgets/task/task_list.dart';
import 'package:bms_flutter/translations.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class QualityReportItemList extends StatelessWidget {
  final Function(QualityReportItem, int) onRatingSelect;
  final Function(QualityReportItem) onCommentSelect;
  final Function(QualityReportItem) onCameraSelect;
  final Function(QualityReportItem) onImageSelect;

  const QualityReportItemList({
    Key key,
    this.onRatingSelect,
    this.onCommentSelect,
    this.onCameraSelect,
    this.onImageSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final taskListBloc = BlocProvider.of<QualityReportItemListBloc>(context);
    return AnimatedBlocBuilder(
      bloc: taskListBloc,
      builder: (context, ListState<QualityReportItem> state) {
        if (state is Failure) {
          return InfoListView(info: Translations.of(context).infoErrorLoading);
        }

        if (state is Loaded) {
          var itemss = (state as Loaded).items;
          if (itemss.isEmpty) {
            return InfoListView(
                info: Translations.of(context).infoNoQualityReportItems);
          }
          return ListView.separated(
            padding: EdgeInsets.only(top: 20, bottom: 200),
            itemBuilder: (BuildContext context, int index) {
              return Card(
                child: QualityReportItemTile(
                  item: itemss[index],
                  onRatingSelect: onRatingSelect,
                  onCommentSelect: onCommentSelect,
                  onCameraSelect: onCameraSelect,
                  onImageSelect: onImageSelect,
                ),
              );
            },
            itemCount: itemss.length,
            separatorBuilder: (context, index) => Divider(),
          );
        }
      },
    );
  }
}

class QualityReportItemTile extends StatelessWidget {
  final QualityReportItem item;
  final Function(QualityReportItem, int) onRatingSelect;
  final Function(QualityReportItem) onCommentSelect;
  final Function(QualityReportItem) onCameraSelect;
  final Function(QualityReportItem) onImageSelect;

  const QualityReportItemTile({
    Key key,
    @required this.item,
    this.onRatingSelect,
    this.onCommentSelect,
    this.onCameraSelect,
    this.onImageSelect,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: <Widget>[
        TaskTile(
          task: item.task,
        ),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: <Widget>[
            IconButton(
              icon: Icon(
                Icons.sentiment_satisfied,
                color: item.rating == 1 ? Colors.green : Colors.grey,
              ),
              onPressed:
                  onRatingSelect != null ? () => onRatingSelect(item, 1) : null,
            ),
            IconButton(
              icon: Icon(
                Icons.sentiment_neutral,
                color: item.rating == 2 ? Colors.orange : Colors.grey,
              ),
              onPressed:
                  onRatingSelect != null ? () => onRatingSelect(item, 2) : null,
            ),
            IconButton(
              icon: Icon(
                Icons.sentiment_dissatisfied,
                color: item.rating == 3 ? Colors.red : Colors.grey,
              ),
              onPressed:
                  onRatingSelect != null ? () => onRatingSelect(item, 3) : null,
            ),
            Expanded(
              child: Container(),
            ),
            if (item.imageLocation != null)
              IconButton(
                icon: Icon(
                  Icons.image,
                ),
                onPressed:
                    onImageSelect != null ? () => onImageSelect(item) : null,
              ),
            IconButton(
              icon: Icon(
                Icons.photo_camera,
              ),
              onPressed:
                  onCameraSelect != null ? () => onCameraSelect(item) : null,
            ),
          ],
        ),
        Row(
          children: <Widget>[
            Expanded(
              child: Padding(
                child: Text('${item.comment ?? 'Kommentar...'}'),
                padding: EdgeInsets.all(16),
              ),
            ),
            IconButton(
              icon: Icon(
                Icons.comment,
              ),
              onPressed:
                  onCommentSelect != null ? () => onCommentSelect(item) : null,
            ),
          ],
        )
      ],
    );
  }
}
