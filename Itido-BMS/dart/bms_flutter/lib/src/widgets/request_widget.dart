import 'package:bms_dart/models.dart';
import 'package:flutter/material.dart';
import '../../style.dart';
import '../../translations.dart';

enum _RequestWidgetAlignment {
  Vertical,
  Horizontal,
  Info,
}

class RequestWidget extends StatelessWidget {
  final double height;
  final double width;
  final Request request;
  final Function(bool) onRespond;
  final _RequestWidgetAlignment requestWidgetAlignment;

  const RequestWidget.vertical({
    Key key,
    this.height = 100,
    this.width = 100,
    @required this.request,
    @required this.onRespond,
  })  : this.requestWidgetAlignment = _RequestWidgetAlignment.Vertical,
        super(key: key);

  const RequestWidget.horizontal({
    Key key,
    @required this.request,
    @required this.onRespond,
  })  : this.requestWidgetAlignment = _RequestWidgetAlignment.Horizontal,
        this.width = 0,
        this.height = 0,
        super(key: key);

  const RequestWidget.info({
    Key key,
    @required this.request,
  })  : this.requestWidgetAlignment = _RequestWidgetAlignment.Info,
        this.width = 0,
        this.height = 0,
        this.onRespond = null,
        super(key: key);

  @override
  Widget build(BuildContext context) {
    if (this.requestWidgetAlignment == _RequestWidgetAlignment.Info) {
      return request.approvalState == ApprovalState.Pending
          ? Text(Translations.of(context).infoPending)
          : request.approvalState == ApprovalState.Approved
              ? Text(
                  Translations.of(context).infoApproved,
                  style: TextStyle(color: acceptGreen),
                )
              : Text(
                  Translations.of(context).infoDeclined,
                  style: TextStyle(color: declineRed),
                );
    } else if (this.requestWidgetAlignment ==
        _RequestWidgetAlignment.Horizontal) {
      return ButtonBar(
        children: <Widget>[
          if (request.approvalState == ApprovalState.Pending &&
              request.canRespondToApprovalState)
            FlatButton(
              child: Text(
                Translations.of(context).buttonApprove,
                style: TextStyle(color: acceptGreen),
              ),
              onPressed: () => onRespond(true),
            ),
          if (request.approvalState == ApprovalState.Pending &&
              request.canRespondToApprovalState)
            FlatButton(
              child: Text(
                Translations.of(context).buttonDeline,
                style: TextStyle(color: declineRed),
              ),
              onPressed: () => onRespond(false),
            ),
          if (request.approvalState == ApprovalState.Pending &&
              !request.canRespondToApprovalState)
            Text(Translations.of(context).infoPending),
          if (request.approvalState == ApprovalState.Approved)
            Text(
              Translations.of(context).infoApproved,
              style: TextStyle(color: acceptGreen),
            ),
          if (request.approvalState == ApprovalState.Denied)
            Text(
              Translations.of(context).infoDeclined,
              style: TextStyle(color: declineRed),
            )
        ],
      );
    } else if (this.requestWidgetAlignment ==
        _RequestWidgetAlignment.Vertical) {
      return SizedBox(
          width: width,
          height: height,
          child: request.approvalState == ApprovalState.Pending
              ? request.canRespondToApprovalState
                  ? Row(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: <Widget>[
                        Container(
                          width: 1,
                          color: Colors.grey[400],
                        ),
                        Expanded(
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: <Widget>[
                              Expanded(
                                child: InkWell(
                                  child: Container(
                                    child: Center(
                                      child: Text(
                                        Translations.of(context).buttonApprove,
                                        style: TextStyle(color: acceptGreen),
                                      ),
                                    ),
                                  ),
                                  onTap: () => onRespond(true),
                                ),
                              ),
                              Container(
                                color: Colors.grey[400],
                                height: 1,
                              ),
                              Expanded(
                                child: InkWell(
                                  child: Container(
                                    child: Center(
                                      child: Text(
                                        Translations.of(context).buttonDeline,
                                        style: TextStyle(color: declineRed),
                                      ),
                                    ),
                                  ),
                                  onTap: () => onRespond(false),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    )
                  : Center(child: Text(Translations.of(context).infoPending))
              : request.approvalState == ApprovalState.Approved
                  ? Center(
                      child: Text(
                      Translations.of(context).infoApproved,
                      style: TextStyle(color: acceptGreen),
                    ))
                  : Center(
                      child: Text(
                      Translations.of(context).infoDeclined,
                      style: TextStyle(color: declineRed),
                    )));
    }
    return Container();
  }
}
