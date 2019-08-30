import 'package:bms_dart/quality_report_item_list_bloc.dart';
import 'package:bms_flutter/src/widgets/bloc_list_half_screen.dart';
import 'package:flutter/material.dart';
import 'package:bms_dart/models.dart';

import 'quality_report_item_list.dart';

class QualityReportItemListScreen extends StatefulWidget {
  static Future show(
    BuildContext context, {
    @required QualityReportItemListBloc Function(BuildContext) blocBuilder,
    Function(QualityReportItem, int) onRatingSelect,
    Function(QualityReportItem) onCommentSelect,
    Function(QualityReportItem) onCameraSelect,
    Function(QualityReportItem) onImageSelect,
    Widget floatingActionButton,
    List<Widget> actions,
  }) {
    return Navigator.of(context).push(MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(actions: actions),
        body: QualityReportItemListScreen(
          onRatingSelect: onRatingSelect,
          onCommentSelect: onCommentSelect,
          onCameraSelect: onCameraSelect,
          onImageSelect: onImageSelect,
          floatingActionButton: floatingActionButton,
          blocBuilder: blocBuilder,
        ),
      ),
    ));
  }

  final QualityReportItemListBloc Function(BuildContext) blocBuilder;
  final Widget floatingActionButton;
  final Function(QualityReportItem, int) onRatingSelect;
  final Function(QualityReportItem) onCommentSelect;
  final Function(QualityReportItem) onCameraSelect;
  final Function(QualityReportItem) onImageSelect;

  QualityReportItemListScreen({
    Key key,
    @required this.blocBuilder,
    this.floatingActionButton,
    this.onRatingSelect,
    this.onCommentSelect,
    this.onCameraSelect,
    this.onImageSelect,
  }) : super(key: key);

  @override
  _QualityReportItemListScreenState createState() =>
      _QualityReportItemListScreenState();
}

class _QualityReportItemListScreenState
    extends State<QualityReportItemListScreen>
    with AutomaticKeepAliveClientMixin {
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return BlocListHalfScreen<
        QualityReportItemListBloc,
        QualityReportItemListEvent,
        ListState<QualityReportItem>,
        QualityReportItem>(
      blocBuilder: widget.blocBuilder,
      builder: (context, bloc, state) {
        return QualityReportItemList(
          onRatingSelect: widget.onRatingSelect,
          onCommentSelect: widget.onCommentSelect,
          onCameraSelect: widget.onCameraSelect,
          onImageSelect: widget.onImageSelect,
        );
      },
      floatingActionButton: widget.floatingActionButton,
    );
  }

  @override
  bool get wantKeepAlive => true;
}
