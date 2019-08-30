import 'package:bms_dart/models.dart';
import 'package:bms_flutter/src/resources/client_controller.dart';
import 'package:http/http.dart' as http show Client;
import 'dart:convert';
import 'package:bms_dart/repositories.dart';
import 'api_path.dart';

class QualityReportApi extends QualityReportSource {
  //Client _client;

  ClientController<QualityReport> _client;
  ClientController<QualityReportItem> _itemClient;
  String path = '${api.path}/api/QualityReports';

  QualityReportApi({
    http.Client client,
  }) {
    _client = ClientController<QualityReport>(
        converter: (json) => QualityReport.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
    _itemClient = ClientController<QualityReportItem>(
        converter: (json) => QualityReportItem.fromJson(json),
        client: client,
        getHeaders: () => api.headers());
  }

  @override
  void dispose() {
    _client.close();
  }

  @override
  Future<List<QualityReport>> fetchOfProjectItem(int projectItemId) async {
    var result = await _client.getMany('$path/GetOfProjectItem/$projectItemId');
    return result;
  }

  @override
  Future<int> create(int projectItemId, {QualityReport qualityReport}) {
    if (qualityReport != null)
      return _client.postId(
        '$path/CreateForProjectItemWithBody/$projectItemId',
        body: qualityReport.toMap(),
      );
    else
      return _client.postId(
        '$path/CreateForProjectItem/$projectItemId',
      );
  }

  @override
  Future<bool> update(int qualityReportId, QualityReport qualityReport) {
    return _client.put(
      '$path/Update/$qualityReportId',
      body: qualityReport.toMap(),
    );
  }

  @override
  Future<bool> complete(int qualityReportId) {
    return _client.put(
      '$path/Complete/$qualityReportId',
    );
  }

  @override
  Future<QualityReport> fetch(int qualityReportId) {
    return _client.get('$path/Get/$qualityReportId');
  }

  @override
  Future<List<QualityReportItem>> fetchItemsOfQualityReport(
      int qualityReportId) {
    return _itemClient
        .getMany('$path/GetItemsOfQualityReport/$qualityReportId');
  }

  @override
  Future<bool> updateQualityReportItem(
      int qualityReportItemId, QualityReportItem qualityReportItem) {
    return _itemClient.put(
      '$path/UpdateItem/$qualityReportItemId',
      body: qualityReportItem.toMap(),
    );
  }
}
