import 'dart:convert';
import 'dart:async';
import 'package:bms_dart/query_result.dart';
import 'package:flutter/widgets.dart';
export 'package:bms_dart/query_result.dart';
import 'package:http/http.dart';

typedef T Converter<T>(dynamic);

class QueryClientController<T> {
  bool _printMethod = true;
  bool _printPath = true;
  bool _printStatusCode = true;
  bool _printBody = true;
  bool _printError = true;

  final Client _client;
  final Converter<T> converter;
  final Map<String, String> Function() getHeaders;

  QueryClientController({
    this.converter,
    Client client,
    @required this.getHeaders,
  }) : this._client = client ?? Client();

  void close() {
    _client.close();
  }

  Future<QueryResult<T>> get(url) async {
    try {
      var response = await _client.get(
        url,
        headers: getHeaders(),
      );
      printMethod('get');
      printPath(url);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 200 && response.body != null) {
        var body = json.decode(response.body);
        var result = converter != null ? converter(body) : body;
        return Ok(result);
      }
      if (response.statusCode == 401) return Unauthorized();
    } catch (e) {
      printError(e);
    }
    return Error();
  }

  Future<QueryResult<List<T>>> getMany(url) async {
    try {
      var response = await _client.get(
        url,
        headers: getHeaders(),
      );
      printMethod('getMany');
      printPath(url);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 200 && response.body != null) {
        var body = json.decode(response.body);
        var list = List.castFrom(body);
        var result = converter != null ? list.map(converter).toList() : list;
        return Ok(result);
      }
      if (response.statusCode == 401) return Unauthorized();
    } catch (e) {
      printError('$url: $e');
    }
    return Error();
  }

  Future<QueryResult<int>> postId(url, {body, Encoding encoding}) async {
    try {
      var _body = body is String ? body : json.encode(body);
      var response = await _client.post(
        url,
        headers: getHeaders(),
        body: _body,
        encoding: encoding,
      );

      printMethod('postId');
      printPath(url);

      print(_body);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 200 && response.body != null) {
        int body = json.decode(response.body);
        return Ok(body);
      }
      if (response.statusCode == 401) return Unauthorized();
    } catch (e) {
      printError(e);
    }
    return Error();
  }

  Future<QueryResult<bool>> postNoContent(url,
      {body, Encoding encoding}) async {
    try {
      var response = await _client.post(
        url,
        headers: getHeaders(),
        body: body is String ? body : json.encode(body),
        encoding: encoding,
      );
      printMethod('postNoContent');
      printPath(url);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 204) {
        return NoContent();
      }
      if (response.statusCode == 401) return Unauthorized();
    } catch (e) {
      printError(e);
    }
    return Error();
  }

  Future<QueryResult<bool>> put(url, {body, Encoding encoding}) async {
    try {
      var response = await _client.put(
        url,
        headers: getHeaders(),
        body: body is String ? body : json.encode(body),
        encoding: encoding,
      );
      printMethod('put');
      printMethod(body);
      printPath(url);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 204 || response.statusCode == 200) {
        return NoContent();
      }
      if (response.statusCode == 401) return Unauthorized();
    } catch (e) {
      printError(e);
    }
    return Error();
  }

  void printMethod(method) {
    if (_printMethod) print('method: $method');
  }

  void printPath(url) {
    if (_printPath) print('path: $url');
  }

  void printStatusCode(Response response) {
    if (_printStatusCode) print('StatusCode: ${response.statusCode}');
  }

  void printBody(Response response) {
    if (_printBody) print('Body: ${response.body}');
  }

  void printError(e) {
    if (_printError) print('Error: $e');
  }
}
